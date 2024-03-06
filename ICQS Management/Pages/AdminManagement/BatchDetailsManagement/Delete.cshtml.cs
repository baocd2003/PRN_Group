using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;

namespace ICQS_Management.Pages.BatchDetailsManagement
{
    public class DeleteModel : PageModel
    {
        private readonly DataAccessLayer.ApplicationDbContext.applicationDbContext _context;

        public DeleteModel(DataAccessLayer.ApplicationDbContext.applicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public BatchDetail BatchDetail { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.BatchDetails == null)
            {
                return NotFound();
            }

            var batchdetail = await _context.BatchDetails.FirstOrDefaultAsync(m => m.BatchDetailId == id);

            if (batchdetail == null)
            {
                return NotFound();
            }
            else 
            {
                BatchDetail = batchdetail;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null || _context.BatchDetails == null)
            {
                return NotFound();
            }
            var batchdetail = await _context.BatchDetails.FindAsync(id);

            if (batchdetail != null)
            {
                BatchDetail = batchdetail;
                _context.BatchDetails.Remove(BatchDetail);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
