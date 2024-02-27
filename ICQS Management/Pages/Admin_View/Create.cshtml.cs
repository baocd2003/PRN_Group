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
            return Page();
        }

        [BindProperty]
        public User User { get; set; }
        [BindProperty]
        public string role { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
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
                    Id = User.UserId,
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
                    Id = User.UserId,
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
