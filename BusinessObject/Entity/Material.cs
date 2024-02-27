using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObject.Entity
{
    public class Material
    {
       
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MaterialId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ProjectMaterial> ProjectMaterials { get; set; }
    }
}
