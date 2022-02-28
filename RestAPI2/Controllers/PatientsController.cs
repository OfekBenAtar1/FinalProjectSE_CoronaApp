using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoronaProgram.Models;
using System;
using System.Collections.Generic;

namespace CoronaProgram.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly ILogger<PatientsController> _logger;

        public PatientsController(ILogger<PatientsController> logger)
        {
            _logger = logger;
        }

        [HttpPut]
        public IActionResult AddPatient([FromBody] Patient patient)
        {
            var result = CovidMgmtApp.GetInstance().Persons.AddNewPatient(patient);

            if (result == null)
                return BadRequest("Patient already exists");
            else
                return Ok(new { PatientID = result.PatientID });
        }
        
        [HttpGet]
        public IActionResult GetPatients()
        {
            return Ok(CovidMgmtApp.GetInstance().Persons.GetPatients());
        }

        [HttpPut]
        [Route("{id}/route")]
        public IActionResult AddLocation([FromRoute] string id, [FromBody] Location location)
        {
            var result = CovidMgmtApp.GetInstance().Persons.AddNewLocation(id, location);

            if (result == null)
                return BadRequest("Can't Add");
            else
                return Ok(result);
        }

        [HttpGet]
        [Route("{id}/route")]
        public IActionResult GetLocations([FromRoute] string id)
        {
            var result = CovidMgmtApp.GetInstance().Persons.GetLocations(id);

            if (result == null)
                return BadRequest("Cannot find this patient :(");
            else
                return Ok(result);

        }

        [HttpPut]
        [Route("{id}/encounters")]
        public IActionResult AddPotentialPatient([FromRoute] string id, [FromBody] PotentialPatient potential)
        {
            var result = CovidMgmtApp.GetInstance().Persons.AddNewPotentialPatient(id, potential);

            if (result == null)
                return BadRequest("Cannot find this patient :(");
            else
                return Ok(result);

        }


        [HttpGet]
        [Route("{id}/encounters")]
        public IActionResult GetEncounters([FromRoute] string id)
        {
            var result = CovidMgmtApp.GetInstance().Persons.GetEncounters(id);

            if (result == null)
                return BadRequest("Cannot find this patient :(");
            else
                return Ok(result);
        }

        [HttpGet]
        [Route("{id}/full")]
        public IActionResult GetPersonDetails([FromRoute] string id)
        {
            var patientDetails = CovidMgmtApp.GetInstance().Persons.GetPatientById(id);
            bool isCovidPositive = patientDetails.IsCovidPositive;
            var labResults = CovidMgmtApp.GetInstance().LabTests.GetPersonTests(id);

            var myAnonyClassObject = new {
                patientDetails = new
                {
                    patientDetails.PatientID,
                    patientDetails.GovtID,
                    patientDetails.FirstName,
                    patientDetails.LastName,
                    patientDetails.BirthDate,
                    patientDetails.PhoneNumber,
                    patientDetails.Address,
                    patientDetails.HouseResidentsAmount,
                    patientDetails.InfectedByPatientID,
                }, isCovidPositive, labResults };

            if (patientDetails == null)
                return BadRequest("Cannot find this patient :(");
            else
                return Ok(myAnonyClassObject);

        }

        [HttpGet]
        [Route("new")]
        public IActionResult GetNewPatientsSince()
        {
            var sinceArray = Request.Query["since"];

            if (sinceArray.ToArray().Length == 0)
                return BadRequest("There is no 'Since' Value");

            var sinceDate = DateTime.Parse(sinceArray[0]);

            return Ok(CovidMgmtApp.GetInstance().Persons.GetPatientsSince(sinceDate));
        }

        [HttpGet]
        [Route("potential")]
        public IActionResult GetPotentialPatients()
        {
            return Ok(CovidMgmtApp.GetInstance().Persons.GetPotentialPatients());
        }


        [HttpGet]
        [Route("isolated")]
        public IActionResult GetIsolatedPersons()
        {
            var result = CovidMgmtApp.GetInstance().Persons.GetIsolatedPersons();
            if (result == null)
                BadRequest("Error : There is Person person");
            return Ok(result);
        }


        [HttpPost]
        [Route("potential/{potentialPatientId}")]
        public IActionResult TransformPotentialPatient([FromRoute] string potentialPatientId, [FromBody] Patient patient)
        {
            var p = CovidMgmtApp.GetInstance().Persons.TransformPotentialPatient(potentialPatientId, patient);
            if (p == null)
                return BadRequest("Error With Your Values");
            return Ok(p);
        }
    }
}
