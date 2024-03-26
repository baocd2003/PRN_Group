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
        private readonly IBaseRepository<User> _baseRepository;  
        private readonly IQuotationManagementRepository _quotationManagement;
        public EditModel(IQuotationManagementRepository quotationManagement, IBaseRepository<User> baseRepository)
        {
            _quotationManagement = quotationManagement;
            _baseRepository = baseRepository;
        }


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
            var iduser = _baseRepository.GetById(User.UserId);
            if (User.Email is null || User.Name is null || User.PhoneNumber is null)
            {
                TempData["createError"] = "Field can not be null";
                return Page();
            }
           
            try
            {
                User.status = iduser.status;
                User.Password = iduser.Password;
                _baseRepository.Update(User);              
                var user = _baseRepository.GetById(User.UserId);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

           
        }


    }
}
