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
    public class IndexModel : PageModel
    {
        private IProjectManagementRepository _pmRepository;
        private readonly IBaseRepository<Project> _projectRepository;
        public IndexModel(IProjectManagementRepository pmRepository, IBaseRepository<Project> projectRepository)
        {
            _pmRepository = pmRepository;
            _projectRepository = projectRepository;
        }
        private const int PageSize = 5;
        public int PageNumber { get; set; }
        public int PageCount => (int)Math.Ceiling((double)TotalRecords / PageSize);
        public int TotalRecords { get; set; }
        public string message { get; set; } = string.Empty;
        public List<Project> Project { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? pageNumber, string? Message)
        {
            if (Message != null)
            {
                message = Message;
            }
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
                    PageNumber = pageNumber ?? 1;
                    TotalRecords = _projectRepository.GetTotalCount();
                    Project = await _pmRepository.GetAllPaging(PageNumber, PageSize);
                    return Page();
                }
            }
        }
    }
}
