using System.Collections.Generic;

namespace SGMH.Healthcare.Vedant.Business.Domain
{
    public partial class PatientsWithTotalCount
    {
        public PatientsWithTotalCount()
        {
        }

        public IEnumerable<PatientModel> Patients { get; set; }
        public int TotalPatients { get; set; }
        public int MalePatients { get; set; }
        public int FemalePatients { get; set; }
    }
}
