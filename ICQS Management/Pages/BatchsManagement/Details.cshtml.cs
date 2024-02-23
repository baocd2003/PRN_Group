using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;

namespace ICQS_Management.Pages.BatchsManagement
{
    public class DetailsModel : PageModel
    {
        private readonly DataAccessLayer.ApplicationDbContext.applicationDbContext _context;

        public DetailsModel(DataAccessLayer.ApplicationDbContext.applicationDbContext context)
        {
            _context = context;
        }

      public Batch Batch { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Batches == null)
            {
                return NotFound();
            }

            var batch = await _context.Batches.FirstOrDefaultAsync(m => m.BatchId == id);
            if (batch == null)
            {
                return NotFound();
            }
            else 
            {
                Batch = batch;
            }
            return Page();
        }
    }
}
