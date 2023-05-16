using CoreApiSamples.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace CoreApiSamples.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }


        [HttpGet]
        public ActionResult<IList<Patient>> GetAll()
        {
            return Ok(_patientService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Patient> GetByIdentity([FromRoute] Guid id)
        {
            var patient = _patientService.GetByIdentity(id);
            if (patient != null)
            {
                return patient;
            }
            return NotFound();
        }

        [HttpPost]
        public ActionResult<Patient> Post([FromBody, BindRequired] Patient patient)
        {
            return _patientService.CreatePatient(patient);
        }
       
    }
}
