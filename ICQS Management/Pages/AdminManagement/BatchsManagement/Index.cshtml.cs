using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Repository;
using Repository.Interface;

namespace ICQS_Management.Pages.BatchsManagement
{
    public class IndexModel : PageModel
    {
        private IBatchManagement _repo;

        public IndexModel(IBatchManagement repo) {
            _repo = repo;
        }
        public IList<Batch> Batch { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session == null)
            {
                return RedirectToPage("/Authentication/ErrorSession");
            }
            else
            {
                string userRole = HttpContext.Session.GetString("userRole");
                if (string.IsNullOrEmpty(userRole) || (userRole != "admin" && userRole != "Staff"))
                {
                    return RedirectToPage("/Authentication/ErrorSession");
                }
                else
                {
                    Batch = _repo.GetBatchesDateAsc().ToList();
                    return Page();
                }
            }
        }
    }
}
