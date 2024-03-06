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
    public class IndexModel : PageModel
    {
        private readonly DataAccessLayer.ApplicationDbContext.applicationDbContext _context;

        public IndexModel(DataAccessLayer.ApplicationDbContext.applicationDbContext context)
        {
            _context = context;
        }

        public IList<Batch> Batch { get;set; } = default!;

        public async Task OnGetAsync()
        {

            if (_context.Batches != null)
            {
                Batch = await _context.Batches.ToListAsync();
            }
        }
    }
}
