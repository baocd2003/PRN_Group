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

namespace ICQS_Management.Pages.ProjectManagement
{
    public class EditProjectModel : PageModel
    {
        private IProjectManagementRepository _pmRepository;
        private readonly IBaseRepository<Project> _projectRepository;
        public EditProjectModel(IProjectManagementRepository pmRepository, IBaseRepository<Project> projectRepository)
        {
            _pmRepository = pmRepository;
            _projectRepository = projectRepository;
        }
        [BindProperty]
        public Project Project { get; set; }
        [BindProperty]
        public string message { get; set; } = string.Empty;
        public float materialPrice { get; set; }

        public IActionResult OnGet(Guid id)
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

                    Project = _pmRepository.GetProjectById(id);
                    materialPrice = _pmRepository.CalculateProjectMaterialPrice(id);
                    if (Project == null)
                    {
                        return NotFound();
                    }
                    return Page();
                }
            }
        }
        public IActionResult OnPost()
        {
            if (Project != null)
            {
                if (_pmRepository.checkUpdatedProjectExist(Project))
                {
                    ModelState.AddModelError("Project.ProjectName", "Project Name already exists!");
                    return Page();
                }
                Project.Status = 1;
                _projectRepository.Update(Project, Project.ProjectID);
                _projectRepository.Save();
                message = "Update successfully.";
                return Page();
            }
            return Page();
        }
    }
}
