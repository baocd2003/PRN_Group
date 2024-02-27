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

namespace ICQS_Management.Pages.Admin_View
{
    public class EditModel : PageModel
    {
        private readonly IBaseRepository<User> _baseRepository;

        public EditModel(IBaseRepository<User> baseRepository)
        {
            _baseRepository = baseRepository;
        }
        [BindProperty]
        public User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _baseRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            User = user;
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

            _baseRepository.Update(User, User.UserId);

            try
            {
                _baseRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;

            }

            return RedirectToPage("./Index");
        }


    }
}
