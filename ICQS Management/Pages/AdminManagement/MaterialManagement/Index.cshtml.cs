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

        public List<Material> Material { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Material = await Task.Run(() => _materialRepository.GetAllMaterials().ToList());
        }
    }
}
