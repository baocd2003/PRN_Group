using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Repository.Interface;
using Repository;
using BusinessObject.DTO;
using BussinessObject.Entity;

namespace ICQS_Management.Pages.AdminManagement.MaterialTypeManagement
{
    public class CreateModel : PageModel
    {
        private IMaterialTypeManagementRepository _materialTypeRepository = new MaterialTypeManagementRepository();

        [BindProperty]
        public string message { get; set; } = string.Empty;
        [BindProperty]
        public MaterialType MaterialType { get; set; }
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
                    return Page();
                }
            }
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (MaterialType != null)
            {
                if (_materialTypeRepository.checkMaterialTypeExist(MaterialType))
                {
                    ModelState.AddModelError("MaterialType.MaterialTypeName", "Material Type name already exists!");
                    return Page();
                }
                _materialTypeRepository.AddMaterialType(MaterialType);
                message = "Add successfully.";
                return Page();
            }
            return NotFound();
        }
    }
}
