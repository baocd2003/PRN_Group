﻿using System;
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

namespace ICQS_Management.Pages.CustomerManagement.QuotationManagement
{
    public class ViewProcessingQuotesModel : PageModel
    {
        private readonly DataAccessLayer.ApplicationDbContext.applicationDbContext _context;
        private IQuotationManagementRepository _quoteRepo = new QuotationManagementRepository();
        public ViewProcessingQuotesModel(DataAccessLayer.ApplicationDbContext.applicationDbContext context)
        {
            _context = context;
        }

        public IList<Quotation> Quotation { get;set; }

        public async Task OnGetAsync()
        {
            Quotation = _quoteRepo.GetAppliedQuotes();
        }
    }
}
