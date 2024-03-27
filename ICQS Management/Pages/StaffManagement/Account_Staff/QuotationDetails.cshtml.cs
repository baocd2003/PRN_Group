using BusinessObject.DTO;
using BussinessObject.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Interface;

namespace ICQS_Management.Pages.StaffManagement.Account_Staff
{
    public class QuotationDetailsModel : PageModel
    {

        private IBatchManagement _batchRepo;
        private IProjectManagementRepository _projectRepo;
        private IMaterialManagementRepository _materialRepo;

        private IQuotationManagementRepository _quoteRepo;

        public QuotationDetailsModel(IBatchManagement batchRepo,
            IProjectManagementRepository projectRepo,
            IMaterialManagementRepository materialRepo,
            IQuotationManagementRepository quotRepo)
        {
            _batchRepo = batchRepo;
            _projectRepo = projectRepo;
            _materialRepo = materialRepo;
            _quoteRepo = quotRepo;
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
                if (string.IsNullOrEmpty(userRole) || (userRole != "Staff"))
                {
                    return RedirectToPage("/Authentication/ErrorSession");
                }
                else
                {
                    Quotation = _quoteRepo.GetQuotation(id.Value);

                    if (Quotation == null)
                    {
                        return NotFound();
                    }
                    return Page();
                }
            }
        }
    }
}
