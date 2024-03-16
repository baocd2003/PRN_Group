using System;
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
using BusinessObject.Entity;

namespace ICQS_Management.Pages.AdminManagement.MaterialManagement
{
    public class DetailsModel : PageModel
    {
        private IMaterialManagementRepository _materialRepository = new MaterialManagementRepository();
        public IMaterialTypeManagementRepository _materialTypeRepository = new MaterialTypeManagementRepository();
        [BindProperty]
        public MaterialDTO MaterialDTO { get; set; } = default!;

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

                    if (MaterialDTO == null)
                    {
                        return NotFound();
                    }
                    return Page();
                }
            }
        }
    }
}
