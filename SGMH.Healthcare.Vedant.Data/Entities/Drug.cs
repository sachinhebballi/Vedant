using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGMH.Healthcare.Vedant.Data.Entities
{
    public partial class Drug
    {
        public Drug()
        {
            this.DrugOrderDetails = new List<DrugOrderDetail>();
        }

        public int DrugId { get; set; }
        public Nullable<int> DrugCategoryId { get; set; }
        [Column("Drug")]
        public string DrugName { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string MeasureUnit { get; set; }
        public virtual DrugCategory DrugCategory { get; set; }
        public virtual ICollection<DrugOrderDetail> DrugOrderDetails { get; set; }
    }
}
