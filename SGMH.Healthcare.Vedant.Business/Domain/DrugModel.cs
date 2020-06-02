using System.ComponentModel.DataAnnotations;

namespace SGMH.Healthcare.Vedant.Business.Domain
{
    public class DrugModel
    {
        public int DrugId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        [Required]
        [MaxLength(100)]
        public string DrugName { get; set; }
        public short? Quantity { get; set; }
        [MaxLength(20)]
        public string MeasureUnit { get; set; }
    }
}