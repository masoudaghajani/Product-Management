using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers;

namespace Presentation.Filter
{
    public class ExceptionAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<HomeController> _logger;
        public ExceptionAttribute(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {

            context.Result = new ViewResult()
            {
                ViewName = "Error",
            };
            _logger.LogError(context.Exception.ToString());
        }
    }
}
