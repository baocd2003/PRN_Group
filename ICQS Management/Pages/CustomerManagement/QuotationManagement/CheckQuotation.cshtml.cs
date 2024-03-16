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
using Repository.Interface;

namespace ICQS_Management.Pages.CustomerManagement.QuotationManagement
{
    public class CheckQuotationModel : PageModel
    {
        private readonly DataAccessLayer.ApplicationDbContext.applicationDbContext _context;

        private IBatchManagement _batchRepo = new BatchManagementRepository();
        private IProjectManagementRepository _projectRepo = new ProjectManagementRepository();
        private IMaterialManagementRepository _materialRepo = new MaterialManagementRepository();
        public CheckQuotationModel(DataAccessLayer.ApplicationDbContext.applicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Quotation Quotation { get; set; }

        [BindProperty]
        public Project Project { get; set; }

        [BindProperty]
        public List<ProjectMaterialDTO> ProjectMaterialList { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (HttpContext.Session == null)
            {
                return RedirectToPage("/Authentication/ErrorSession");
            }
            else
            {
                string userRole = HttpContext.Session.GetString("userRole");
                if (string.IsNullOrEmpty(userRole) || (userRole != "Customer"))
                {
                    return RedirectToPage("/Authentication/ErrorSession");
                }
                else
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    Quotation = _batchRepo.GetQuotationWithProject((Guid)id);
                    Project = _projectRepo.GetProjectByQuoteId(Quotation.QuotationId);
                    var projectMaterials = _projectRepo.GetProjectMaterialByProjectId(Quotation.ProjectId);
                    var materials = _materialRepo.GetAllMaterials();
                    ProjectMaterialList = (from pm in projectMaterials
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
                    if (Quotation == null)
                    {
                        return NotFound();
                    }
                    TempData["id"] = id;
                    return Page();
                }
            }
        }

            public async Task<IActionResult> OnPostAsync()
                {

                    Guid quoteId = (Guid)TempData["id"];
                    if (Request.Form.ContainsKey("confirmBut"))
                    {
                        _batchRepo.MinusQuantityInBatch(quoteId);
                    }
                    else
                    {
                        _batchRepo.DeleteQuotation(quoteId);
                    }
                    return RedirectToPage("./Index");
                }
            } 

}
