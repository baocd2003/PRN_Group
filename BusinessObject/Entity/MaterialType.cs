using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObject.Entity;

namespace BusinessObject.Entity
{
    public class MaterialType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MaterialTypeId { get; set; }
        public string MaterialTypeName { get; set; }
        public string UnitType { get; set; }
        public float QuantityPerArea { get; set; }
        public virtual ICollection<Material> Materials { get; set; }
    }
}
