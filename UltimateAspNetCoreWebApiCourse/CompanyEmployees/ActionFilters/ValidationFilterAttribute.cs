using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.ActionFilters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        private readonly ILoggerManager logger;

        public ValidationFilterAttribute(ILoggerManager logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var action = context.RouteData.Values["action"];
            var controller = context.RouteData.Values["controller"];

            var param = context.ActionArguments.SingleOrDefault(x => x.Value.ToString().Contains("Dto")).Value;

            if (param == null)
            {
                this.logger.LogError($"Object received from client is null. Controller: {controller}, Action: {action}");
                context.Result = new BadRequestObjectResult($"Object is null. Controller: {controller}, Action: {action}");
                return;
            }

            if (!context.ModelState.IsValid)
            {
                this.logger.LogError($"Invalid model state for the object. Controller: {controller}, Action: {action}");
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
