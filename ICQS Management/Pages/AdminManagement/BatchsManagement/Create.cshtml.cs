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

namespace ICQS_Management.Pages.BatchsManagement
{
    public class CreateModel : PageModel
    {
        private IBatchManagement _repo;

        public CreateModel(IBatchManagement repo)
        {
            _repo = repo;
        }

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
                    return Page();
                }
            }
        }

        [BindProperty]
        public Batch Batch { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (Batch == null)
            {
                return Page();
            }
            if(Batch.ImportDate.Date < DateTime.Now.Date)
            {
                TempData["ErrorMessage"] = "Cannot import batch in past day";
                return Page();
            }
            if (_repo.CheckOverlapBatch(Batch))
            {
                TempData["ErrorMessage"] = "This batch in this date was created";
                return Page();
            }
            List<BatchDetail> batchDetail = new List<BatchDetail>();
            string detailList = JsonConvert.SerializeObject(batchDetail);
            HttpContext.Session.SetString("detailList", detailList);
            HttpContext.Session.SetString("ImportDate", Batch.ImportDate.ToString());
            return RedirectToPage("/AdminManagement/BatchDetailsManagement/Create");
        }
    }
}
