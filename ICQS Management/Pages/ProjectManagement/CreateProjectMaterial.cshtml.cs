using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Repository.Interface;
using Repository;

namespace ICQS_Management.Pages.ProjectManagement
{
    public class CreateProjectMaterialModel : PageModel
    {
        private IMaterialManagementRepository _mMRepository = new MaterialManagementRepository();
        private IProjectManagementRepository _projectManagementRepository = new ProjectManagementRepository();


        [BindProperty]
        public ProjectMaterial ProjectMaterial { get; set; } = default!;
        public IActionResult OnGet(Guid ProjectId)
        {
            //ProjectMaterial.ProjectId = ProjectId;
            var ProjId = new ProjectMaterial
            {
                ProjectId = ProjectId,
            };
            ProjectMaterial = ProjId;
            ViewData["MaterialId"] = new SelectList(_mMRepository.GetAllMaterials(), "MaterialId", "Name");
            return Page();
        }




        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            _projectManagementRepository.AddProjectMaterial(ProjectMaterial);
            return RedirectToPage("./Index");
        }
    }
}
