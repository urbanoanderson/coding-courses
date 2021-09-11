using AutoMapper;
using CompanyEmployees.ActionFilters;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        private readonly IRepositoryManager repository;

        private readonly ILoggerManager logger;

        private readonly IMapper mapper;

        private readonly IDataShaper<EmployeeDto> dataShaper;

        public EmployeesController(IRepositoryManager repository,
            ILoggerManager logger,
            IMapper mapper,
            IDataShaper<EmployeeDto> dataShaper)
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
            this.dataShaper = dataShaper;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId, [FromQuery]EmployeeParameters employeeParameters)
        {
            if (!employeeParameters.ValidAgeRange)
                return this.BadRequest("Max age can't be less than min age.");

            Company company = await this.repository.Company.GetCompanyAsync(companyId, trackChanges: false);

            if (company == null)
            {
                this.logger.LogInfo($"Company with id '{companyId}' doesn't exist in the database.");
                return this.NotFound();
            }

            PagedList<Employee> employees = await this.repository.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges: false);

            this.Response.Headers.Add("X-Pagination",
                JsonConvert.SerializeObject(employees.MetaData));
            
            IEnumerable<EmployeeDto> employeesDto = this.mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return this.Ok(this.dataShaper.ShapeData(employeesDto, employeeParameters.Fields));
        }

        [HttpGet("{id}", Name = "GetEmployeeForCompany")]
        public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
        {
            Company company = await this.repository.Company.GetCompanyAsync(companyId, trackChanges: false);

            if (company == null)
            {
                this.logger.LogInfo($"Company with id '{companyId}' doesn't exist in the database.");
                return this.NotFound();
            }

            Employee employee = await this.repository.Employee.GetEmployeeAsync(companyId, id, trackChanges: false);

            if (employee == null)
            {
                this.logger.LogInfo($"Employee with id '{id}' doesn't exist in the database.");
                return this.NotFound();
            }

            EmployeeDto employeeDto = this.mapper.Map<EmployeeDto>(employee);

            return this.Ok(employeeDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody]EmployeeForCreationDto employeeDto)
        {
            Company company = await this.repository.Company.GetCompanyAsync(companyId, trackChanges: false);

            if (company == null)
            {
                this.logger.LogInfo($"Company with id '{companyId}' doesn't exist in the database.");
                return this.NotFound();
            }

            Employee employee = this.mapper.Map<Employee>(employeeDto);

            this.repository.Employee.CreateEmployeeForCompany(companyId, employee);
            await this.repository.SaveAsync();

            EmployeeDto employeeToReturn = this.mapper.Map<EmployeeDto>(employee);

            return this.CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = employeeToReturn.Id }, employeeToReturn);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id)
        {
            Employee employee = HttpContext.Items["employee"] as Employee;

            this.repository.Employee.DeleteEmployee(employee);
            await this.repository.SaveAsync();

            return this.NoContent();
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody]EmployeeForUpdateDto employeeDto)
        {
            Employee employee = HttpContext.Items["employee"] as Employee;

            this.mapper.Map(employeeDto, employee);
            await this.repository.SaveAsync();

            return this.NoContent();
        }

        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id,
            [FromBody]JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                this.logger.LogError("patchDoc object received from client is null.");
                return this.BadRequest("patchDoc is null");
            }

            Employee employee = HttpContext.Items["employee"] as Employee;
            EmployeeForUpdateDto employeeDto = this.mapper.Map<EmployeeForUpdateDto>(employee);

            patchDoc.ApplyTo(employeeDto, this.ModelState);
            this.TryValidateModel(employeeDto);

            if (!this.ModelState.IsValid)
            {
                this.logger.LogError($"Invalid model state for the patch document");
                return this.UnprocessableEntity(ModelState);
            }

            patchDoc.ApplyTo(employeeDto);
            this.mapper.Map(employeeDto, employee);
            await this.repository.SaveAsync();

            return this.NoContent();
        }
    }
}
