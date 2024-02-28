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

namespace ICQS_Management.Pages.QuotationManagement
{
    public class projectMaterialEditModel : PageModel
    {
        private readonly DataAccessLayer.ApplicationDbContext.applicationDbContext _context;

        public projectMaterialEditModel(DataAccessLayer.ApplicationDbContext.applicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ProjectMaterial ProjectMaterial { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProjectMaterial = await _context.ProjectMaterials
                .Include(p => p.Materials)
                .Include(p => p.Projects).FirstOrDefaultAsync(m => m.ProjectMaterialId == id);

            if (ProjectMaterial == null)
            {
                return NotFound();
            }
           ViewData["MaterialId"] = new SelectList(_context.Materials, "MaterialId", "Name");
           ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectID", "Description");
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

            _context.Attach(ProjectMaterial).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectMaterialExists(ProjectMaterial.ProjectMaterialId))
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

        private bool ProjectMaterialExists(Guid id)
        {
            return _context.ProjectMaterials.Any(e => e.ProjectMaterialId == id);
        }
    }
}
