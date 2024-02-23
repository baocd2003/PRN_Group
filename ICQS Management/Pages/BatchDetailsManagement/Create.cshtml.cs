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

namespace ICQS_Management.Pages.BatchDetailsManagement
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
       ;
        ViewData["MaterialId"] = new SelectList(_context.Materials, "MaterialId", "Name");
            return Page();
        }

        [BindProperty]
        public BatchDetail BatchDetail { get; set; } = default!;
        
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
                ViewData["MaterialId"] = new SelectList(_context.Materials, "MaterialId", "Name");
                string detailList = JsonConvert.SerializeObject(batchDetails);
                HttpContext.Session.SetString("detailList", detailList);
                return Page();
            }else
            {
                _repo.CreateBatch(ImportDate, batchDetails);
                HttpContext.Session.Remove("ImportDate");
                return RedirectToPage("./Index");
            }
           
        }
    }
}
