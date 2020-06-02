using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGMH.Healthcare.Vedant.Data.Entities
{
    public partial class DrugCategory
    {
        public DrugCategory()
        {
            this.Drugs = new List<Drug>();
        }

        public int DrugCategoryId { get; set; }
        [Column("DrugCategory")]
        public string DrugCategoryName { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public virtual ICollection<Drug> Drugs { get; set; }
    }
}
