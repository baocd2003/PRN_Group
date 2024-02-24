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

namespace ICQS_Management.Pages.ProjectManagement
{
    public class CreateModel : PageModel
    {
        private IProjectManagementRepository _pmRepository = new ProjectManagementRepository();

        [BindProperty]
        public Project Project { get; set; } = default!;
        
        public IActionResult OnPost()
        {
            if (Project != null)
            {
                var project = _pmRepository.AddProject(Project);
                return RedirectToPage("./CreateProjectMaterial", new { ProjectId = project.ProjectID });
            }
            return Page();
        }
    }
}
