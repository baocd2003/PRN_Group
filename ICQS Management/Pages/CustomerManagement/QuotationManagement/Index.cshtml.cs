using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Repository.Interface;
using Repository;

namespace ICQS_Management.Pages.QuotationManagement
{
    public class IndexModel : PageModel
    {
        private readonly DataAccessLayer.ApplicationDbContext.applicationDbContext _context;
        private IQuotationManagementRepository _quoteRepo = new QuotationManagementRepository();
        public IndexModel(DataAccessLayer.ApplicationDbContext.applicationDbContext context)
        {
            _context = context;
        }

        public IList<Quotation> Quotation { get;set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session == null)
            {
                return RedirectToPage("/Authentication/ErrorSession");
            }
            else
            {
                string userRole = HttpContext.Session.GetString("userRole");
                if (string.IsNullOrEmpty(userRole) || (userRole != "Customer"))
                {
                    return RedirectToPage("/Authentication/ErrorSession");
                }
                else
                {
                      Quotation =_quoteRepo.GetProcessingQuotes();
                    return Page();
                }
            }

        }
    }
}
