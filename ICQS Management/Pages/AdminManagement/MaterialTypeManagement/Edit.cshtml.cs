using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Repository.Interface;
using Repository;

namespace ICQS_Management.Pages.AdminManagement.MaterialTypeManagement
{
    public class EditModel : PageModel
    {
        private IMaterialTypeManagementRepository _materialTypeRepository = new MaterialTypeManagementRepository();
        public EditModel(IMaterialTypeManagementRepository materialTypeRepository)
        {
            _materialTypeRepository = materialTypeRepository;
        }
        [BindProperty]
        public MaterialType MaterialType { get; set; }
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

                    MaterialType = _materialTypeRepository.GetMaterialTypeById(id);

                    if (MaterialType == null)
                    {
                        return NotFound();
                    }
                    return Page();
                }
            }
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (MaterialType != null)
            {
                if (_materialTypeRepository.checkUpdatedMaterialTypeExist(MaterialType))
                {
                    ModelState.AddModelError("MaterialType.MaterialTypeName", "Material Type name already exists!");
                    return Page();
                }
                _materialTypeRepository.UpdateMaterialType(MaterialType);
                message = "Update successfully.";
                return Page();
            }
            return NotFound();
        }
    }
}
