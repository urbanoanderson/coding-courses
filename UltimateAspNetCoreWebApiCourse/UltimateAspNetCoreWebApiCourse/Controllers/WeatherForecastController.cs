using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UltimateAspNetCoreWebApiCourse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILoggerManager _logger;

        public WeatherForecastController(ILoggerManager logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            this._logger.LogInfo("Info message from our values controller");
            this._logger.LogDebug("Debug message from our values controller");
            this._logger.LogWarn("Warn message from our values controller");
            this._logger.LogError("Error message from our values controller");

            return new string[] { "value1", "value2" };
        }
    }
}
