using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGMH.Healthcare.Vedant.API.Extensions;
using SGMH.Healthcare.Vedant.Business.Domain;
using SGMH.Healthcare.Vedant.Business.Interfaces;
using System.Collections.Generic;

namespace SGMH.Healthcare.Vedant.API.Controllers
{
    [Authorize]
    [Route("api/patients")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientLogic _patientLogic;
        private readonly IValidator<PatientModel> _patientValidator;

        public PatientsController(IPatientLogic patientLogic, IValidator<PatientModel> patientValidator)
        {
            this._patientLogic = patientLogic;
            this._patientValidator = patientValidator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(FluentValidation.Results.ValidationResult), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Save([FromBody]PatientModel patientModel)
        {
            var validationResult = this._patientValidator.Validate(patientModel);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.GetFieldLevelErrors());
            }

            _patientLogic.Save(patientModel);

            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FluentValidation.Results.ValidationResult), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public IActionResult Update([FromBody]PatientModel patientModel)
        {
            var validationResult = this._patientValidator.Validate(patientModel);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.GetFieldLevelErrors());
            }

            _patientLogic.Save(patientModel);

            return Ok();
        }

        [HttpGet("id")]
        public object GetNewPatientId(int? centreId) => new { patientId = _patientLogic.GetNewPatientId(centreId) };


        [HttpGet]
        public PagedResult<PatientModel> GetPatients(int pageNumber, int pageSize, int? centreId) =>
            _patientLogic.GetAllPatients(centreId, pageNumber, pageSize);

        [HttpGet("cities")]
        public List<string> GetCities(string q) => _patientLogic.GetCities(q);

        [HttpGet("{patientId}")]
        public PatientModel GetPatient(int patientId)
        {
            if (patientId == 0) return null;

            return _patientLogic.GetPatient(patientId);
        }

        [HttpGet("Search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public PagedResult<PatientModel> SearchPatients(string q, int pageNumber, int pageSize, int? centreId)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return null;
            }

            var a = _patientLogic.SearchPatients(q, centreId, pageNumber, pageSize);

            return a;
        }

        [HttpDelete("{patientId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult DeletePatient(int patientId)
        {
            if (patientId == 0)
            {
                return BadRequest();
            }

            _patientLogic.DeletePatient(patientId);
            return Ok();
        }
    }
}