using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Repository;

namespace ICQS_Management.Pages.Account_Staff
{
    public class RequestQuotationModel : PageModel
    {
        private readonly DataAccessLayer.ApplicationDbContext.applicationDbContext _context;
        private BatchManagementRepository _repo = new BatchManagementRepository();
        public RequestQuotationModel(DataAccessLayer.ApplicationDbContext.applicationDbContext context)
        {
            _context = context;
        }

        public IList<Quotation> Quotation { get;set; }

        public async Task OnGetAsync()
        {
            Quotation = _repo.GetRequestQuotation().ToList();
        }
    }
}
