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
using BusinessObject.Entity;

namespace ICQS_Management.Pages.BatchDetailsManagement
{
    public class CreateModel : PageModel
    {
        private IBatchManagement _repo;
        private IMaterialManagementRepository _materialRepo;
        private IMaterialTypeManagementRepository _typeRepo;
        public CreateModel(IBatchManagement repo, IMaterialManagementRepository materialRepo, IMaterialTypeManagementRepository typeRepo)
        {
            _repo = repo;
            _materialRepo = materialRepo;
            _typeRepo = typeRepo;
        }

        public IList<BatchDetailDTO> BatchDetails { get; set; } = default!;
        [BindProperty]
        public MaterialType MaterialType { get; set; } = default!;

        [BindProperty]
        public float MediumPrice { get; set; } = 0;
        public IActionResult OnGet(Guid? materialId)
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
                    ImportDate = DateTime.Parse(HttpContext.Session.GetString("ImportDate"));
                    string detailListJson = HttpContext.Session.GetString("detailList");
                    List<BatchDetail> batchDetails = JsonConvert.DeserializeObject<List<BatchDetail>>(detailListJson);

                    var materials = _materialRepo.GetAllMaterials();
                    Materials = _materialRepo.GetAllMaterials().ToList();
                    ViewData["MaterialId"] = new SelectList(_materialRepo.GetOthersMaterial(batchDetails), "MaterialId", "Name");
                    IEnumerable<Material> list = _materialRepo.GetOthersMaterial(batchDetails);
                    MaterialType = _typeRepo.GetMaterialTypeById(list.FirstOrDefault().MaterialTypeId);
                    if (materialId != null)
                    {
                        ViewData["MaterialId"] = new SelectList(_materialRepo.GetOthersMaterial(batchDetails), "MaterialId", "Name",materialId);
                        Material mat = _materialRepo.GetMaterialById((Guid)materialId);
                        MaterialType = _typeRepo.GetMaterialTypeById(mat.MaterialTypeId);
                        TempData["MatId"] = (Guid)materialId;
                        MediumPrice = mat.MediumPrice;
                    }
                    else {
                        ViewData["MaterialId"] = new SelectList(_materialRepo.GetOthersMaterial(batchDetails), "MaterialId", "Name");
                        List<Material> matList = _materialRepo.GetOthersMaterial(batchDetails).ToList();
                        Material mat = matList.FirstOrDefault();
                        MaterialType = _typeRepo.GetMaterialTypeById(mat.MaterialTypeId);
                        TempData["MatId"] = mat.MaterialId;
                        MediumPrice = mat.MediumPrice;
                    }

                    BatchDetails = (from bd in batchDetails
                                    join m in materials on bd.MaterialId equals m.MaterialId
                                    select new BatchDetailDTO
                                    {
                                        BatchDetailId = bd.BatchDetailId,
                                        Quantity = bd.Quantity,
                                        Price = bd.Price,
                                        MaterialName = m.Name,
                                    }).ToList();
                    return Page();
                }
            }
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
            BatchDetail.MaterialId = (Guid)TempData["MatId"];
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
                    return RedirectToPage("./CheckOutBatch");
                }
                IEnumerable<Material> list = _materialRepo.GetOthersMaterial(batchDetails);
                MaterialType = _typeRepo.GetMaterialTypeById(list.FirstOrDefault().MaterialTypeId);
                MediumPrice = list.FirstOrDefault().MediumPrice;
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
            } else
            {
                if (!batchDetails.Any())
                {
                    ModelState.AddModelError("", "Add at least 1 detail in batch");
                    return Page();
                }
                else
                {
                    string detailList = JsonConvert.SerializeObject(batchDetails);
                    HttpContext.Session.SetString("detailList", detailList);
                    //HttpContext.Session.Remove("ImportDate");
                    return RedirectToPage("./CheckOutBatch");
                }
               
            }
           
        }
        public IActionResult OnPostChooseMaterialAsync(Guid MaterialId)
        {
            BatchDetail.MaterialId = MaterialId;
            return RedirectToPage("./Create", new { materialId = MaterialId });
        }
    }
}
