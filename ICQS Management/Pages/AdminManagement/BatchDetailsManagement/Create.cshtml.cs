using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Repository;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Repository.Interface;
using BusinessObject.DTO;

namespace ICQS_Management.Pages.BatchDetailsManagement
{
    public class CreateModel : PageModel
    {
        private readonly DataAccessLayer.ApplicationDbContext.applicationDbContext _context;
        private BatchManagementRepository _repo = new BatchManagementRepository();
        private IMaterialManagementRepository _materialRepo = new MaterialManagementRepository();
        public CreateModel(DataAccessLayer.ApplicationDbContext.applicationDbContext context)
        {
            _context = context;
        }

        public IList<BatchDetailDTO> BatchDetails { get; set; } = default!;

        public IActionResult OnGet()
        {
            ImportDate = DateTime.Parse(HttpContext.Session.GetString("ImportDate"));
            string detailListJson = HttpContext.Session.GetString("detailList");
            List<BatchDetail> batchDetails = JsonConvert.DeserializeObject<List<BatchDetail>>(detailListJson);

            var materials = _materialRepo.GetAllMaterials();
            Materials = _materialRepo.GetAllMaterials().ToList();
            ViewData["MaterialId"] = new SelectList(_materialRepo.GetOthersMaterial(batchDetails), "MaterialId", "Name");
            
            BatchDetails = (from bd in batchDetails 
                            join m in materials on bd.MaterialId equals m.MaterialId
                            select new BatchDetailDTO
                            {
                                BatchDetailId = bd.BatchDetailId,
                                Quantity = bd.Quantity,
                                Price = bd.Price,
                                MaterialName = m.Name,
                                Unit = m.UnitType
                            }).ToList();
            return Page();
        }

        [BindProperty]
        public BatchDetail BatchDetail { get; set; } = default!;
        public List<Material> Materials { get; set; }
        public DateTime ImportDate { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (BatchDetail == null)
            {
                return Page();
            }
            ImportDate = DateTime.Parse(HttpContext.Session.GetString("ImportDate"));
            string detailListJson = HttpContext.Session.GetString("detailList");
            List<BatchDetail> batchDetails = JsonConvert.DeserializeObject<List<BatchDetail>>(detailListJson);
            batchDetails.Add(BatchDetail);
            var continueCheckbox = Request.Form["continueCheckbox"];
            if (!string.IsNullOrEmpty(continueCheckbox))
            {
                ViewData["MaterialId"] = new SelectList(_materialRepo.GetOthersMaterial(batchDetails), "MaterialId", "Name");
                string detailList = JsonConvert.SerializeObject(batchDetails);
                HttpContext.Session.SetString("detailList", detailList);
                if (!_materialRepo.GetOthersMaterial(batchDetails).Any())
                {
                    HttpContext.Session.SetString("detailList", detailList);
                    //HttpContext.Session.Remove("ImportDate");
                    return RedirectToPage("./CheckOutBatch");
                }
                var materials = _materialRepo.GetAllMaterials();
                BatchDetails = (from bd in batchDetails
                                join m in materials on bd.MaterialId equals m.MaterialId
                                select new BatchDetailDTO
                                {
                                    BatchDetailId = bd.BatchDetailId,
                                    Quantity = bd.Quantity,
                                    Price = bd.Price,
                                    MaterialName = m.Name
                                }).ToList();
                return Page();
            }else
            {
                //_repo.CreateBatch(ImportDate, batchDetails);
                string detailList = JsonConvert.SerializeObject(batchDetails);
                HttpContext.Session.SetString("detailList", detailList);
                //HttpContext.Session.Remove("ImportDate");
                return RedirectToPage("./CheckOutBatch");
            }
           
        }
    }
}
