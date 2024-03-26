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

namespace ICQS_Management.Pages.QuotationManagement
{
    public class DeleteModel : PageModel
    {
        private readonly IQuotationManagementRepository _quotationManagement;
        private readonly IBatchManagement _batchManagement;

        public DeleteModel(IQuotationManagementRepository quotationManagement, IBatchManagement batchManagement)
        {
            _quotationManagement = quotationManagement;
            _batchManagement = batchManagement;
        }

        [BindProperty]
        public Quotation Quotation { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
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
                    if (id == null)
                    {
                        return NotFound();
                    }

                    Quotation = _quotationManagement.FindQuotationById(id.Value);

                    if (Quotation == null)
                    {
                        return NotFound();
                    }
                    return Page();
                }
            }
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var NotePase = Quotation.Note;
            Quotation = _quotationManagement.FindQuotationById(id.Value);
            Quotation.Note = NotePase;

            if (Quotation != null)
            {
                _quotationManagement.UpdateNote(Quotation);
                _batchManagement.DeleteQuotation(id.Value);
            }

            return RedirectToPage("./Index");
        }
    }
}

