using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Repository.Interface;
using Repository;

namespace ICQS_Management.Pages.Authentication
{
    public class CustomerRegisterModel : PageModel
    {
        private IAuthRepository _authRepository = new AuthRepository();

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {

            if (Customer is null)
            {
                TempData["RegisterError"] = "Can not add null value";
                return Page();
            }
            else if (Customer.Name is null)
            {
                TempData["RegisterError"] = "Name can not be null";
                return Page();
            }
            else if (Customer.Email is null)
            {
                TempData["RegisterError"] = "Email can not be null";
                return Page();
            }
            else if (Customer.Password is null)
            {
                TempData["RegisterError"] = "Password can not be null";
                return Page();
            }
            else if (Customer.PhoneNumber is null)
            {
                TempData["RegisterError"] = "Phone can not be null";
                return Page();
            }
            
            var customer = await _authRepository.CustomerRegister(Customer);

            return RedirectToPage("./LoginView");
        }
    }
}
