using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.ActionFilters
{
    public class ValidateCompanyExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager repository;

        private readonly ILoggerManager logger;

        public ValidateCompanyExistsAttribute(IRepositoryManager repository, ILoggerManager logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            bool trackChanges = context.HttpContext.Request.Method.Equals("PUT");
            Guid id = (Guid)context.ActionArguments["id"];
            var company = await this.repository.Company.GetCompanyAsync(id, trackChanges);

            if (company == null)
            {
                this.logger.LogInfo($"Company with id '{id}' doesn't exist in the database.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("company", company);
                await next();
            }
        }
    }
}
