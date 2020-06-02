using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGMH.Healthcare.Vedant.Data.Entities
{
    public partial class Patient
    {
        public Patient()
        {
            this.Consultations = new List<Consultation>();
        }

        public int PatientId { get; set; }
        public int AddressId { get; set; }
        public Nullable<int> ImageId { get; set; }
        public string PatientName { get; set; }
        public string Gender { get; set; }
        public Nullable<Byte> Age { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public Nullable<System.DateTime> RegisteredDate { get; set; }
        [Column("RegisteredCentreId")]
        public int CentreId { get; set; }
        public string RegistrationNumber { get; set; }
        public bool IsActive { get; set; }
        public virtual Address Address { get; set; }
        public virtual Centre Centre { get; set; }
        public virtual ICollection<Consultation> Consultations { get; set; }
    }
}
