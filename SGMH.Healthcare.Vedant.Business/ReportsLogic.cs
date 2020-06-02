using System.Collections.Generic;
using System.Linq;
using SGMH.Healthcare.Vedant.Business.Domain;
using SGMH.Healthcare.Vedant.Business.Interfaces;
using SGMH.Healthcare.Vedant.Data;

namespace SGMH.Healthcare.Vedant.Business
{
    public class ReportsLogic : IReportsLogic
    {
        private readonly PatientsContext _patientsContext;

        public ReportsLogic(PatientsContext patientsContext)
        {
            _patientsContext = patientsContext;
        }

        public List<ReportsModel> GetPatientsByCentre()
        {
            var patients = (from p in _patientsContext.Patients
                            join c in _patientsContext.Centres
                            on p.CentreId equals c.CentreId
                            group p by new { c.City } into groupPatients
                            select new ReportsModel
                            {
                                Field = groupPatients.Key.City,
                                Value = groupPatients.Count()
                            });

            return patients.OrderByDescending(x => x.Value).ToList();
        }

        public List<ReportsModel> GetPatientsByDate()
        {
            var patients = (from p in _patientsContext.Patients
                            group p by p.RegisteredDate.Value.Year into g
                            orderby g.Key
                            select new
                            {
                                Field = g.Key,
                                Value = g.Count()
                            }).ToList();

            return patients.Select(p => new ReportsModel
            {
                Field = p.Field.ToString(),
                Value = p.Value
            }).ToList();
        }

        public List<ReportsModel> GetConsultationsByDate()
        {
            var patients = (from c in _patientsContext.Consultation
                            group c by c.ConsultationDate.Year into g
                            orderby g.Key
                            select new
                            {
                                Field = g.Key,
                                Value = g.Count()
                            }).ToList();

            return patients.Select(p => new ReportsModel
            {
                Field = p.Field.ToString(),
                Value = p.Value
            }).ToList();
        }
    }
}
