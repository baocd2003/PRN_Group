using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Entity
{
    public class ProjectMaterial
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProjectMaterialId { get; set; }
        public Guid MaterialId { get; set; }
        public Guid ProjectId { get; set; }
        public virtual Project Projects { get; set; }
        public virtual Material Materials { get; set; }
        public int Quantity { get; set; }
        
    }
}
