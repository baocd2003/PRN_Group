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
using Newtonsoft.Json;
using Repository.Interface;
using Repository;
using BusinessObject.Entity;

namespace ICQS_Management.Pages.BatchDetailsManagement
{
    public class EditCheckoutModel : PageModel
    {
        private IMaterialManagementRepository _materialRepo = new MaterialManagementRepository();
        private IMaterialTypeManagementRepository _typeRepo = new MaterialTypeManagementRepository();

        public EditCheckoutModel(IMaterialManagementRepository materialRepo, IMaterialTypeManagementRepository typeRepo)
        {
            _materialRepo = materialRepo;
            _typeRepo = typeRepo;
        }

        [BindProperty]
        public BatchDetail BatchDetail { get; set; }

        [BindProperty]
        private int DetailIndex { get; set; }

        

        [BindProperty]
        public MaterialType MaterialType { get; set; } = default!;
        [BindProperty]
        public float MediumPrice { get; set; } = 0;
        public Material Material { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int index)
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
                    if (index == null)
                    {
                        return NotFound();
                    }
                    string detailListJson = HttpContext.Session.GetString("detailList");
                    List<BatchDetail> batchDetails = JsonConvert.DeserializeObject<List<BatchDetail>>(detailListJson);
                    BatchDetail = batchDetails[index];
                    BatchDetail.MaterialId = batchDetails[index].MaterialId;
                    DetailIndex = index;
                    TempData["IndexValue"] = index;
                    Material = _materialRepo.GetMaterialById(batchDetails[index].MaterialId);
                    MaterialType = _typeRepo.GetMaterialTypeById(Material.MaterialTypeId);
                    MediumPrice = Material.MediumPrice;
                    return Page();
                }
            }
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            string detailListJson = HttpContext.Session.GetString("detailList");
            List<BatchDetail> batchDetails = JsonConvert.DeserializeObject<List<BatchDetail>>(detailListJson);
            int index = 0;
            if (TempData.ContainsKey("IndexValue"))
            {
               index = (int)TempData["IndexValue"];
            }
            batchDetails[index].Quantity = BatchDetail.Quantity;
            batchDetails[index].Price = BatchDetail.Price;
            string detailList = JsonConvert.SerializeObject(batchDetails);
            HttpContext.Session.SetString("detailList", detailList);
            return RedirectToPage("./Create");
        }
    }
}
