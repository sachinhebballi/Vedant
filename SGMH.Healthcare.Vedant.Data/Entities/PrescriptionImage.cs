using System.ComponentModel.DataAnnotations.Schema;

namespace SGMH.Healthcare.Vedant.Data.Entities
{
    public partial class PrescriptionImage
    {
        public int PrescriptionImageId { get; set; }
        public int ConsultationId { get; set; }
        [Column("PrescriptionImage")]
        public string PrescriptionImageName { get; set; }
        public virtual Consultation Consultation { get; set; }
    }
}
