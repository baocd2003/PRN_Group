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
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ICQS_Management.Pages.ProjectManagement
{
    public class CreateProjectMaterialModel : PageModel
    {
        private IMaterialManagementRepository _mMRepository = new MaterialManagementRepository();
        private IProjectManagementRepository _projectManagementRepository = new ProjectManagementRepository();
        private IMaterialTypeManagementRepository _materialTypeRepository = new MaterialTypeManagementRepository();
        public CreateProjectMaterialModel(IProjectManagementRepository pmRepository, IMaterialManagementRepository materialRepository, IMaterialTypeManagementRepository materialTypeRepository)
        {
            _projectManagementRepository = pmRepository;
            _mMRepository = materialRepository;
            _materialTypeRepository = materialTypeRepository;
        }
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
        [BindProperty]
        public int countProjectMaterial { get; set; }
        [BindProperty]
        public Boolean isDisabled { get; set; } = false;
        [BindProperty]
        public Boolean isTypeDisabled { get; set; } = false;
        [BindProperty]
        public string UnitType { get; set; }
        [BindProperty]
        public float QuantityPerArea { get; set; }
        [BindProperty]
        public float TotalArea { get; set; }
        public IActionResult OnGet(Guid ProjectId, Guid? materialTypeId)
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
                                               MaterialTypeId = m.MaterialTypeId,
                                               ProjectId = pm.ProjectId,
                                               MaterialId = pm.MaterialId,
                                               MaterialName = m.Name,
                                               Quantity = pm.Quantity,
                                               MediumPrice = m.MediumPrice,
                                               UnitType = _materialTypeRepository.GetMaterialTypeById(m.MaterialTypeId).UnitType,
                                               TotalPrice = _projectManagementRepository.GetProjectById(pm.ProjectId).TotalPrice
                                           }).ToList();
                    totalprice = _projectManagementRepository.GetProjectById(ProjectId).TotalPrice;
                    countProjectMaterial = ProjectMaterialList.Count;
                    var availableMaterials = _mMRepository.GetAllMaterials()
                        .Where(material => !projectMaterials.Any(pm => pm.MaterialId == material.MaterialId));
                    if (materialTypeId != null)
                    {
                        ViewData["MaterialTypeId"] = new SelectList(_materialTypeRepository.GetAllMaterialTypes().Where(e => !ProjectMaterialList.Select(pml => pml.MaterialTypeId).Contains(e.MaterialTypeId)), "MaterialTypeId", "MaterialTypeName", materialTypeId);
                        UnitType = _materialTypeRepository.GetMaterialTypeById(materialTypeId.Value).UnitType;
                        ViewData["MaterialId"] = new SelectList(availableMaterials.Where(am => am.MaterialTypeId == materialTypeId), "MaterialId", "Name");
                        if (ViewData["MaterialId"] == null || ((SelectList)ViewData["MaterialId"]).Count() <= 0)
                        {
                            isDisabled = true;
                        }
                    }
                    else
                    {
                        var materialType = _materialTypeRepository.GetAllMaterialTypes().FirstOrDefault(e => !ProjectMaterialList.Select(pml => pml.MaterialTypeId).Contains(e.MaterialTypeId));
                        if (materialType == null)
                        {
                            isTypeDisabled = true;
                            isDisabled = true;
                            QuantityPerArea = _materialTypeRepository.GetMaterialTypeById(_materialTypeRepository.GetAllMaterialTypes().First().MaterialTypeId).QuantityPerArea;
                            TotalArea = Convert.ToInt32(_projectManagementRepository.GetProjectById(projectId).AreaPerFloor) * _projectManagementRepository.GetProjectById(projectId).NumOfFloors;
                            return Page();
                        }
                        ViewData["MaterialTypeId"] = new SelectList(_materialTypeRepository.GetAllMaterialTypes().Where(e => !ProjectMaterialList.Select(pml => pml.MaterialTypeId).Contains(e.MaterialTypeId)), "MaterialTypeId", "MaterialTypeName", _materialTypeRepository.GetAllMaterialTypes().Where(e => !ProjectMaterialList.Select(pml => pml.MaterialTypeId).Contains(e.MaterialTypeId)).First().MaterialTypeId);
                        UnitType = _materialTypeRepository.GetMaterialTypeById(_materialTypeRepository.GetAllMaterialTypes().Where(e => !ProjectMaterialList.Select(pml => pml.MaterialTypeId).Contains(e.MaterialTypeId)).First().MaterialTypeId).UnitType;
                        ViewData["MaterialId"] = new SelectList(availableMaterials.Where(am => am.MaterialTypeId == _materialTypeRepository.GetAllMaterialTypes().Where(e => !ProjectMaterialList.Select(pml => pml.MaterialTypeId).Contains(e.MaterialTypeId)).First().MaterialTypeId), "MaterialId", "Name");
                        if (ViewData["MaterialId"] == null || ((SelectList)ViewData["MaterialId"]).Count() <= 0)
                        {
                            isDisabled = true;
                        }
                    }
                    QuantityPerArea = _materialTypeRepository.GetMaterialTypeById(_materialTypeRepository.GetAllMaterialTypes().First().MaterialTypeId).QuantityPerArea;
                    TotalArea = Convert.ToInt32(_projectManagementRepository.GetProjectById(projectId).AreaPerFloor) * _projectManagementRepository.GetProjectById(projectId).NumOfFloors;
                    return Page();
                }
            }
        }
        public IActionResult OnPostAsync()
        {
            ProjectMaterial.ProjectId = projectId;
            int totalArea = Convert.ToInt32(_projectManagementRepository.GetProjectById(projectId).AreaPerFloor) * _projectManagementRepository.GetProjectById(projectId).NumOfFloors;
            float quantityPerArea = _materialTypeRepository.GetMaterialTypeById(_mMRepository.GetMaterialById(ProjectMaterial.MaterialId).MaterialTypeId).QuantityPerArea;
            ProjectMaterial.Quantity = (int)Math.Ceiling(totalArea * quantityPerArea);
            _projectManagementRepository.AddProjectMaterial(ProjectMaterial);
            _projectManagementRepository.UpdateProjectTotalPrice(ProjectMaterial.ProjectId);
            return RedirectToPage("./CreateProjectMaterial", new { ProjectId = ProjectMaterial.ProjectId });
        }
        public IActionResult OnPostDeleteAsync(Guid id)
        {
            _projectManagementRepository.DeleteProjectMaterial(id);
            _projectManagementRepository.UpdateProjectTotalPrice(projectId);
            return RedirectToPage("./CreateProjectMaterial", new { ProjectId = projectId });
        }
        public IActionResult OnPostChooseMaterialAsync(Guid id, Guid MaterialTypeId)
        {
            return RedirectToPage("./CreateProjectMaterial", new { ProjectId = id,  materialTypeId = MaterialTypeId });
        }
    }
}
