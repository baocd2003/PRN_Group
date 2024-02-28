﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;

namespace ICQS_Management.Pages.QuotationManagement
{
    public class DetailsModel : PageModel
    {
        private readonly DataAccessLayer.ApplicationDbContext.applicationDbContext _context;

        public DetailsModel(DataAccessLayer.ApplicationDbContext.applicationDbContext context)
        {
            _context = context;
        }

        public Quotation Quotation { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Quotation = await _context.Quotations
                .Include(q => q.Project).FirstOrDefaultAsync(m => m.QuotationId == id);

            if (Quotation == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
