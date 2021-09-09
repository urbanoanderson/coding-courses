using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.ActionFilters
{
    public class ValidateEmployeeForCompanyExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager repository;

        private readonly ILoggerManager logger;

        public ValidateEmployeeForCompanyExistsAttribute(IRepositoryManager repository, ILoggerManager logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string method = context.HttpContext.Request.Method;
            Guid companyId = (Guid)context.ActionArguments["companyId"];
            Company company = await this.repository.Company.GetCompanyAsync(companyId, false);

            if (company == null)
            {
                this.logger.LogInfo($"Company with id '{companyId}' doesn't exist in the database.");
                context.Result = new NotFoundResult();
            }

            Guid id = (Guid)context.ActionArguments["id"];
            bool trackChanges = method.Equals("PUT") || method.Equals("PATCH");
            Employee employee = await this.repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);

            if (employee == null)
            {
                this.logger.LogInfo($"Employee with id '{id}' doesn't exist in the database.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("employee", employee);
                await next();
            }
        }
    }
}
