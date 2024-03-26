using System;
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
        private IBatchManagement _repo;
        private IMaterialManagementRepository _materialRepo;
        private IMaterialTypeManagementRepository _typeRepo;
        public UpdateMoreDetailsModel(IBatchManagement repo , IMaterialManagementRepository materialRepo, IMaterialTypeManagementRepository typeRepo)
        {
            _repo = repo;
            _materialRepo = materialRepo;
            _typeRepo = typeRepo;
        }

        [BindProperty]
        public Guid BatchId { get; set; }
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
                    var id = HttpContext.Session.GetString("selectedBatchId");
                    List<BatchDetail> list = _repo.GetBatchDetailsByBatchId(Guid.Parse(id));
                    //List<BatchDetail> list = _context.BatchDetails.Where(bd => bd.BatchId == id).ToList();
                    if (materialId != null)
                    {
                        ViewData["MaterialId"] = new SelectList(_materialRepo.GetOthersMaterial(list), "MaterialId", "Name", materialId);
                        Material mat = _materialRepo.GetMaterialById((Guid)materialId);
                        MaterialType = _typeRepo.GetMaterialTypeById(mat.MaterialTypeId);
                        TempData["MatId"] = (Guid)materialId;
                        MediumPrice = mat.MediumPrice;
                    }
                    else
                    {
                        ViewData["MaterialId"] = new SelectList(_materialRepo.GetOthersMaterial(list), "MaterialId", "Name");
                        List<Material> matList = _materialRepo.GetOthersMaterial(list).ToList();
                        Material mat = matList.FirstOrDefault();
                        MaterialType = _typeRepo.GetMaterialTypeById(mat.MaterialTypeId);
                        TempData["MatId"] = mat.MaterialId;
                        MediumPrice = mat.MediumPrice;
                    }
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
            BatchDetail.MaterialId = (Guid)TempData["MatId"];
            string detailListJson = HttpContext.Session.GetString("moreDetailList");
            List<BatchDetail> batchDetails = JsonConvert.DeserializeObject<List<BatchDetail>>(detailListJson);
            batchDetails.Add(BatchDetail);
            var continueCheckbox = Request.Form["continueCheckbox"];
            if (!string.IsNullOrEmpty(continueCheckbox))
            {
                List<BatchDetail> list = _repo.GetBatchDetailsByBatchId(selectedBatchId);
                ViewData["MaterialId"] = new SelectList(_materialRepo.GetOthersMaterial(list), "MaterialId", "Name");
                List<Material> matList = _materialRepo.GetOthersMaterial(list).ToList();
                Material mat = matList.FirstOrDefault();
                MaterialType = _typeRepo.GetMaterialTypeById(mat.MaterialTypeId);
                MediumPrice = mat.MediumPrice;
                string detailList = JsonConvert.SerializeObject(batchDetails);
                HttpContext.Session.SetString("moreDetailList", detailList);
                return Page();
            }
            else
            {
                _repo.AddMoreDetailsInBatch(batchDetails);
                HttpContext.Session.Remove("moreDetailList");
                return RedirectToPage("/AdminManagement/BatchsManagement/Index");
            }
        }

        public IActionResult OnPostChooseMaterialAsync(Guid MaterialId)
        {
            BatchDetail.MaterialId = MaterialId;
            return RedirectToPage("./UpdateMoreDetails", new { materialId = MaterialId });
        }
    }
}
