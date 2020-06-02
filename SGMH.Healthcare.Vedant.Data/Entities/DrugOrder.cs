using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SGMH.Healthcare.Vedant.Data.Entities
{
    public partial class DrugOrder
    {
        public DrugOrder()
        {
            this.DrugOrderDetails = new List<DrugOrderDetail>();
        }

        [Key]
        public int OrderId { get; set; }
        public int CentreId { get; set; }
        public string OrderNumber { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public Nullable<short> OrderQuantity { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public virtual Centre Centre { get; set; }
        public virtual ICollection<DrugOrderDetail> DrugOrderDetails { get; set; }
    }
}
