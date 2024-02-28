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

namespace ICQS_Management.Pages.ProjectManagement
{
    public class ProjectMaterialDetailModel : PageModel
    {
        private IProjectManagementRepository _pmRepository = new ProjectManagementRepository();
        private IMaterialManagementRepository _materialRepository = new MaterialManagementRepository();
        [BindProperty]
        public ProjectMaterialCombined ProjectMaterialList { get; set; }
        [BindProperty]
        public Guid ProjectId { get; set; }
        [BindProperty]
        public string message { get; set; } = string.Empty;
        public void OnGet(Guid id)
        {
            ProjectId = id;
            var projectMaterial = _pmRepository.GetProjectMaterialByProjectMaterialId(id);
            var materials = _materialRepository.GetAllMaterials();
            ViewData["MaterialName"] = new SelectList(_materialRepository.GetAllMaterials(), "Name", "Name");
            if (projectMaterial != null)
            {
                ProjectMaterialList = new ProjectMaterialCombined
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
                ProjectMaterialList = null;
            }
        }
        public class ProjectMaterialCombined
        {
            public Guid ProjectMaterialId { get; set; }
            public Guid ProjectId { get; set; }
            public Guid MaterialId { get; set; }
            public string MaterialName { get; set; }
            public int Quantity { get; set; }
        }
    }
}
