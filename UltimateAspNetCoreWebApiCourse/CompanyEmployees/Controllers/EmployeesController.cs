using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController : ControllerBase
    {
        private IRepositoryManager repository;

        private ILoggerManager logger;

        private IMapper mapper;

        public EmployeesController(IRepositoryManager repository,
            ILoggerManager logger,
            IMapper mapper)
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetEmployeesForCompany(Guid companyId)
        {
            Company company = this.repository.Company.GetCompany(companyId, trackChanges: false);

            if (company == null)
            {
                this.logger.LogInfo($"Company with id '{companyId}' doesn't exist in the database.");
                return this.NotFound();
            }

            IEnumerable<Employee> employees = this.repository.Employee.GetEmployees(companyId, trackChanges: false);
            IEnumerable<EmployeeDto> employeesDto = this.mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return this.Ok(employeesDto);
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
        {
            Company company = this.repository.Company.GetCompany(companyId, trackChanges: false);

            if (company == null)
            {
                this.logger.LogInfo($"Company with id '{companyId}' doesn't exist in the database.");
                return this.NotFound();
            }

            Employee employee = this.repository.Employee.GetEmployee(companyId, id, trackChanges: false);

            if (employee == null)
            {
                this.logger.LogInfo($"Employee with id '{id}' doesn't exist in the database.");
                return this.NotFound();
            }

            EmployeeDto employeeDto = this.mapper.Map<EmployeeDto>(employee);

            return this.Ok(employeeDto);
        }
    }
}
