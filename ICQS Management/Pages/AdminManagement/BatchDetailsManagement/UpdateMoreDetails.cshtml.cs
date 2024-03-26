﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Newtonsoft.Json;
using Repository;
using Repository.Interface;
using BusinessObject.Entity;

namespace ICQS_Management.Pages.BatchDetailsManagement
{
    public class UpdateMoreDetailsModel : PageModel
    {
        private readonly DataAccessLayer.ApplicationDbContext.applicationDbContext _context;
        private BatchManagementRepository _repo = new BatchManagementRepository();
        private IMaterialManagementRepository _materialRepo = new MaterialManagementRepository();

        public UpdateMoreDetailsModel(DataAccessLayer.ApplicationDbContext.applicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Guid BatchId { get; set; }
        [BindProperty]
        public MaterialType MaterialType { get; set; } = default!;

        [BindProperty]
        public float MediumPrice { get; set; } = 0;

        public IActionResult OnGet()
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
                    var id = HttpContext.Session.GetString("selectedBatchId");
                    List<BatchDetail> list = _repo.GetBatchDetailsByBatchId(Guid.Parse(id));
                    //List<BatchDetail> list = _context.BatchDetails.Where(bd => bd.BatchId == id).ToList();
                    ViewData["MaterialId"] = new SelectList(_materialRepo.GetOthersMaterial(list), "MaterialId", "Name");
                    return Page();
                }
            }
        }

        [BindProperty]
        public BatchDetail BatchDetail { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var id = HttpContext.Session.GetString("selectedBatchId");
            Guid selectedBatchId = Guid.Parse(id);
            if (BatchDetail == null)
            {
                return Page();
            }
            BatchDetail.BatchId = selectedBatchId;
            string detailListJson = HttpContext.Session.GetString("moreDetailList");
            List<BatchDetail> batchDetails = JsonConvert.DeserializeObject<List<BatchDetail>>(detailListJson);
            batchDetails.Add(BatchDetail);

            var continueCheckbox = Request.Form["continueCheckbox"];
            if (!string.IsNullOrEmpty(continueCheckbox))
            {
                List<BatchDetail> list = _repo.GetBatchDetailsByBatchId(selectedBatchId);
                ViewData["MaterialId"] = new SelectList(_materialRepo.GetOthersMaterial(list), "MaterialId", "Name");
                string detailList = JsonConvert.SerializeObject(batchDetails);
                HttpContext.Session.SetString("moreDetailList", detailList);
                return Page();
            }
            else
            {
                _repo.AddMoreDetailsInBatch(batchDetails);
                HttpContext.Session.Remove("moreDetailList");
                return RedirectToPage("/AdminManagement/BatchsManagement/Details?id=" + selectedBatchId);
            }
        }
    }
}
