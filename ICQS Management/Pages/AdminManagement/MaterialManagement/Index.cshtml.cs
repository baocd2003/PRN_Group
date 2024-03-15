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

namespace ICQS_Management.Pages.AdminManagement.MaterialManagement
{
    public class IndexModel : PageModel
    {
        private IMaterialManagementRepository _materialRepository = new MaterialManagementRepository();
        private readonly IBaseRepository<Material> _baseRepository = new BaseRepository<Material>();
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 6;
        public int PageCount => (int)Math.Ceiling((double)TotalRecords / PageSize);
        public int TotalRecords { get; set; }

        public List<Material> Material { get; set; } = default!;

        public async Task OnGetAsync(int? pageNumber)
        {
            if (pageNumber.HasValue)
            {
                PageNumber = pageNumber.Value;
            }
            PageNumber = pageNumber ?? 1;
            TotalRecords = _baseRepository.GetTotalCount();
            Material = await _materialRepository.GetMaterialsPaged(PageNumber, PageSize);
        }
    }
}
