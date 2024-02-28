using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Entity
{
    public class Staff : User
    {
        public Guid StaffId { get; set; }
        public ICollection<Quotation> Quotations { get; set; }

    }
}
