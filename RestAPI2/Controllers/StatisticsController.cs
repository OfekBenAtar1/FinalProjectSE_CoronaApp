
using CoronaProgram.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoronaProgram.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly ILogger<LabTestsController> _logger;

        public StatisticsController(ILogger<LabTestsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetStatistics()
        {
            return Ok(CovidMgmtApp.GetInstance().Persons.GetStatistics());
        }
    }
}
