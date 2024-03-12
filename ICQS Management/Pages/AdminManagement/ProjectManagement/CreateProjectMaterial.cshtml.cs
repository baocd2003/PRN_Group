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
using BusinessObject.DTO;

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
        public byte projectStatus { get; set; }
        [BindProperty]
        public float totalprice { get; set; }
        [BindProperty]
        public List<ProjectMaterialDTO> ProjectMaterialList { get; set; }
        public IActionResult OnGet(Guid ProjectId)
        {
            projectName = _projectManagementRepository.GetProjectById(ProjectId).ProjectName;
            projectId = ProjectId;
            projectStatus = _projectManagementRepository.GetProjectById(ProjectId).Status;
            var projectMaterials = _projectManagementRepository.GetProjectMaterialByProjectId(ProjectId);

            var materials = _mMRepository.GetAllMaterials();
            ProjectMaterialList = (from pm in projectMaterials
                                   join m in materials on pm.MaterialId equals m.MaterialId
                                   where pm.ProjectId == ProjectId
                                   select new ProjectMaterialDTO
                                   {
                                       ProjectMaterialId = pm.ProjectMaterialId,
                                       ProjectId = pm.ProjectId,
                                       MaterialId = pm.MaterialId,
                                       MaterialName = m.Name,
                                       Quantity = pm.Quantity,
                                       MediumPrice = m.MediumPrice,
                                       UnitType = m.UnitType,
                                       TotalPrice = _projectManagementRepository.GetProjectById(pm.ProjectId).TotalPrice
                                   }).ToList();
            totalprice = _projectManagementRepository.GetProjectById(ProjectId).TotalPrice;

            var availableMaterials = _mMRepository.GetAllMaterials()
                .Where(material => !projectMaterials.Any(pm => pm.MaterialId == material.MaterialId));
            ViewData["MaterialId"] = new SelectList(availableMaterials, "MaterialId", "Name");
            return Page();
        }
        public IActionResult OnPostAsync()
        {
            ProjectMaterial.ProjectId = projectId;
            _projectManagementRepository.AddProjectMaterial(ProjectMaterial);
            return RedirectToPage("./CreateProjectMaterial", new { ProjectId = ProjectMaterial.ProjectId });
        }

        public IActionResult OnPostEditAsync(Guid id, int editedQuantity)
        {
            ProjectMaterial projectMaterial = _projectManagementRepository.GetProjectMaterialByProjectMaterialId(id);
            projectMaterial.Quantity = editedQuantity;
            _projectManagementRepository.UpdateProjectMaterial(projectMaterial);
            return RedirectToPage("./CreateProjectMaterial", new { ProjectId = projectId });
        }
        public IActionResult OnPostDeleteAsync(Guid id)
        {
            _projectManagementRepository.DeleteProjectMaterial(id);
            return RedirectToPage("./CreateProjectMaterial", new { ProjectId = projectId });
        }
    }
}
