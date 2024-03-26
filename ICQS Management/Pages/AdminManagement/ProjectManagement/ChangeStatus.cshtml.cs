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

namespace ICQS_Management.Pages.ProjectManagement
{
    public class ChangeStatusModel : PageModel
    {
        private IProjectManagementRepository _pmRepository;
        public ChangeStatusModel(IProjectManagementRepository pmRepository)
        {
            _pmRepository = pmRepository;
        }
        [BindProperty]
        public Project Project { get; set; } = default!;
        [BindProperty]
        public byte status { get; set; }
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
                    Project = await Task.Run(() => _pmRepository.GetProjectById(id));
                    status = Project.Status;
                    return Page();
                }
            }
        }

        public IActionResult OnPostAsync(Guid id)
        {
            if(_pmRepository.GetProjectById(id).Status == 1)
            {
                _pmRepository.ChangeProjectStatus(_pmRepository.GetProjectById(id));
                return RedirectToPage("./Index", new { Message = "Disable Successfully" });
            }
            else
            {
                _pmRepository.ChangeProjectStatus(_pmRepository.GetProjectById(id));
                return RedirectToPage("./Index", new { Message = "Enable Successfully" });
            }
        }
    }
}
