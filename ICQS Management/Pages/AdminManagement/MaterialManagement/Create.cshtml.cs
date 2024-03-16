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

namespace ICQS_Management.Pages.AdminManagement.MaterialManagement
{
    public class CreateModel : PageModel
    {
        private IMaterialManagementRepository _materialRepository = new MaterialManagementRepository();
        [BindProperty]
        public Material Material { get; set; } = default!;
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
                    return Page();
                }
            }
        }

        public IActionResult OnPostAsync()
        {
            if (Material != null)
            {
                if (_materialRepository.checkMaterialExist(Material))
                {
                    ModelState.AddModelError("Material.Name", "Material Name already exists!");
                    return Page();
                }
                _materialRepository.AddMaterial(Material);
                message = "Add successfully.";
                return Page();
            }
            return Page();
        }
    }
}
