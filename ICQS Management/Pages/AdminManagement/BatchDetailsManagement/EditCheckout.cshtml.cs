using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Newtonsoft.Json;
using Repository.Interface;
using Repository;

namespace ICQS_Management.Pages.BatchDetailsManagement
{
    public class EditCheckoutModel : PageModel
    {
        private readonly DataAccessLayer.ApplicationDbContext.applicationDbContext _context;
        private IMaterialManagementRepository _materialRepo = new MaterialManagementRepository();

        public EditCheckoutModel(DataAccessLayer.ApplicationDbContext.applicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BatchDetail BatchDetail { get; set; }

        [BindProperty]
        private int DetailIndex { get; set; }
        public async Task<IActionResult> OnGetAsync(int index)
        {
            if (index == null)
            {
                return NotFound();
            }
            string detailListJson = HttpContext.Session.GetString("detailList");
            List<BatchDetail> batchDetails = JsonConvert.DeserializeObject<List<BatchDetail>>(detailListJson);
            BatchDetail = batchDetails[index];
            DetailIndex = index;
            TempData["IndexValue"] = index;
            if (BatchDetail == null)
            {
                return NotFound();
            }
           ViewData["MaterialId"] = new SelectList(_materialRepo.GetOthersMaterial(batchDetails), "MaterialId", "Name", batchDetails[index].MaterialId);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            string detailListJson = HttpContext.Session.GetString("detailList");
            List<BatchDetail> batchDetails = JsonConvert.DeserializeObject<List<BatchDetail>>(detailListJson);
            int index = 0;
            if (TempData.ContainsKey("IndexValue"))
            {
               index = (int)TempData["IndexValue"];
            }
                batchDetails[index] = BatchDetail;
            string detailList = JsonConvert.SerializeObject(batchDetails);
            HttpContext.Session.SetString("detailList", detailList);
            return RedirectToPage("./CheckOutBatch");
        }
    }
}
