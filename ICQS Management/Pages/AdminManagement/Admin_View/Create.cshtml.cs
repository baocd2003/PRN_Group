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
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;

namespace ICQS_Management.Pages.Admin_View
{
    public class CreateModel : PageModel
    {
        private readonly IBaseRepository<User> _baseRepository;
        private readonly IQuotationManagementRepository _quotationManagement;

        public CreateModel(IBaseRepository<User> baseRepository, IQuotationManagementRepository quotationManagement)
        {
            _baseRepository = baseRepository;
            _quotationManagement = quotationManagement;
        }

        public IActionResult OnGet(Guid userId)
        {
            if (HttpContext.Session != null)
            {
                string userRole = HttpContext.Session.GetString("userRole");
                if (userRole == null || userRole != "admin")
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
            var exitedUser = _quotationManagement.GetCustomerByEmail(User.Email);
            if (User.Email is null || User.Name is null || User.PhoneNumber is null || User.Password is null)
            {
                TempData["createError"] = "Field can not be null";
                return RedirectToPage();
            }
            if (!Regex.IsMatch(User.Email, @"^\S+@gmail\.com$"))
            {
                TempData["createError"] = "Email is invalid";
                return RedirectToPage();
            }
            else if (exitedUser is not null)
            {
                TempData["createError"] = "Email can not be duplicated";
                return RedirectToPage();
            }
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
                    status = "1",
                    PhoneNumber = User.PhoneNumber,
                };

                _baseRepository.InsertUser(staffDTO);
               
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
                    status = "1",
                    PhoneNumber = User.PhoneNumber,
                };
                _baseRepository.InsertUser(customerDTO);
                
            }
           
            return RedirectToPage("./Index");
        }
    }
}
