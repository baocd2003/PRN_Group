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
using System.Text.RegularExpressions;

namespace ICQS_Management.Pages.Authentication
{
    public class CustomerRegisterModel : PageModel
    {
        private IAuthRepository _authRepository;
        private readonly IQuotationManagementRepository _quotationManagementRepository;

        public CustomerRegisterModel(IAuthRepository authRepository, IQuotationManagementRepository quotationManagementRepository)
        {
            _authRepository = authRepository;
            _quotationManagementRepository = quotationManagementRepository;
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {

            var exitedUser = _quotationManagementRepository.GetCustomerByEmail(Customer.Email);
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
            else if (!Regex.IsMatch(Customer.Email, @"^\S+@gmail\.com$"))
            {
                TempData["RegisterError"] = "Email format is invalid!";
                return Page();
            }
            else if (exitedUser is not null)
            {
                TempData["RegisterError"] = "Email existed!";
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
