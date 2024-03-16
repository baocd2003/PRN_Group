using System;
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

namespace ICQS_Management.Pages.CustomerManagement.ProjectManagement
{
    public class ProjectListModel : PageModel
    {
        private IProjectManagementRepository _pmRepository = new ProjectManagementRepository();

        public List<Project> Project { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
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
                    Project = await Task.Run(() => _pmRepository.GetAllProjects().Where(p => p.Status == 1).ToList());
                    return Page();
                }
            }
        }
    }
}
