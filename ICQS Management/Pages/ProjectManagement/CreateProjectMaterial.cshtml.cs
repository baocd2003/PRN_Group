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
        [BindProperty]
        public string projectName { get; set; }
        [BindProperty]
        public bool IsAddMoreMaterial { get; set; }
        public IActionResult OnGet(Guid ProjectId)
        {
            projectName = _projectManagementRepository.GetProjectById(ProjectId).ProjectName;
            var ProjId = new ProjectMaterial
            {
                ProjectId = ProjectId,
            };
            ProjectMaterial = ProjId;
            ViewData["MaterialId"] = new SelectList(_mMRepository.GetAllMaterials(), "MaterialId", "Name");
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPostAsync()
        {
            _projectManagementRepository.AddProjectMaterial(ProjectMaterial);
            if(IsAddMoreMaterial)
            {
                return RedirectToPage("./CreateProjectMaterial", new { ProjectId = ProjectMaterial.ProjectId });
            }
            return RedirectToPage("./Index");
        }
    }
}
