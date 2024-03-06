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
        [BindProperty]
        public string message { get; set; } = string.Empty;

        public IActionResult OnPost()
        {
            if (Project != null)
            {
                if(_pmRepository.checkUpdatedProjectExist(Project))
                {
                    ModelState.AddModelError("Project.ProjectName", "Project Name already exists!");
                    return Page();
                }
                Project.Status = 1;
                _pmRepository.AddProject(Project);
                message = "Add successfully.";
                return RedirectToPage("./CreateProjectMaterial", new { ProjectId = Project.ProjectID });
            }
            return Page();
        }
    }
}
