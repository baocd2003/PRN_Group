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
        private IProjectManagementRepository _pmRepository = new ProjectManagementRepository();

        [BindProperty]
        public Project Project { get; set; } = default!;
        [BindProperty]
        public byte status { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Project = await Task.Run(() => _pmRepository.GetProjectById(id));
            status = Project.Status;
            return Page();
        }

        public IActionResult OnPostAsync(Guid id)
        {
            _pmRepository.ChangeProjectStatus(_pmRepository.GetProjectById(id));
            return RedirectToPage("./Index");
        }
    }
}
