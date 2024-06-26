﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Repository;
using Repository.Interface;

namespace ICQS_Management.Pages.BatchDetailsManagement
{
    public class EditModel : PageModel
    {
        private IBatchManagement _repo;

        public EditModel(IBatchManagement context)
        {
            _repo = context;
        }

        [BindProperty]
        public BatchDetail BatchDetail { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (HttpContext.Session == null)
            {
                return RedirectToPage("/Authentication/ErrorSession");
            }
            else
            {
                string userRole = HttpContext.Session.GetString("userRole");
                if (string.IsNullOrEmpty(userRole) || (userRole != "admin" && userRole != "Staff"))
                {
                    return RedirectToPage("/Authentication/ErrorSession");
                }
                else
                {
                    var batchdetail = _repo.GetDetailById((Guid)id);
                    if (batchdetail == null)
                    {
                        return NotFound();
                    }
                    BatchDetail = batchdetail;
                    return Page();
                }
            }
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            _repo.UpdateBatchDetail(BatchDetail);
            return RedirectToPage("/AdminManagement/BatchsManagement/Index");
        }
    }
}
