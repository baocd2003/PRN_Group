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
        [Required]
        public string ProjectName { get; set; } = default!;
        [BindProperty]
        [Required]
        public string Description { get; set; } = default!;
        [BindProperty]
        [Required(ErrorMessage = "Area per floor field is required!")]
        public int AreaPerFloor { get; set; } = default!;
        [BindProperty]
        [Required(ErrorMessage = "Number of floors field is required!")]
        public int NumOfFloors { get; set; } = default!;
        private Project NewProject { get; set; } = new Project();

        ///////////////////////////////////////////////////////////////
        // Project Material Properties
        public class NewProjectMaterial
        {
            public Guid MaterialId { get; set; } = default!;
            public int Quantity { get; set; } = default!;

        }
        [BindProperty]
        [Required]
        public IList<NewProjectMaterial> NewProjectMaterials { get; set; } = default!;
        private IList<ProjectMaterial> CreatedProjectMaterials { get; set; } = new List<ProjectMaterial>();


        //////////////////////////////////////////////////////////////////////
        // Quotation properties
        [BindProperty]
        public DateTime RequestDate { get; set; } = default!;
        [BindProperty]
        public float EstimatePrice { get; set; } = default!;
        private Quotation CreatedQuotation { get; set; } = new Quotation();




        [BindProperty]
        public Project Project { get; set; } = default!;
        [BindProperty]
        public IList<ProjectMaterial> ProjectMaterials { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            NewProject.ProjectName = ProjectName;
            NewProject.Description = Description;
            NewProject.AreaPerFloor = AreaPerFloor;
            NewProject.NumOfFloors = NumOfFloors;
            NewProject.Status = 1;

            var projectResult = _context.Projects.Add(NewProject);
            if (projectResult == null)
            {
                ModelState.AddModelError(string.Empty, "something wrong when create project");
                return Page();
            }

            foreach (var projectMaterial in NewProjectMaterials)
            {
                CreatedProjectMaterials.Add(new ProjectMaterial()
                {
                    MaterialId = projectMaterial.MaterialId,
                    ProjectId = projectResult.Entity.ProjectID,
                    Quantity = projectMaterial.Quantity
                });
            }
            foreach (var createdPrjMaterial in CreatedProjectMaterials)
            {
                _context.ProjectMaterials.Add(createdPrjMaterial);
            }

            CreatedQuotation.ProjectId = projectResult.Entity.ProjectID;
            CreatedQuotation.RequestDate = RequestDate;
            CreatedQuotation.EstimatePrice = EstimatePrice;
            CreatedQuotation.Status = 0;

            string loggedEmail = HttpContext.Session.GetString("LoggedEmail");
            Customer cus = _context.Customers.FirstOrDefault(c  => c.Email == loggedEmail);
            if (cus.Quotations == null)
            {
                cus.Quotations = new List<Quotation>();
            }
            cus.Quotations.Add(CreatedQuotation);

            _context.Quotations.Add(CreatedQuotation);


            await _context.SaveChangesAsync();


            return RedirectToPage("./Index");
        }
    }
}
