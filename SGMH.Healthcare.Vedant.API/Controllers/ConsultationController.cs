using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGMH.Healthcare.Vedant.Business.Domain;
using SGMH.Healthcare.Vedant.Business.Interfaces;
using System;

namespace SGMH.Healthcare.Vedant.API.Controllers
{
    [Authorize]
    [Route("api/Consultations")]
    public class ConsultationController : ControllerBase
    {
        private readonly IConsultationLogic _consultationLogic;
        private readonly ICentreLogic _centreLogic;
        private readonly IPatientLogic _patientLogic;

        public ConsultationController(IConsultationLogic consultationLogic, IPatientLogic patientLogic, ICentreLogic centreLogic)
        {
            this._consultationLogic = consultationLogic;
            this._centreLogic = centreLogic;
            this._patientLogic = patientLogic;
        }

        [HttpGet("{consultationId}/Print")]
        public PrintModel Print(int consultationId)
        {
            var consultationModel = _consultationLogic.GetConsultation(consultationId);
            var patient = _patientLogic.GetPatient(consultationModel.PatientId);
            var centreModel = _centreLogic.GetCentre(Convert.ToInt32(patient.RegisteredCentreId));

            return new PrintModel { ConsultationModel = consultationModel, CentreModel = centreModel };
        }

        [HttpPost]
        public void Save([FromBody]ConsultationModel consultationModel) => _consultationLogic.Save(consultationModel);

        [HttpPut]
        public void Update([FromBody]ConsultationModel consultationModel) => _consultationLogic.Save(consultationModel);

        [HttpGet("patient/{patientId}")]
        public ConsultationResultModel GetAllConsultations(int patientId) => _consultationLogic.GetAllConsultations(patientId);

        [HttpGet("consultationId")]
        public ConsultationModel GetConsultation(int consultationId) => _consultationLogic.GetConsultation(consultationId);

        [HttpGet("Latest/{patientId}")]
        public int GetLatestConsultation(int patientId) => _consultationLogic.GetLatestConsultation(patientId);

        [HttpDelete("{consultationId}")]
        public void DeleteConsultation(int consultationId) => _consultationLogic.DeleteConsultation(consultationId);
    }
}