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
using Repository.Interface;
using Repository;
using BusinessObject.DTO;

namespace ICQS_Management.Pages.Account_Staff
{
    public class ApplyBatchModel : PageModel
    {
        private readonly DataAccessLayer.ApplicationDbContext.applicationDbContext _context;
        private IBatchManagement _batchRepo = new BatchManagementRepository();
        private IProjectManagementRepository _projectRepo = new ProjectManagementRepository();
        private IMaterialManagementRepository _materialRepo = new MaterialManagementRepository();
        private IQuotationManagementRepository _quoteRepo = new QuotationManagementRepository();
        public ApplyBatchModel(DataAccessLayer.ApplicationDbContext.applicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Quotation Quotation { get; set; }

        [BindProperty]
        public List<ProjectMaterialDTO> ProjectMaterials { get; set; }

        [BindProperty]
        public List<Batch> Batches { get; set; }

        [BindProperty]
        public List<Guid> SelectedItems { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SelectedItems = new List<Guid>();
            Quotation = await _context.Quotations
                .Include(q => q.Project).FirstOrDefaultAsync(m => m.QuotationId == id);
            var projectMaterials = _projectRepo.GetProjectMaterialByProjectId(Quotation.ProjectId);
            TempData["QuotationId"] = id;
            var materials = _materialRepo.GetAllMaterials();
            ProjectMaterials = (from pm in projectMaterials
                                   join m in materials on pm.MaterialId equals m.MaterialId
                                   where pm.ProjectId == Quotation.ProjectId
                                   select new ProjectMaterialDTO
                                   {
                                       ProjectMaterialId = pm.ProjectMaterialId,
                                       ProjectId = pm.ProjectId,
                                       MaterialId = pm.MaterialId,
                                       MaterialName = m.Name,
                                       Quantity = pm.Quantity
                                   }).ToList();
            Batches = _batchRepo.GetBatchesDateAsc();
            //foreach (var batch in Batches)
            //{
            //    batch.BatchDetails = _context.BatchDetails.Where(bd => bd.BatchId == batch.BatchId).Include(bd => bd.Materials).ToList();
            //}
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public IActionResult OnPostForm1()
        {           
            return RedirectToPage("./Index");
        }
        public IActionResult OnPostForm2()
        {
            Guid QuotationId = (Guid)TempData["QuotationId"];
            if (!_batchRepo.CheckAvailableBatchForQuote(QuotationId, SelectedItems))
            {
                TempData["QuotationId"] = QuotationId;
                Quotation = _quoteRepo.GetQuotation(QuotationId);
                TempData["ErrorMessage"] = "Materials in selected batchs not enough";
                var projectMaterials = _projectRepo.GetProjectMaterialByProjectId(Quotation.ProjectId);
                var materials = _materialRepo.GetAllMaterials();
                ProjectMaterials = (from pm in projectMaterials
                                    join m in materials on pm.MaterialId equals m.MaterialId
                                    where pm.ProjectId == Quotation.ProjectId
                                    select new ProjectMaterialDTO
                                    {
                                        ProjectMaterialId = pm.ProjectMaterialId,
                                        ProjectId = pm.ProjectId,
                                        MaterialId = pm.MaterialId,
                                        MaterialName = m.Name,
                                        Quantity = pm.Quantity
                                    }).ToList();
                Batches = _batchRepo.GetBatchesDateAsc();
                return Page();
            }
            _batchRepo.UpdateQuantityInBatch(QuotationId, SelectedItems);
            return RedirectToPage("/AdminManagement/BatchsManagement/Index");
        }

    }
}
