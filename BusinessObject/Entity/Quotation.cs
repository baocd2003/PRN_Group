
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
namespace BussinessObject.Entity
{
    public class Quotation
    {
        public Quotation()
        {
            this.Batchs = new HashSet<Batch>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid QuotationId { get; set; }
        public Guid ProjectId { get; set; }
        //public int TotalArea { get; set; }
        public DateTime RequestDate { get; set; }
        public double EstimatePrice { get; set; }
        public double CompletePrice { get; set; }

        public int Status { get; set; }
        public virtual Project Project { get; set; } = null!;
        public virtual ICollection<Batch> Batchs { get; set; }
    }
}