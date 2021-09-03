using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILoggerManager logger;

        private readonly IRepositoryManager repository;

        public WeatherForecastController(ILoggerManager logger, IRepositoryManager repositoryManager)
        {
            this.logger = logger;
            this.repository = repositoryManager;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            this.logger.LogInfo("Info message from our values controller");
            this.logger.LogDebug("Debug message from our values controller");
            this.logger.LogWarn("Warn message from our values controller");
            this.logger.LogError("Error message from our values controller");

            return new string[] { "value1", "value2" };
        }
    }
}
