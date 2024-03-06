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
using Repository;

namespace ICQS_Management.Pages.BatchDetailsManagement
{
    public class EditModel : PageModel
    {
        private readonly DataAccessLayer.ApplicationDbContext.applicationDbContext _context;
        private BatchManagementRepository _repo = new BatchManagementRepository();

        public EditModel(DataAccessLayer.ApplicationDbContext.applicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BatchDetail BatchDetail { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {

            var batchdetail =  await _context.BatchDetails.FirstOrDefaultAsync(m => m.BatchDetailId == id);
            if (batchdetail == null)
            {
                return NotFound();
            }
            BatchDetail = batchdetail;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            _repo.UpdateBatchDetail(BatchDetail);
            return RedirectToPage("/BatchsManagement/Index");
        }
    }
}
