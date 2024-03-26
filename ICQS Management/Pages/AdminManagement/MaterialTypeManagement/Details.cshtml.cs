using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Repository.Interface;
using Repository;

namespace ICQS_Management.Pages.AdminManagement.MaterialTypeManagement
{
    public class DetailsModel : PageModel
    {
        private IMaterialTypeManagementRepository _materialTypeRepository = new MaterialTypeManagementRepository();
        public DetailsModel(IMaterialTypeManagementRepository materialTypeRepository)
        {
            _materialTypeRepository = materialTypeRepository;
        }
        public MaterialType MaterialType { get; set; }

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
    }
}
