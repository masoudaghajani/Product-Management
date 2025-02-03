using Autofac;
using Domain.Contracts;
using MediatR;
using Microsoft.Extensions.DependencyModel;
using Presentation.Filter;
using System.Reflection;
using Module = Autofac.Module;


namespace Presentation.MiddleWare
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var endPointApplicationAssemblies = GetAllAssemblies().ToArray();
            builder.RegisterAssemblyTypes(endPointApplicationAssemblies)
                           .AssignableTo<IScopedDependency>()
                           .AsImplementedInterfaces()
                           .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(endPointApplicationAssemblies)
              .AssignableTo<ITransientDependency>()
              .AsImplementedInterfaces()
              .InstancePerDependency();

            builder.RegisterAssemblyTypes(endPointApplicationAssemblies)
             .AssignableTo<ISingletonDependency>()
             .AsImplementedInterfaces()
             .SingleInstance();
            builder.RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ExceptionAttribute>()
              
               .InstancePerLifetimeScope();
            builder.RegisterType<JwtService>()

              .InstancePerLifetimeScope();

        }
        public static IEnumerable<Assembly> GetAllAssemblies()
        {
            var platform = Environment.OSVersion.Platform.ToString();
            var runtimeAssemblyNames = DependencyContext.Default.GetRuntimeAssemblyNames(platform);

            var res = new List<Assembly>();

            foreach (var assembly in runtimeAssemblyNames)
            {
                try
                {
                    res.Add(Assembly.Load(assembly.FullName));
                }
                catch (Exception ex)
                {
                   // Log.Logger.Error(ex.ToString());
                    throw;
                }
            }

            return res;
        }
    }
}
