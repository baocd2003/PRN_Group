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
using Repository;
using BusinessObject.DTO;

namespace ICQS_Management.Pages.CustomerManagement.QuotationManagement
{
    public class CheckQuotationModel : PageModel
    {
        private readonly DataAccessLayer.ApplicationDbContext.applicationDbContext _context;

        private BatchManagementRepository _repo = new BatchManagementRepository();
        public CheckQuotationModel(DataAccessLayer.ApplicationDbContext.applicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Quotation Quotation { get; set; }

        [BindProperty]
        public Project Project { get; set; }

        [BindProperty]
        public List<ProjectMaterialDTO> ProjectMaterials { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Quotation = await _context.Quotations
                .Include(q => q.Project).FirstOrDefaultAsync(m => m.QuotationId == id);

            if (Quotation == null)
            {
                return NotFound();
            }
            TempData["id"] = id;
           ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectID", "Description");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            Guid quoteId = (Guid)TempData["id"];
            if (Request.Form.ContainsKey("confirmBut"))
            { 
                _repo.MinusQuantityInBatch(quoteId);
            }
            else
            {
                _repo.ClearAffectedBatches(quoteId);
            }
            return RedirectToPage("./Index");
        }

       
    }
}
