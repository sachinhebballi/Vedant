using System;
using System.Collections.Generic;

namespace SGMH.Healthcare.Vedant.Data.Entities
{
    public partial class Consultation
    {
        public Consultation()
        {
            this.Prescriptions = new List<Prescription>();
            this.PrescriptionImages = new List<PrescriptionImage>();
        }

        public int ConsultationId { get; set; }
        public int PatientId { get; set; }
        public string Description { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ConsultationDate { get; set; }
        public string OthersPrescription { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; }
        public virtual ICollection<PrescriptionImage> PrescriptionImages { get; set; }
    }
}
