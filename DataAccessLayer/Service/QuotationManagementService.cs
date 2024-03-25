using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
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

        public Quotation AddQuotation(Quotation quotation)
        {
            _db.Quotations.Add(quotation);
            _db.SaveChanges();
            return quotation;
        }

        public IList<Quotation> GetAllQuotations()
        {
            return _db.Quotations.Include(q => q.Project)
                .ThenInclude(p => p.ProjectMaterials)
                .ThenInclude(pm => pm.Materials)
                .ThenInclude(m => m.MaterialTypes).ToList();
        }

        public static Staff GetResponder(Guid quotId)
        {
            using (var _dbb = new applicationDbContext())
            {
                return _dbb.Staffs.FirstOrDefault(s => s.Quotations.Any(q => q.QuotationId == quotId));
            }
        }

        public Quotation GetQuotation(Guid id)
        {
            return _db.Quotations.FirstOrDefault(q => q.QuotationId == id);
        }

        public Customer GetCustomerByEmail(string email)
        {
            return _db.Customers.FirstOrDefault(c => c.Email == email);
        }

        public List<Quotation> GetProcessingQuotes()
        {
            return _db.Quotations.Include(q => q.Project).Where(q => q.Status == 0).ToList();
        }

        public List<Quotation> GetAppliedQuotes()
        {
            return _db.Quotations.Include(q => q.Project).Where(q => q.Status == 1).ToList();
        }
        public Quotation FindQuotationById(Guid id)
        {
            using (applicationDbContext _dbnew = new applicationDbContext())
            {
                return _dbnew.Quotations.Include(x => x.Project)
                    .FirstOrDefault(x => x.QuotationId.Equals(id));
            }
        }
        public void UpdateNote(Quotation quotation)
        {
            using (applicationDbContext _dbnew = new applicationDbContext())
            {
                _dbnew.Attach(quotation).State = EntityState.Modified;
                try
                {
                    _dbnew.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EyeglassExists(quotation.QuotationId))
                    {

                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }        
        private bool EyeglassExists(Guid id)
        {
            using (applicationDbContext _dbNew = new applicationDbContext())
            {
                return (_dbNew.Quotations?.Any(e => e.QuotationId == id)).GetValueOrDefault();
            }

        }

    }
}
