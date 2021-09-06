using AutoMapper;
using CompanyEmployees.ModelBinders;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly IRepositoryManager repository;

        private readonly ILoggerManager logger;

        private readonly IMapper mapper;

        public CompaniesController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCompanies()
        {
            var companies = this.repository.Company.GetAllCompanies(trackChanges: false);

            var companiesDto = this.mapper.Map<IEnumerable<CompanyDto>>(companies);

            return this.Ok(companiesDto);
        }

        [HttpGet("{id}", Name = "CompanyById")]
        public IActionResult GetCompany(Guid id)
        {
            Company company = this.repository.Company.GetCompany(id, trackChanges: false);

            if (company == null)
            {
                this.logger.LogInfo($"Company with id '{id}' doesn't exist in the database.");
                return this.NotFound();
            }
            else
            {
                var companyDto = this.mapper.Map<CompanyDto>(company);
                return this.Ok(companyDto);
            }
        }

        [HttpPost]
        public IActionResult CreateCompany([FromBody]CompanyForCreationDto companyDto)
        {
            if (companyDto == null)
            {
                this.logger.LogError($"{nameof(CompanyForCreationDto)} object received from client is null.");
                return this.BadRequest($"{nameof(CompanyForCreationDto)} is null");
            }

            Company company = this.mapper.Map<Company>(companyDto);

            this.repository.Company.CreateCompany(company);
            this.repository.Save();

            CompanyDto companyToReturn = this.mapper.Map<CompanyDto>(company);

            return this.CreatedAtRoute("CompanyById", new { id = companyToReturn.Id }, companyToReturn);
        }

        [HttpGet("collection/({ids})", Name = "CompanyCollection")]
        public IActionResult GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                this.logger.LogError($"Parameter '{nameof(ids)}' is null");
                return this.BadRequest($"Parameter '{nameof(ids)}' is null");
            }

            IEnumerable<Company> companies = this.repository.Company.GetByIds(ids, trackChanges: false);

            if (ids.Count() != companies.Count())
            {
                this.logger.LogError("Some ids are not valid in a collection");
                return this.NotFound();
            }

            IEnumerable<CompanyDto> companiesToReturn = this.mapper.Map<IEnumerable<CompanyDto>>(companies);

            return this.Ok(companiesToReturn);
        }

        [HttpPost("collection")]
        public IActionResult CreateCompanyCollection([FromBody]IEnumerable<CompanyForCreationDto> companyCollectionDto)
        {
            if (companyCollectionDto == null)
            {
                this.logger.LogError("Company collection received from client is null.");
                return this.BadRequest("Company collection is null");
            }

            IEnumerable<Company> companies = this.mapper.Map<IEnumerable<Company>>(companyCollectionDto);

            foreach (var company in companies)
            {
                this.repository.Company.CreateCompany(company);
            }

            this.repository.Save();

            IEnumerable<CompanyDto> companiesToReturn = this.mapper.Map<IEnumerable<CompanyDto>>(companies);
            var ids = string.Join(",", companiesToReturn.Select(c => c.Id));

            return this.CreatedAtRoute("CompanyCollection", new { ids }, companiesToReturn);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCompany(Guid id)
        {
            Company company = this.repository.Company.GetCompany(id, trackChanges: false);

            if (company == null)
            {
                this.logger.LogInfo($"Company with id '{id}' doesn't exist in the database.");
                return this.NotFound();
            }

            this.repository.Company.DeleteCompany(company);
            this.repository.Save();

            return this.NoContent();
        }
    }
}
