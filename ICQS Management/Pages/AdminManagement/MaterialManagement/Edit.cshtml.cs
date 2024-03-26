using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Repository.Interface;
using Repository;
using BusinessObject.DTO;
using BusinessObject.Entity;

namespace ICQS_Management.Pages.AdminManagement.MaterialManagement
{
    public class EditModel : PageModel
    {
        private IMaterialManagementRepository _materialRepository;
        public IMaterialTypeManagementRepository _materialTypeRepository;
        public EditModel(IMaterialManagementRepository materialRepository, IMaterialTypeManagementRepository materialTypeRepository)
        {
            _materialRepository = materialRepository;
            _materialTypeRepository = materialTypeRepository;
        }
        [BindProperty]
        public MaterialDTO MaterialDTO { get; set; } = default!;
        [BindProperty]
        public string message { get; set; } = string.Empty;
        public async Task<IActionResult> OnGetAsync(Guid id)
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
                    if (id == null)
                    {
                        return NotFound();
                    }

                    Material material = _materialRepository.GetMaterialById(id);
                    MaterialType materialType = _materialTypeRepository.GetMaterialTypeById(material.MaterialTypeId);
                    MaterialDTO = new MaterialDTO
                    {
                        MaterialId = material.MaterialId,
                        MaterialTypeId = material.MaterialTypeId,
                        Name = material.Name,
                        MediumPrice = material.MediumPrice,
                        MaterialTypeName = materialType.MaterialTypeName,
                        UnitType = materialType.UnitType
                    };
                    ViewData["MaterialTypeId"] = new SelectList(_materialTypeRepository.GetAllMaterialTypes(), "MaterialTypeId", "MaterialTypeName");
                    if (MaterialDTO == null)
                    {
                        return NotFound();
                    }
                    return Page();
                }
            }
        }

        public IActionResult OnPostAsync()
        {
            if (MaterialDTO != null)
            {
                Material updatedMaterial = new Material
                {
                    MaterialId = MaterialDTO.MaterialId,
                    Name = MaterialDTO.Name,
                    MediumPrice = MaterialDTO.MediumPrice,
                    MaterialTypeId = MaterialDTO.MaterialTypeId,
                };
                ViewData["MaterialTypeId"] = new SelectList(_materialTypeRepository.GetAllMaterialTypes(), "MaterialTypeId", "MaterialTypeName");
                if (_materialRepository.checkUpdatedMaterialExist(updatedMaterial))
                {
                    ModelState.AddModelError("MaterialDTO.Name", "Material Name already exists!");
                    return Page();
                }
                _materialRepository.UpdateMaterial(updatedMaterial);
                message = "Update successfully.";
                return Page();
            }
            return NotFound();
        }
    }
}
