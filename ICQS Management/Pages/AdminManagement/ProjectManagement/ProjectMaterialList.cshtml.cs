﻿using System;
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

namespace ICQS_Management.Pages.ProjectManagement
{
    public class ProjectMaterialListModel : PageModel
    {
        private IProjectManagementRepository _pmRepository;
        private IMaterialManagementRepository _materialRepository;
        private IMaterialTypeManagementRepository _materialTypeRepository;
        public ProjectMaterialListModel(IProjectManagementRepository pmRepository, IMaterialManagementRepository materialRepository, IMaterialTypeManagementRepository materialTypeRepository)
        {
            _pmRepository = pmRepository;
            _materialRepository = materialRepository;
            _materialTypeRepository = materialTypeRepository;
        }
        [BindProperty]
        public List<ProjectMaterialDTO> ProjectMaterialList { get;set; }
        [BindProperty]
        public byte Status { get; set; }
        [BindProperty]
        public Guid ProjectId { get; set; }
        [BindProperty]
        public String ProjectName { get; set; }
        [BindProperty]
        public int countProjectMaterial { get; set; }
        public async Task<IActionResult> OnGet(Guid id, byte status)
        {
            if (HttpContext.Session == null)
            {
                return RedirectToPage("/Authentication/ErrorSession");
            }
            else
            {
                string userRole = HttpContext.Session.GetString("userRole");
                if (string.IsNullOrEmpty(userRole) || (userRole != "admin"))
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
                                               Quantity = pm.Quantity,
                                               MediumPrice = m.MediumPrice,
                                               UnitType = _materialTypeRepository.GetMaterialTypeById(m.MaterialTypeId).UnitType,
                                               TotalPrice = _pmRepository.GetProjectById(pm.ProjectId).TotalPrice
                                           }).ToList();
                    countProjectMaterial = ProjectMaterialList.Count;
                    return Page();
                }
            }
        }
    }
}
