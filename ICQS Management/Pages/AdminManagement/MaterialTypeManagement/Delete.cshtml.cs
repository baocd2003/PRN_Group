﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;

namespace ICQS_Management.Pages.AdminManagement.MaterialTypeManagement
{
    public class DeleteModel : PageModel
    {
        private readonly DataAccessLayer.ApplicationDbContext.applicationDbContext _context;

        public DeleteModel(DataAccessLayer.ApplicationDbContext.applicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MaterialType MaterialType { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MaterialType = await _context.MaterialTypes.FirstOrDefaultAsync(m => m.MaterialTypeId == id);

            if (MaterialType == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MaterialType = await _context.MaterialTypes.FindAsync(id);

            if (MaterialType != null)
            {
                _context.MaterialTypes.Remove(MaterialType);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}