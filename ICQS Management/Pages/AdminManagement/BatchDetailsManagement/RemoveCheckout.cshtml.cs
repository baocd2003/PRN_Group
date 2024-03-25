using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace ICQS_Management.Pages.BatchDetailsManagement
{
    public class RemoveCheckoutModel : PageModel
    {
        private readonly DataAccessLayer.ApplicationDbContext.applicationDbContext _context;

        public RemoveCheckoutModel(DataAccessLayer.ApplicationDbContext.applicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BatchDetail BatchDetail { get; set; }

        [BindProperty]
        private int DetailIndex { get; set; }
        public async Task<IActionResult> OnGetAsync(int index)
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
                    if (index == null)
                    {
                        return NotFound();
                    }
                    string detailListJson = HttpContext.Session.GetString("detailList");
                    List<BatchDetail> batchDetails = JsonConvert.DeserializeObject<List<BatchDetail>>(detailListJson);
                    BatchDetail = batchDetails[index];
                    TempData["IndexValue"] = index;
                    DetailIndex = index;
                    if (BatchDetail == null)
                    {
                        return NotFound();
                    }
                    ViewData["MaterialId"] = new SelectList(_context.Materials, "MaterialId", "Name", batchDetails[index].MaterialId);
                    return Page();
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string detailListJson = HttpContext.Session.GetString("detailList");
            List<BatchDetail> batchDetails = JsonConvert.DeserializeObject<List<BatchDetail>>(detailListJson);
            int index = 0;
            if (TempData.ContainsKey("IndexValue"))
            {
                index = (int)TempData["IndexValue"];
            }
            batchDetails.RemoveAt(index);
            string detailList = JsonConvert.SerializeObject(batchDetails);
            HttpContext.Session.SetString("detailList", detailList);
            return RedirectToPage("./Create");
        }
    }
}
