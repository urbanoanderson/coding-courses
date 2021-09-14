﻿using AutoMapper;
using CompanyEmployees.ActionFilters;
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
    [ApiVersion("1.0")]
    [Route("api/companies")]
    [ApiController]
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

        [HttpOptions]
        public IActionResult GetCompaniesOptions()
        {
            this.Response.Headers.Add("Allow", "GET, OPTIONS, POST");

            return this.Ok();
        }

        [HttpGet(Name = "GetCompanies")]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await this.repository.Company.GetAllCompaniesAsync(trackChanges: false);

            var companiesDto = this.mapper.Map<IEnumerable<CompanyDto>>(companies);

            return this.Ok(companiesDto);
        }

        [HttpGet("{id}", Name = "CompanyById")]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            Company company = await this.repository.Company.GetCompanyAsync(id, trackChanges: false);

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

        [HttpGet("collection/({ids})", Name = "CompanyCollection")]
        public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                this.logger.LogError($"Parameter '{nameof(ids)}' is null");
                return this.BadRequest($"Parameter '{nameof(ids)}' is null");
            }

            IEnumerable<Company> companies = await this.repository.Company.GetByIdsAsync(ids, trackChanges: false);

            if (ids.Count() != companies.Count())
            {
                this.logger.LogError("Some ids are not valid in a collection");
                return this.NotFound();
            }

            IEnumerable<CompanyDto> companiesToReturn = this.mapper.Map<IEnumerable<CompanyDto>>(companies);

            return this.Ok(companiesToReturn);
        }

        [HttpPost(Name = "CreateCompany")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCompany([FromBody]CompanyForCreationDto companyDto)
        {
            Company company = this.mapper.Map<Company>(companyDto);

            this.repository.Company.CreateCompany(company);
            await this.repository.SaveAsync();

            CompanyDto companyToReturn = this.mapper.Map<CompanyDto>(company);

            return this.CreatedAtRoute("CompanyById", new { id = companyToReturn.Id }, companyToReturn);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateCompanyCollection([FromBody]IEnumerable<CompanyForCreationDto> companyCollectionDto)
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

            await this.repository.SaveAsync();

            IEnumerable<CompanyDto> companiesToReturn = this.mapper.Map<IEnumerable<CompanyDto>>(companies);
            var ids = string.Join(",", companiesToReturn.Select(c => c.Id));

            return this.CreatedAtRoute("CompanyCollection", new { ids }, companiesToReturn);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            Company company = this.HttpContext.Items["company"] as Company;

            this.repository.Company.DeleteCompany(company);
            await this.repository.SaveAsync();

            return this.NoContent();
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody]CompanyForUpdateDto companyDto)
        {
            Company company = this.HttpContext.Items["company"] as Company;

            this.mapper.Map(companyDto, company);
            await this.repository.SaveAsync();

            return this.NoContent();
        }
    }
}
