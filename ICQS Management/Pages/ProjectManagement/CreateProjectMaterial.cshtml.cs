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
using Microsoft.CodeAnalysis;
using Microsoft.Build.Evaluation;

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
        public Guid projectId { get; set; }
        [BindProperty]
        public bool IsAddMoreMaterial { get; set; }
        [BindProperty]
        public byte projectStatus { get; set; }
        public IActionResult OnGet(Guid ProjectId)
        {
            projectName = _projectManagementRepository.GetProjectById(ProjectId).ProjectName;
            projectId = ProjectId;
            projectStatus = _projectManagementRepository.GetProjectById(ProjectId).Status;
            var projectMaterials = _projectManagementRepository.GetProjectMaterialByProjectId(ProjectId);
            var availableMaterials = _mMRepository.GetAllMaterials()
                .Where(material => !projectMaterials.Any(pm => pm.MaterialId == material.MaterialId));
            ViewData["MaterialId"] = new SelectList(availableMaterials, "MaterialId", "Name");
            return Page();
        }
        public IActionResult OnPostAsync()
        {
            ProjectMaterial.ProjectId = projectId;
            _projectManagementRepository.AddProjectMaterial(ProjectMaterial);
            if(IsAddMoreMaterial)
            {
                return RedirectToPage("./CreateProjectMaterial", new { ProjectId = ProjectMaterial.ProjectId });
            }
            return RedirectToPage("./ProjectMaterialList", new { id = ProjectMaterial.ProjectId , status = _projectManagementRepository.GetProjectById(ProjectMaterial.ProjectId).Status });
        }
    }
}
