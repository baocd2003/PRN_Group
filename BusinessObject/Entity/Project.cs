using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObject.Entity
{
    public class Project
    {
        public Project()
        {
            this.Customers = new HashSet<Customer>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProjectID { get; set; } 
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public float AreaPerFloor { get; set; }
        public int NumOfFloors { get; set; }
        public byte Status { get; set; }
        //public virtual User Customers { get; set; }
        public virtual ICollection<ProjectMaterial> ProjectMaterials { get; set;}

    }
}
