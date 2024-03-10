using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class BatchDetailDTO
    {
        public Guid BatchDetailId { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public Guid BatchId { get; set; }
        public string MaterialName { get; set; }
    }
}
