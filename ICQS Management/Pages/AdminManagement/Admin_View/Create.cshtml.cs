using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using NuGet.Common;
using Repository.Interface;

namespace ICQS_Management.Pages.Admin_View
{
    public class CreateModel : PageModel
    {
        private readonly IBaseRepository<User> _baseRepository;

        public CreateModel(IBaseRepository<User> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public IActionResult OnGet(Guid userId)
        {
            if (HttpContext.Session != null)
            {
                string userRole = HttpContext.Session.GetString("userRole");
                if(userRole == null || userRole != "admin")
                {
                    return RedirectToPage("/Authentication/ErrorSession");
                }
                return Page();
            }
            else
            {
                return RedirectToPage("/Authentication/ErrorSession");
            }
        }

        [BindProperty]
        public User User { get; set; }
        [BindProperty]
        public string role { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (role == "Staff")
            {
                User.UserId = Guid.NewGuid();
                var staffDTO = new Staff
                {
                    UserId = User.UserId,
                    Email = User.Email,
                    Name = User.Name,
                    StaffId = User.UserId,
                    Password = User.Password,
                    PhoneNumber = User.PhoneNumber,
                };

                _baseRepository.Insert(staffDTO);
                _baseRepository.Save();
            }
            else if (role == "Customer")
            {
                User.UserId = Guid.NewGuid();
                var customerDTO = new Customer
                {
                    UserId = User.UserId,
                    Email = User.Email,
                    Name = User.Name,
                    CustomerId = User.UserId,
                    Password = User.Password,
                    PhoneNumber = User.PhoneNumber,
                };
                _baseRepository.Insert(customerDTO);
                _baseRepository.Save();
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }



            return RedirectToPage("./Index");
        }
    }
}
