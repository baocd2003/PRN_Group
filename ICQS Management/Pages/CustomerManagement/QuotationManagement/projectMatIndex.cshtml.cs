using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;

namespace ICQS_Management.Pages.QuotationManagement
{
    public class projectMatIndexModel : PageModel
    {
        private readonly DataAccessLayer.ApplicationDbContext.applicationDbContext _context;

        public projectMatIndexModel(DataAccessLayer.ApplicationDbContext.applicationDbContext context)
        {
            _context = context;
        }

        public IList<ProjectMaterial> ProjectMaterial { get;set; }

        public async Task OnGetAsync()
        {
            ProjectMaterial = await _context.ProjectMaterials
                .Include(p => p.Materials)
                .Include(p => p.Projects).ToListAsync();
        }
    }
}
