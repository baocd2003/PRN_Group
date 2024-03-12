using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Repository;
using Newtonsoft.Json;

namespace ICQS_Management.Pages.BatchsManagement
{
    public class DetailsModel : PageModel
    {
        private readonly DataAccessLayer.ApplicationDbContext.applicationDbContext _context;
        private BatchManagementRepository _repo = new BatchManagementRepository();

        public DetailsModel(DataAccessLayer.ApplicationDbContext.applicationDbContext context)
        {
            _context = context;
        }

        public Batch Batch { get; set; } = default!;
        public IList<BatchDetail> BatchDetails { get; set; } = default!;

        [BindProperty]
        public Guid BId { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            BatchDetails = _repo.GetBatchDetailsByBatchId((Guid)id).ToList();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {

            List<BatchDetail> batchDetail = new List<BatchDetail>();
            string detailList = JsonConvert.SerializeObject(batchDetail);
            HttpContext.Session.SetString("moreDetailList", detailList);
            HttpContext.Session.SetString("selectedBatchId", BId.ToString());
            return RedirectToPage("/AdminManagement/BatchDetailsManagement/UpdateMoreDetails");
        }
    }
}
