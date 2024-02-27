﻿using System;
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

namespace ICQS_Management.Pages.ProjectManagement
{
    public class IndexModel : PageModel
    {
        private IProjectManagementRepository _pmRepository = new ProjectManagementRepository();

        public List<Project> Project { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Project = await Task.Run(() => _pmRepository.GetAllProjects().ToList());
        }
    }
}