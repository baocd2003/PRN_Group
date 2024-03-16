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

namespace ICQS_Management.Pages.BatchsManagement
{
    public class CreateModel : PageModel
    {
        private readonly DataAccessLayer.ApplicationDbContext.applicationDbContext _context;
        private BatchManagementRepository _repo = new BatchManagementRepository();

        public CreateModel(DataAccessLayer.ApplicationDbContext.applicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Batch Batch { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (Batch == null)
            {
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
            HttpContext.Session.SetString("ImportDate" , Batch.ImportDate.ToString());
            return RedirectToPage("/AdminManagement/BatchDetailsManagement/Create");
        }


  
    }
}
