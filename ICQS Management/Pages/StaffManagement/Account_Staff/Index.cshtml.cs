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
    public class IndexModel : PageModel
    {
        private readonly IBaseRepository<Staff> _baseRepository;
        public IndexModel(IBaseRepository<Staff> baseRepository)
        {
            _baseRepository = baseRepository;
        }
        [BindProperty]

        public Staff Staff { get;set; } = default!;

        public async Task OnGetAsync(Guid id)
        {
            Staff = _baseRepository.GetById(id);
        }
    }
}
