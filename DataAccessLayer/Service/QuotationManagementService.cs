using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using NuGet.Packaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Service
{
    public class QuotationManagementService
    {
        private static QuotationManagementService instance;
        public static readonly object instanceLock = new object();
        private readonly applicationDbContext _db = new applicationDbContext();
        public QuotationManagementService() { }
        public static QuotationManagementService Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance is null)
                    {
                        instance = new QuotationManagementService();
                    }
                    return instance;
                }
            }
        }

        public Quotation AddQuotation (Quotation quotation) {
            _db.Quotations.Add(quotation);
            _db.SaveChanges();
            return quotation;
        }

        public IEnumerable<Quotation> GetAllQuotations()
        {
            return _db.Quotations.ToList();
        }

        public Quotation GetQuotation(Guid id)
        {
            return _db.Quotations.FirstOrDefault(q => q.QuotationId == id);
        }
        public Customer GetCustomerByEmail(string email)
        {
            return _db.Customers.FirstOrDefault(c => c.Email ==  email);
        }
    }
}
