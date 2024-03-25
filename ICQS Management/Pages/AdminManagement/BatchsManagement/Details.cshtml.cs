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
using Repository.Interface;

namespace ICQS_Management.Pages.BatchsManagement
{
    public class DetailsModel : PageModel
    {
        private IBatchManagement _repo;

        public DetailsModel(IBatchManagement repo) {
            _repo = repo;
        }
        public Batch Batch { get; set; } = default!;
        public IList<BatchDetail> BatchDetails { get; set; } = default!;

        [BindProperty]
        public Guid BId { get; set; }
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
                    if (id == null)
                    {
                        return NotFound();
                    }
                    BatchDetails = _repo.GetBatchDetailsByBatchId((Guid)id).ToList();
                    return Page();
                }
            }
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
