using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class QuoteRequestDTO
    {
        public Guid QuotationId { get; set; }
        public List<Guid> BatchId { get; set; }
    }
}
