using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObject.Entity
{
    public class BatchDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BatchDetailId { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public Guid BatchId { get; set; }
        public Guid MaterialId { get; set; }
        public virtual Material Materials { get; set; }
        public virtual Batch Batch { get; set; }
    }
}
