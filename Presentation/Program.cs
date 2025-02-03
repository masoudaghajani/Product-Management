using Autofac.Extensions.DependencyInjection;
using Autofac;
using Presentation.MiddleWare;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.MSSqlServer;
using Microsoft.AspNetCore.RateLimiting;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Autofac.Core;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var Connection = builder.Configuration.GetConnectionString("DefaultConnection");
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);


#region serilog
var sinkOpts = new MSSqlServerSinkOptions();
sinkOpts.TableName = "Logs";
var columnOpts = new ColumnOptions();
columnOpts.Store.Remove(StandardColumn.Properties);
columnOpts.Store.Remove(StandardColumn.MessageTemplate);
columnOpts.Store.Remove(StandardColumn.Level);
columnOpts.Store.Remove(StandardColumn.Exception);

builder.Host.UseSerilog((context, Services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(Services)

    .WriteTo.MSSqlServer(
                    connectionString: Connection,
                    sinkOptions: sinkOpts,
                    restrictedToMinimumLevel: LogEventLevel.Error,
                    columnOptions: columnOpts

)
.WriteTo.File(new JsonFormatter(), "logs/error-logs-.json", rollingInterval: RollingInterval.Day).MinimumLevel.Error());

#endregion
#region Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new AutofacModule());


});
#endregion
#region RateLimiter
int RateLimiterStatusCode = int.Parse(builder.Configuration.GetSection("RateLimiter")["StatusCode"]);
int RateLimiterPermitLimit = int.Parse(builder.Configuration.GetSection("RateLimiter")["PermitLimit"]);
int RateLimiterQueueLimit = int.Parse(builder.Configuration.GetSection("RateLimiter")["QueueLimit"]);
builder.Services.AddRateLimiter(option =>
{
    option.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = RateLimiterStatusCode;
        await context.HttpContext.Response.WriteAsync("Too many requests. Please try later again... ", cancellationToken: token);
    };
    option.AddFixedWindowLimiter("SlowDown", windowsOption =>
    {
        windowsOption.Window = TimeSpan.FromSeconds(1);
        windowsOption.PermitLimit = RateLimiterPermitLimit;
        windowsOption.QueueLimit = RateLimiterQueueLimit;
        windowsOption.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });

});
#endregion
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true
    };
    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            context.HandleResponse(); // Prevents default response
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var errorResponse = new { Error = "Unauthorized. Invalid or missing token." };
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    };
});
// Add services to the container.
builder.Services.AddDbContextPool<ApplicationDbContext>(options => options.UseSqlServer(Connection));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

});
builder.Logging.AddSerilog();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();  // Ensure authentication is used
app.MapControllers();

app.Run();
