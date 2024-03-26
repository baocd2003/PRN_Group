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
using Repository.Interface;

namespace ICQS_Management.Pages.StaffManagement.Account_Staff
{
    public class QuotationRequestModel : PageModel
    {
        
        private IBatchManagement _batchManagementRepository = new BatchManagementRepository();
        public QuotationRequestModel(IBatchManagement batchManagementRepository)
        {
            _batchManagementRepository = batchManagementRepository;
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
                if (string.IsNullOrEmpty(userRole) || (userRole != "Staff"))
                {
                    return RedirectToPage("/Authentication/ErrorSession");
                }
                else
                {
                    Quotation = _batchManagementRepository.GetRequestQuotation().ToList();
                    return Page();
                }
            }
        }
    }
}
