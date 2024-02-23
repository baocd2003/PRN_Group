using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;

namespace ICQS_Management.Pages.BatchDetailsManagement
{
    public class EditModel : PageModel
    {
        private readonly DataAccessLayer.ApplicationDbContext.applicationDbContext _context;

        public EditModel(DataAccessLayer.ApplicationDbContext.applicationDbContext context)
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

            var batchdetail =  await _context.BatchDetails.FirstOrDefaultAsync(m => m.BatchDetailId == id);
            if (batchdetail == null)
            {
                return NotFound();
            }
            BatchDetail = batchdetail;
           ViewData["BatchId"] = new SelectList(_context.Batches, "BatchId", "BatchId");
           ViewData["MaterialId"] = new SelectList(_context.Materials, "MaterialId", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(BatchDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BatchDetailExists(BatchDetail.BatchDetailId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool BatchDetailExists(Guid id)
        {
          return (_context.BatchDetails?.Any(e => e.BatchDetailId == id)).GetValueOrDefault();
        }
    }
}
