using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Repository.Interface;
using Repository;
using BusinessObject.DTO;

namespace ICQS_Management.Pages.AdminManagement.MaterialManagement
{
    public class IndexModel : PageModel
    {
        private IMaterialManagementRepository _materialRepository;
        private readonly IBaseRepository<Material> _baseRepository;
        public IndexModel(IMaterialManagementRepository materialRepository, IBaseRepository<Material> baseRepository)
        {
            _baseRepository = baseRepository;
            _materialRepository = materialRepository;
        }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public int PageCount => (int)Math.Ceiling((double)TotalRecords / PageSize);
        public int TotalRecords { get; set; }
        public string message { get; set; } = string.Empty;
        public List<MaterialDTO> MaterialDTO { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? pageNumber, string ? Message)
        {
            if(Message != null)
            {
                message = Message;
            }
            if (HttpContext.Session == null)
            {
                return RedirectToPage("/Authentication/ErrorSession");
            }
            else
            {
                string userRole = HttpContext.Session.GetString("userRole");
                if (string.IsNullOrEmpty(userRole) || (userRole != "admin"))
                {
                    return RedirectToPage("/Authentication/ErrorSession");
                }
                else
                {
                    if (pageNumber.HasValue)
                    {
                        PageNumber = pageNumber.Value;
                    }
                    PageNumber = pageNumber ?? 1;
                    TotalRecords = _baseRepository.GetTotalCount();
                    MaterialDTO = await _materialRepository.GetMaterialDTOsPaged(PageNumber, PageSize);
                    return Page();
                }
            }
        }
    }
}
