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

namespace ICQS_Management.Pages.Admin_View
{
    public class IndexModel : PageModel
    {
        private readonly IBaseRepository<User> _baseRepository;
        private readonly IAuthRepository authRepository;

        //Paging
        private const int PageSize = 6;
        public int PageNumber { get; set; }
        public int PageCount => (int)Math.Ceiling((double)TotalRecords / PageSize);
        public int TotalRecords { get; set; }

        public IndexModel(IBaseRepository<User> baseRepository, IAuthRepository authRepository)
        {

            _baseRepository = baseRepository;
            this.authRepository = authRepository;
        }
        public IList<User> User { get; set; }

        public async Task<IActionResult> OnGetAsync(int? pageNumber)
        {
            if (HttpContext.Session != null)
            {
                string userRole = HttpContext.Session.GetString("userRole");
                if (userRole == null || userRole != "admin")
                {
                    return RedirectToPage("/Authentication/ErrorSession");
                }
                else
                {
                    PageNumber = pageNumber ?? 1;
                    TotalRecords = _baseRepository.GetTotalCount();
                    User = await authRepository.GetAllPaging(PageNumber, PageSize);
                }
            }
            else
            {
                return RedirectToPage("/Authentication/ErrorSession");
            }
            return Page();
        }
    }
}
