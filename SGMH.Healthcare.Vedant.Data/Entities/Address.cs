using System.Collections.Generic;

namespace SGMH.Healthcare.Vedant.Data.Entities
{
    public sealed partial class Address
    {
        public Address()
        {
            this.Patients = new List<Patient>();
        }

        public int AddressId { get; set; }
        public string City { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public ICollection<Patient> Patients { get; set; }
    }
}
