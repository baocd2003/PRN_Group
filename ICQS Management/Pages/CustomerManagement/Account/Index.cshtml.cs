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
using Repository;

namespace ICQS_Management.Pages.Account
{
    public class IndexModel : PageModel
    {
        private readonly IBaseRepository<Customer> _baseRepository;
        public IndexModel(IBaseRepository<Customer> baseRepository)
        {
            _baseRepository = baseRepository;
        }
        [BindProperty]
        public Customer Customer { get; set; } = default!;

        public async Task OnGetAsync(Guid id)
        {
            var customers = _baseRepository.GetById(id);
            Customer = customers;
            
        }
    }
}
