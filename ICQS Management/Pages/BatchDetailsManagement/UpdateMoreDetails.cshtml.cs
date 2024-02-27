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

namespace ICQS_Management.Pages.BatchDetailsManagement
{
    public class UpdateMoreDetailsModel : PageModel
    {
        private readonly DataAccessLayer.ApplicationDbContext.applicationDbContext _context;
        private BatchManagementRepository _repo = new BatchManagementRepository();

        public UpdateMoreDetailsModel(DataAccessLayer.ApplicationDbContext.applicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Guid BatchId { get; set; }


        public IActionResult OnGet(Guid id)
        {
        ViewData["MaterialId"] = new SelectList(_context.Materials, "MaterialId", "Name");
            BatchId = id;
            return Page();
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
                ViewData["MaterialId"] = new SelectList(_context.Materials, "MaterialId", "Name");
                string detailList = JsonConvert.SerializeObject(batchDetails);
                HttpContext.Session.SetString("moreDetailList", detailList);
                return Page();
            }
            else
            {
                _repo.AddMoreDetailsInBatch(batchDetails);
                HttpContext.Session.Remove("moreDetailList");
                return RedirectToPage("/BatchsManage");
            }
        }
    }
}
