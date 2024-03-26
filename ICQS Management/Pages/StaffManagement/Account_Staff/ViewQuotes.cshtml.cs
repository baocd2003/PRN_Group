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

namespace ICQS_Management.Pages.StaffManagement.Account_Staff
{
    public class ViewQuotesModel : PageModel
    {
        private IQuotationManagementRepository _repo;

        public ViewQuotesModel(IQuotationManagementRepository context)
        {
            _repo = context;
        }

        public IList<Quotation> Quotation { get;set; }

        public async Task OnGetAsync()
        {
            Quotation = _repo.GetAllQuotations();
        }
    }
}
