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
    public class DetailsModel : PageModel
    {
        private readonly IBaseRepository<User> _baseRepository;

        public DetailsModel(IBaseRepository<User> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public User User { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
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
                    if (id == null)
                    {
                        return NotFound();
                    }

                    User = _baseRepository.GetById(id);

                    if (User == null)
                    {
                        return NotFound();
                    }
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
