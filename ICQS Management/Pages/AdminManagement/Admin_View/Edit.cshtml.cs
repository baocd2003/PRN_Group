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

namespace ICQS_Management.Pages.Admin_View
{
    public class EditModel : PageModel
    {
        private readonly IBaseRepository<User> _baseRepository = new BaseRepository<User>();



        [BindProperty]
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            try
            {
                _baseRepository.Update(User, User.UserId);
                _baseRepository.Save();
                return Page();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

           
        }


    }
}
