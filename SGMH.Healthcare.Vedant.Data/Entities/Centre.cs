using System.Collections.Generic;

namespace SGMH.Healthcare.Vedant.Data.Entities
{
    public partial class Centre
    {
        public Centre()
        {
            //this.DrugOrders = new List<DrugOrder>();
            this.Patients = new List<Patient>();
        }

        public int CentreId { get; set; }
        public string CentreName { get; set; }
        public bool IsActive { get; set; }
        public string CentreCode { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Logo { get; set; }
        public string ContactNumber { get; set; }
        public string ZipCode { get; set; }
        public string DoctorName { get; set; }
        //public virtual ICollection<DrugOrder> DrugOrders { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
    }
}
