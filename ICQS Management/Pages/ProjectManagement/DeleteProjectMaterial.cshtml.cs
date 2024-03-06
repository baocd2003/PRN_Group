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
using BusinessObject.DTO;
using Microsoft.CodeAnalysis;

namespace ICQS_Management.Pages.ProjectManagement
{
    public class DeleteProjectMaterialModel : PageModel
    {
        private IProjectManagementRepository _projectManagementRepository = new ProjectManagementRepository();
        private IMaterialManagementRepository _materialRepository = new MaterialManagementRepository();
        [BindProperty]
        public ProjectMaterialDTO ProjectMaterial { get; set; }
        [BindProperty]
        public Guid ProjectMaterialId { get; set; }
        [BindProperty]
        public Guid ProjectId { get; set; }
        [BindProperty]
        public byte status { get; set; }
        public void OnGetAsync(Guid id)
        {
            ProjectMaterialId = id;
            var projectMaterial = _projectManagementRepository.GetProjectMaterialByProjectMaterialId(id);
            var materials = _materialRepository.GetAllMaterials();
            ProjectId = projectMaterial.ProjectId;
            status = _projectManagementRepository.GetProjectById(ProjectId).Status;
            if (projectMaterial != null)
            {
                ProjectMaterial = new ProjectMaterialDTO
                {
                    ProjectMaterialId = projectMaterial.ProjectMaterialId,
                    ProjectId = projectMaterial.ProjectId,
                    MaterialId = projectMaterial.MaterialId,
                    MaterialName = materials.FirstOrDefault(m => m.MaterialId == projectMaterial.MaterialId)?.Name,
                    Quantity = projectMaterial.Quantity
                };
            }
            else
            {
                NotFound();
            }
        }

        public IActionResult OnPost()
        {
            _projectManagementRepository.DeleteProjectMaterial(ProjectMaterialId);
            return RedirectToPage("./ProjectMaterialList", new { id = ProjectId, status = status });
        }
    }
}
