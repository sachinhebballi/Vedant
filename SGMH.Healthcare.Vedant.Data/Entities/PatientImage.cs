using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SGMH.Healthcare.Vedant.Data.Entities
{
    public partial class PatientImage
    {
        [Key]
        public int ImageId { get; set; }
        public byte[] Image { get; set; }
        public System.DateTime ModifiedDate { get; set; }
    }
}
