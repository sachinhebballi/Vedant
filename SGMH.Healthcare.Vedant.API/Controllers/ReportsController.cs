using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SGMH.Healthcare.Vedant.Business.Domain;
using SGMH.Healthcare.Vedant.Business.Interfaces;

namespace SGMH.Healthcare.Vedant.API.Controllers
{
    [Authorize]
    [Route("api/reports/")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsLogic _reportsLogic;

        public ReportsController(IReportsLogic reportsLogic)
        {
            this._reportsLogic = reportsLogic;
        }

        [HttpGet("patients/centre")]
        public List<ReportsModel> GetPatientsByCentre()
            => _reportsLogic.GetPatientsByCentre();

        [HttpGet("patients/date")]
        public List<ReportsModel> GetPatientsByDate()
            => _reportsLogic.GetPatientsByDate();

        [HttpGet("consultations/date")]
        public List<ReportsModel> GetConsultationsByDate()
            => _reportsLogic.GetConsultationsByDate();
    }
}
