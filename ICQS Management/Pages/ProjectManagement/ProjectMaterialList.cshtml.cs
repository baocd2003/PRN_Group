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

namespace ICQS_Management.Pages.ProjectManagement
{
    public class ProjectMaterialListModel : PageModel
    {
        private IProjectManagementRepository _pmRepository = new ProjectManagementRepository();
        private IMaterialManagementRepository _materialRepository = new MaterialManagementRepository();
        public List<ProjectMaterialCombined> ProjectMaterialList { get;set; }
        [BindProperty]
        public byte Status { get; set; }
        [BindProperty]
        public Guid ProjectId { get; set; }
        public void OnGet(Guid id, byte status)
        {
            Status = status;
            ProjectId = id;
            var projectMaterials = _pmRepository.GetProjectMaterialByProjectId(id);
            var materials = _materialRepository.GetAllMaterials();
            ProjectMaterialList = (from pm in projectMaterials
                             join m in materials on pm.MaterialId equals m.MaterialId
                             where pm.ProjectId == id
                             select new ProjectMaterialCombined
                             {
                                 ProjectMaterialId = pm.ProjectMaterialId,
                                 ProjectId = pm.ProjectId,
                                 MaterialId = pm.MaterialId,
                                 MaterialName = m.Name,
                                 Quantity = pm.Quantity
                             }).ToList();
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
