using System;
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
        private IBatchManagement _repo;
        private IMaterialManagementRepository _materialRepo;

        public CheckOutBatchModel(IBatchManagement repo, IMaterialManagementRepository materialRepo) {
            _repo = repo;
            _materialRepo = materialRepo;
        }
        public IList<BatchDetailDTO> BatchDetail { get;set; }

        public DateTime ImportDate { get; set; }


        public async Task<IActionResult> OnGetAsync()
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
                    return Page();
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string detailListJson = HttpContext.Session.GetString("detailList");
            List<BatchDetail> batchDetails = JsonConvert.DeserializeObject<List<BatchDetail>>(detailListJson);
            ImportDate = DateTime.Parse(HttpContext.Session.GetString("ImportDate"));
            if (!batchDetails.Any())
            {
                ModelState.AddModelError("", "Add at least 1 detail in batch");
                return Page();
            }
            _repo.CreateBatch(ImportDate, batchDetails);
            return RedirectToPage("/AdminManagement/BatchsManagement/Index");

        }
    }
}
