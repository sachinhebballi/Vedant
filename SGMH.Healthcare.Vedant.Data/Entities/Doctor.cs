using System.Collections.Generic;

namespace SGMH.Healthcare.Vedant.Data.Entities
{
    public partial class Doctor
    {
        public Doctor()
        {
            this.Consultations = new List<Consultation>();
        }

        public int DoctorId { get; set; }
        public string Name { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public virtual ICollection<Consultation> Consultations { get; set; }
    }
}
