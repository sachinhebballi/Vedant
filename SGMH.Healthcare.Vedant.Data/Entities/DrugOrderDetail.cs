using System.ComponentModel.DataAnnotations;

namespace SGMH.Healthcare.Vedant.Data.Entities
{
    public partial class DrugOrderDetail
    {
        [Key]
        public int OrderDetailsId { get; set; }
        public int OrderId { get; set; }
        public int DrugId { get; set; }
        public short? Quantity { get; set; }
        public string MeasureUnit { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public virtual Drug Drug { get; set; }
        public virtual DrugOrder DrugOrder { get; set; }
    }
}
