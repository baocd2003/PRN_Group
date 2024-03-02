using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Repository.Interface;
using Repository;
using static ICQS_Management.Pages.ProjectManagement.ProjectMaterialListModel;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.DTO;

namespace ICQS_Management.Pages.ProjectManagement
{
    public class ProjectMaterialDetailModel : PageModel
    {
        private IProjectManagementRepository _pmRepository = new ProjectManagementRepository();
        private IMaterialManagementRepository _materialRepository = new MaterialManagementRepository();
        [BindProperty]
        public ProjectMaterialDTO ProjectMaterialList { get; set; }
        [BindProperty]
        public Guid ProjectId { get; set; }
        [BindProperty]
        public byte status { get; set; }
        [BindProperty]
        public string message { get; set; } = string.Empty;
        public void OnGet(Guid id, string? Message)
        {
            message = Message != null ? Message : string.Empty;
            var projectMaterial = _pmRepository.GetProjectMaterialByProjectMaterialId(id);
            var materials = _materialRepository.GetAllMaterials();
            ProjectId = projectMaterial.ProjectId;
            status = _pmRepository.GetProjectById(ProjectId).Status;
            if (projectMaterial != null)
            {
                ProjectMaterialList = new ProjectMaterialDTO
                {
                    ProjectMaterialId = projectMaterial.ProjectMaterialId,
                    ProjectId = projectMaterial.ProjectId,
                    MaterialId = projectMaterial.MaterialId,
                    MaterialName = materials.FirstOrDefault(m => m.MaterialId == projectMaterial.MaterialId)?.Name,
                    Quantity = projectMaterial.Quantity
                };
                //--Combobox
                var projectMaterials = _pmRepository.GetProjectMaterialByProjectId(ProjectId);
                var availableMaterials = _materialRepository.GetAllMaterials()
                .Where(material => !projectMaterials.Any(pm => pm.MaterialId == material.MaterialId) ||
                               material.MaterialId == projectMaterial.MaterialId);
                ViewData["MaterialId"] = new SelectList(availableMaterials, "MaterialId", "Name", ProjectMaterialList.MaterialId);
                //--
            }
            else
            {
                ProjectMaterialList = null;
            }
        }
        public IActionResult OnPost()
        {
            ProjectMaterial projectMaterial = new ProjectMaterial();
            projectMaterial.ProjectMaterialId = ProjectMaterialList.ProjectMaterialId;
            projectMaterial.MaterialId = ProjectMaterialList.MaterialId;
            projectMaterial.ProjectId = ProjectMaterialList.ProjectId;
            projectMaterial.Quantity = ProjectMaterialList.Quantity;
            _pmRepository.UpdateProjectMaterial(projectMaterial);
            return RedirectToPage("./ProjectMaterialDetail", new { id = projectMaterial.ProjectMaterialId, Message = "Update successfully." });
        }
    }
}
