using System.Collections.Generic;
using SGMH.Healthcare.Vedant.Business.Domain;

namespace SGMH.Healthcare.Vedant.Business.Interfaces
{
    public interface IReportsLogic
    {
        List<ReportsModel> GetPatientsByCentre();

        List<ReportsModel> GetPatientsByDate();

        List<ReportsModel> GetConsultationsByDate();
    }
}
