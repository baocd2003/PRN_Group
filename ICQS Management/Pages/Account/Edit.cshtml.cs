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

namespace ICQS_Management.Pages.Account
{
    public class EditModel : PageModel
    {
        private readonly IBaseRepository<Customer> _baseRepository;
        public EditModel(IBaseRepository<Customer> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = _baseRepository.GetById(id);
            if (customer == null)
            {
                return NotFound();
            }
            Customer = customer;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _baseRepository.Update(Customer,Customer.CustomerId);

            try
            {
                _baseRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception();
            }

            return RedirectToPage("./Details", new {id = Customer.CustomerId});
        }

        
    }
}
