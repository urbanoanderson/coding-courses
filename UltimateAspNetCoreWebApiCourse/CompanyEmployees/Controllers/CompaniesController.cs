using AutoMapper;
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

        [HttpGet("{id}")]
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
    }
}
