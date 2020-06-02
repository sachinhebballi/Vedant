using SGMH.Healthcare.Vedant.Business.Domain;
using System.Collections.Generic;

namespace SGMH.Healthcare.Vedant.Business.Interfaces
{
    public interface IPatientLogic
    {
        void Save(PatientModel patientModel);

        string GetNewPatientId(int? centreId);

        PagedResult<PatientModel> GetAllPatients(int? centreId, int pageNumber, int pageSize = 10);

        PatientModel GetPatient(int PatientId);

        PagedResult<PatientModel> SearchPatients(string q, int? centreId, int pageNumber, int pageSize);

        List<string> GetCities(string q);

        void DeletePatient(int PatientId);
    }
}
