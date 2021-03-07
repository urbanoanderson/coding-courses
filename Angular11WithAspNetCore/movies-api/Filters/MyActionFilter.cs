using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace MoviesAPI.Filters
{
    public class MyActionFilter : IActionFilter
    {
        private ILogger<MyActionFilter> logger;

        public MyActionFilter(ILogger<MyActionFilter> logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            this.logger.LogTrace("OnActionExecuting");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            this.logger.LogTrace("OnActionExecuted");
        }
    }
}