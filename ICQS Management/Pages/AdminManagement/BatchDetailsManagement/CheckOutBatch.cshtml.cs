﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Repository;
using BusinessObject.DTO;
using Repository.Interface;

namespace ICQS_Management.Pages.BatchDetailsManagement
{
    public class CheckOutBatchModel : PageModel
    {
        private readonly DataAccessLayer.ApplicationDbContext.applicationDbContext _context;
        private BatchManagementRepository _repo = new BatchManagementRepository();
        private IMaterialManagementRepository _materialRepo = new MaterialManagementRepository();

        public CheckOutBatchModel(DataAccessLayer.ApplicationDbContext.applicationDbContext context)
        {
            _context = context;
        }

        public IList<BatchDetailDTO> BatchDetail { get;set; }

        public DateTime ImportDate { get; set; }


        public async Task OnGetAsync()
        {
            string detailListJson = HttpContext.Session.GetString("detailList");
            List<BatchDetail> batchDetails = JsonConvert.DeserializeObject<List<BatchDetail>>(detailListJson);
            var materials = _materialRepo.GetAllMaterials();
            BatchDetail = (from bd in batchDetails
                            join m in materials on bd.MaterialId equals m.MaterialId
                            select new BatchDetailDTO
                            {
                                BatchDetailId = bd.BatchDetailId,
                                Quantity = bd.Quantity,
                                Price = bd.Price,
                                MaterialName = m.Name
                            }).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string detailListJson = HttpContext.Session.GetString("detailList");
            List<BatchDetail> batchDetails = JsonConvert.DeserializeObject<List<BatchDetail>>(detailListJson);
            ImportDate = DateTime.Parse(HttpContext.Session.GetString("ImportDate"));
            _repo.CreateBatch(ImportDate, batchDetails);
            return RedirectToPage("/AdminManagement/BatchsManagement/Index");

        }

        //public void OnPostCreateMoreDetails()
        //{

        //}
    }
}
