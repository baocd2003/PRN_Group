using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObject.Entity
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProjectID { get; set; } 
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public float AreaPerFloor { get; set; }
        public int NumOfFloors { get; set; }
        public int NumOfLabors { get; set; }
        public float LaborSalaryPerMonth { get; set; }
        public int MonthDuration { get; set; }
        public float TotalPrice { get; set; }
        public byte Status { get; set; }
        public virtual ICollection<ProjectMaterial> ProjectMaterials { get; set;}

    }
}
