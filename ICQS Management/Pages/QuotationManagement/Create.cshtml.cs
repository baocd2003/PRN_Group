using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;

namespace ICQS_Management.Pages.QuotationManagement
{
    public class CreateModel : PageModel
    {
        private readonly DataAccessLayer.ApplicationDbContext.applicationDbContext _context;

        public CreateModel(DataAccessLayer.ApplicationDbContext.applicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CustomerId"] = new SelectList(_context.Customers, "UserId", "Discriminator");
        ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectID", "Description");
        ViewData["StaffID"] = new SelectList(_context.Staffs, "UserId", "Discriminator");
            return Page();
        }

        [BindProperty]
        public Quotation Quotation { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Quotations == null || Quotation == null)
            {
                return Page();
            }

            _context.Quotations.Add(Quotation);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
