﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using BusinessObject.DTO;
using Repository.Interface;
using Repository;

namespace ICQS_Management.Pages.CustomerManagement.ProjectManagement
{
    public class ProjectMaterialListModel : PageModel
    {
        private IProjectManagementRepository _pmRepository;
        private IMaterialManagementRepository _materialRepository;
        public ProjectMaterialListModel(IProjectManagementRepository pmRepository, IMaterialManagementRepository materialRepository)
        {
            _pmRepository = pmRepository;
            _materialRepository = materialRepository;
        }
        [BindProperty]
        public List<ProjectMaterialDTO> ProjectMaterialList { get; set; }
        [BindProperty]
        public byte Status { get; set; }
        [BindProperty]
        public Guid ProjectId { get; set; }
        [BindProperty]
        public String ProjectName { get; set; }
        public async Task<IActionResult> OnGet(Guid id, byte status)
        {
            if (HttpContext.Session == null)
            {
                return RedirectToPage("/Authentication/ErrorSession");
            }
            else
            {
                string userRole = HttpContext.Session.GetString("userRole");
                if (string.IsNullOrEmpty(userRole) || (userRole != "Customer"))
                {
                    return RedirectToPage("/Authentication/ErrorSession");
                }
                else
                {
                    Status = status;
                    ProjectId = id;
                    ProjectName = _pmRepository.GetProjectById(id).ProjectName;
                    var projectMaterials = _pmRepository.GetProjectMaterialByProjectId(id);
                    var materials = _materialRepository.GetAllMaterials();
                    ProjectMaterialList = (from pm in projectMaterials
                                           join m in materials on pm.MaterialId equals m.MaterialId
                                           where pm.ProjectId == id
                                           select new ProjectMaterialDTO
                                           {
                                               ProjectMaterialId = pm.ProjectMaterialId,
                                               ProjectId = pm.ProjectId,
                                               MaterialId = pm.MaterialId,
                                               MaterialName = m.Name,
                                               Quantity = pm.Quantity
                                           }).ToList();
                    return Page();
                }
            }
        }
    }
}
