using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
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
            try
            {
                var companies = this.repository.Company.GetAllCompanies(trackChanges: false);
                var companiesDto = this.mapper.Map<IEnumerable<CompanyDto>>(companies);

                return this.Ok(companiesDto);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Something went wrong in the {nameof(GetCompanies)} action {ex}");
                return this.StatusCode(500, "Internal Server Error");
            }
        }
    }
}
