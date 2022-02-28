
using CoronaProgram.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoronaProgram.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LabTestsController : ControllerBase
    {
        private readonly ILogger<LabTestsController> _logger;

        public LabTestsController(ILogger<LabTestsController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult AddLabTest([FromBody] LabTestResult testResult )
        {
            var result = CovidMgmtApp.GetInstance().LabTests.AddNewTest(testResult);
            if (result == null)
                return BadRequest("Can't Add This Test");
            return Ok(new { PatientID = result.patientID });
        }
  

    }
}
