using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Entity
{
    public class Customer : User
    {

        public Guid CustomerId { get; set; }

    }
}
