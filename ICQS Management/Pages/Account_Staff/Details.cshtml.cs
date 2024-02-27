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

namespace ICQS_Management.Pages.Account_Staff
{
    public class DetailsModel : PageModel
    {
        private readonly IBaseRepository<Staff> _baseRepository;
        public DetailsModel(IBaseRepository<Staff> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public Staff Staff { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var staff = _baseRepository.GetById(id);
            if (staff == null)
            {
                return NotFound();
            }
            else 
            {
                Staff = staff;
            }
            return Page();
        }
    }
}
