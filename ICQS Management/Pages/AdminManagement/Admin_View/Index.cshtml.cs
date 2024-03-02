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

namespace ICQS_Management.Pages.Admin_View
{
    public class IndexModel : PageModel
    {
        private readonly IBaseRepository<User> _baseRepository;

        public IndexModel(IBaseRepository<User> baseRepository)
        {
            _baseRepository = baseRepository;
        }
        public IList<User> User { get; set; }

        public async Task OnGetAsync()
        {
            User = _baseRepository.GetAll().ToList();
        }
    }
}
