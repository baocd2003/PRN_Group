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
        private IProjectManagementRepository _pmRepository = new ProjectManagementRepository();
        private readonly IBaseRepository<Project> _projectRepository = new BaseRepository<Project>();
        //Paging
        private const int PageSize = 1;
        public int PageNumber { get; set; }
        public int PageCount => (int)Math.Ceiling((double)TotalRecords / PageSize);
        public int TotalRecords { get; set; }

        public List<Project> Project { get;set; } = default!;

        public async Task OnGetAsync(int? pageNumber)
        {
            PageNumber = pageNumber ?? 1;
            TotalRecords = _projectRepository.GetTotalCount();
            Project = _projectRepository.GetAll(PageNumber, PageSize).ToList();           
        }
    }
}
