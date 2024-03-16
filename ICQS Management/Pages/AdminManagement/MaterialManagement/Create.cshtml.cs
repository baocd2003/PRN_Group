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
using BusinessObject.DTO;

namespace ICQS_Management.Pages.AdminManagement.MaterialManagement
{
    public class CreateModel : PageModel
    {
        private IMaterialManagementRepository _materialRepository = new MaterialManagementRepository();
        public IMaterialTypeManagementRepository _materialTypeRepository = new MaterialTypeManagementRepository();
        [BindProperty]
        public MaterialDTO MaterialDTO { get; set; } = default!;
        [BindProperty]
        public string message { get; set; } = string.Empty;
        public IActionResult OnGet()
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
                    ViewData["MaterialTypeId"] = new SelectList(_materialTypeRepository.GetAllMaterialTypes(), "MaterialTypeId", "MaterialTypeName");
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
                    Name = MaterialDTO.Name,
                    MediumPrice = MaterialDTO.MediumPrice,
                    MaterialTypeId = MaterialDTO.MaterialTypeId,
                };
                ViewData["MaterialTypeId"] = new SelectList(_materialTypeRepository.GetAllMaterialTypes(), "MaterialTypeId", "MaterialTypeName");
                if (_materialRepository.checkMaterialExist(updatedMaterial))
                {
                    ModelState.AddModelError("MaterialDTO.Name", "Material Name already exists!");
                    return Page();
                }
                _materialRepository.AddMaterial(updatedMaterial);
                message = "Add successfully.";
                return Page();
            }
            return NotFound();
        }
    }
}
