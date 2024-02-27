using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ICQS_Management.Pages.QuotationManagement
{
    public class CreateModel : PageModel
    {
        private readonly DataAccessLayer.ApplicationDbContext.applicationDbContext _context;

        public CreateModel(DataAccessLayer.ApplicationDbContext.applicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Project = await _context.Projects.FirstOrDefaultAsync(m => m.ProjectID == id);
            if (Project == null)
            {
                return NotFound();
            }
            ProjectMaterials = await _context.ProjectMaterials.Include(p => p.Materials).Where(p => p.ProjectId == id).ToListAsync();

            //ViewData["CustomerId"] = new SelectList(_context.Customers, "UserId", "Discriminator");
            //ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectID", "ProjectName");
            //ViewData["StaffID"] = new SelectList(_context.Staffs, "UserId", "Discriminator");
            return Page();
        }

        // Project properties
        [BindProperty]
        public DateTime RequestDate { get; set; } = default!;
        [BindProperty]
        public float EstimatePrice { get; set; } = default!;
        [BindProperty]
        public Project Project { get; set; } = default!;
        [BindProperty]
        public IList<ProjectMaterial> ProjectMaterials { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid || Project == null || ProjectMaterials == null)
            //{
            //    return Page();
            //}
            
            _context.Projects.Add(Project);
            await _context.SaveChangesAsync();

            foreach (var projectMaterial in ProjectMaterials)
            {
                projectMaterial.ProjectId = Project.ProjectID;
                _context.ProjectMaterials.Add(projectMaterial);
            }
            await _context.SaveChangesAsync();


            return RedirectToPage("./Index");
        }
    }
}
