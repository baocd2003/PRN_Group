using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Newtonsoft.Json;

namespace ICQS_Management.Pages.BatchDetailsManagement
{
    public class IndexModel : PageModel
    {
        private readonly DataAccessLayer.ApplicationDbContext.applicationDbContext _context;

        public IndexModel(DataAccessLayer.ApplicationDbContext.applicationDbContext context)
        {
            _context = context;
        }

        public IList<BatchDetail> BatchDetail { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.BatchDetails != null)
            {
                BatchDetail = await _context.BatchDetails
                .Include(b => b.Batch)
                .Include(b => b.Materials).ToListAsync();
            }
        }
    }
}
