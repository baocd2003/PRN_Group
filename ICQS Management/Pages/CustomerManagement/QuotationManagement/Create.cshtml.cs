﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Repository.Interface;
using ICQS_Management.Pages.Authentication;
using DataAccessLayer.Service;

namespace ICQS_Management.Pages.QuotationManagement
{
    public class CreateModel : PageModel
    {
        private readonly IQuotationManagementRepository _quotationRepo;
        private readonly IMaterialManagementRepository _materialRepo;
        private readonly IProjectManagementRepository _projectRepo;

        public CreateModel(IQuotationManagementRepository quotationRepo, 
            IMaterialManagementRepository materialRepo,
            IProjectManagementRepository projectRepo)
        {
            _quotationRepo = quotationRepo;
            _materialRepo = materialRepo;
            _projectRepo = projectRepo;
        }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (HttpContext.Session == null)
            {
                return RedirectToPage("/Authentication/ErrorSession");
            }
            ProjectId = id.Value;
            PopulateData();
            
            return Page();
        }

        public void PopulateData()
        {
            Project = _projectRepo.GetProjectById(ProjectId);
            if (Project == null)
            {
                throw new Exception("Project not found!");
            }
            ProjectMaterials = _projectRepo.GetProjectMaterialByProjectId(ProjectId);
            Materials = _materialRepo.GetAllMaterials();
        }

        // Project properties
        [BindProperty]
        [Required]
        public string ProjectName { get; set; }
        [BindProperty]
        [Required]
        public string Description { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Area per floor field is required!")]
        public int AreaPerFloor { get; set; } = default!;
        [BindProperty]
        [Required(ErrorMessage = "Number of floors field is required!")]
        public int NumOfFloors { get; set; } = default!;
        [BindProperty]
        [Required]
        public int NumOfLabors { get; set; }
        [BindProperty]
        [Required]
        public float LaborSalaryPerMonth { get; set; }
        [BindProperty]
        [Required]
        public int MonthDuration { get; set; }
        [BindProperty]
        public IEnumerable<Material> Materials { get; set; }
        private Project NewProject { get; set; } = new Project();

        ///////////////////////////////////////////////////////////////
        // Project Material Properties
        public class NewProjectMaterial
        {
            [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive integer!")]
            public int Quantity { get; set; } = default!;
            public Guid MaterialId { get; set; } = default!;

        } 
        [BindProperty]
        [Required]
        public IList<NewProjectMaterial> NewProjectMaterials { get; set; } = default!;
        private IList<ProjectMaterial> CreatedProjectMaterials { get; set; } = new List<ProjectMaterial>();


        //////////////////////////////////////////////////////////////////////
        // Quotation properties
        [BindProperty]
        public DateTime RequestDate { get; set; } = default!;
        [BindProperty]
        public float EstimatePrice { get; set; } = default!;
        private Quotation CreatedQuotation { get; set; } = new Quotation();
        public Project Project { get; set; } = default!;
        [BindProperty]
        public IEnumerable<ProjectMaterial> ProjectMaterials { get; set; } = default!;
        [BindProperty]
        public Guid ProjectId { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PopulateData();
                return Page();
            }

            NewProject.ProjectName = ProjectName;
            NewProject.Description = Description;
            NewProject.AreaPerFloor = AreaPerFloor;
            NewProject.NumOfFloors = NumOfFloors;
            NewProject.NumOfLabors = NumOfLabors;
            NewProject.LaborSalaryPerMonth = LaborSalaryPerMonth;
            NewProject.MonthDuration = MonthDuration;
            NewProject.TotalPrice = EstimatePrice;
            NewProject.Status = 2;

            var projectResult = _projectRepo.AddProject(NewProject);
            if (projectResult == null)
            {
                ModelState.AddModelError(string.Empty, "something wrong when create project");
                return Page();
            }

            foreach (var projectMaterial in NewProjectMaterials)
            {
                CreatedProjectMaterials.Add(new ProjectMaterial()
                {
                    ProjectId = projectResult.ProjectID,
                    MaterialId = projectMaterial.MaterialId,
                    Quantity = projectMaterial.Quantity
                });
            }
            foreach (var createdPrjMaterial in CreatedProjectMaterials)
            {
                _projectRepo.AddProjectMaterial(createdPrjMaterial);
            }

            CreatedQuotation.ProjectId = projectResult.ProjectID;
            CreatedQuotation.RequestDate = RequestDate;
            CreatedQuotation.EstimatePrice = EstimatePrice;
            CreatedQuotation.Status = 0;
            CreatedQuotation.Note = "";

            string loggedEmail = HttpContext.Session.GetString("LoggedEmail");
            Customer cus = _quotationRepo.GetCustomerByEmail(loggedEmail);
            if (cus.Quotations == null)
            {
                cus.Quotations = new List<Quotation>();
            }
            cus.Quotations.Add(CreatedQuotation);

            _quotationRepo.AddQuotation(CreatedQuotation);

            return RedirectToPage("./Index");
        }
    }
}
