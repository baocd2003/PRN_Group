using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Service;

public class ProjectManagementService : applicationDbContext
{
    private static ProjectManagementService instance = null;
    private static readonly object instanceLock = new object();
    private ProjectManagementService()
    {

    }
    public static ProjectManagementService Instance
    {
        get
        {
            lock (instanceLock)
            {
                if (instance == null)
                {
                    instance = new ProjectManagementService();
                }
                return instance;
            }
        }
    }
    public async Task<List<Project>> GetAllPaging(int pageNumber, int pageSize)
    {
        using (applicationDbContext _db = new applicationDbContext())
        {
            var listQuerry = await _db.Projects.Include(x => x.ProjectMaterials).OrderBy(p => p.Status == 1 ? 0 : p.Status == 2 ? 1 : 2).ThenBy(p => p.ProjectName).ToListAsync();
            return listQuerry.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }
    }
    public IEnumerable<Project> GetAllProjects()
    {
        return this.Projects.ToList();
    }

    public Project GetProjectById(Guid id)
    {
        return this.Projects.FirstOrDefault(c => c.ProjectID.Equals(id));
    }

    public Project AddProject(Project project)
    {
        this.Projects.Add(project);
        this.SaveChanges();
        return project;
    }
    public void AddProjectMaterial(ProjectMaterial projetMaterial)
    {
        this.ProjectMaterials.Add(projetMaterial);
        this.SaveChanges();
    }
    public bool checkProjectExist(Project project)
    {
        return (GetAllProjects().Where(p => (p.ProjectName.Equals(project.ProjectName))).Count() > 0);
    }
    public bool checkUpdatedProjectExist(Project project)
    {
        var allProjects = GetAllProjects();
        return allProjects.Any(p => p.ProjectName == project.ProjectName && p.ProjectID != project.ProjectID);
    }
    public void UpdateProject(Project project)
    {
        var updatedProject = GetProjectById(project.ProjectID);
        if (updatedProject != null)
        {
            updatedProject.ProjectName = project.ProjectName;
            updatedProject.AreaPerFloor = project.AreaPerFloor;
            updatedProject.NumOfFloors = project.NumOfFloors;
            updatedProject.Description = project.Description;
            updatedProject.NumOfLabors = project.NumOfLabors;
            updatedProject.LaborSalaryPerMonth = project.LaborSalaryPerMonth;
            updatedProject.MonthDuration = project.MonthDuration;
            updatedProject.TotalPrice = project.TotalPrice;
            updatedProject.Status = project.Status;
            this.SaveChanges(true);
        }
    }
    public IEnumerable<ProjectMaterial> GetAllProjectMaterial()
    {
        return this.ProjectMaterials.ToList();
    }
    public IEnumerable<ProjectMaterial> GetProjectMaterialByProjectId(Guid projectID)
    {
        return this.ProjectMaterials.Include(p => p.Materials).ThenInclude(p => p.MaterialTypes).Where(pm => pm.ProjectId == projectID).ToList();
    }
    public ProjectMaterial GetProjectMaterialByProjectMaterialId(Guid projectMaterialID)
    {
        return this.ProjectMaterials.FirstOrDefault(p => p.ProjectMaterialId.Equals(projectMaterialID));
    }
    public void ChangeProjectStatus(Project project)
    {
        if (project.Status == 1)
        {
            project.Status = 0;
        }
        else if (project.Status == 0)
        {
            project.Status = 1;
        }
        this.SaveChanges();
    }
    public void UpdateProjectMaterial(ProjectMaterial projectMaterial)
    {
        var updatedProjectMaterial = GetProjectMaterialByProjectMaterialId(projectMaterial.ProjectMaterialId);
        if (updatedProjectMaterial != null)
        {
            updatedProjectMaterial.MaterialId = projectMaterial.MaterialId;
            updatedProjectMaterial.Quantity = projectMaterial.Quantity;
            this.SaveChanges();
        }
    }
    public void DeleteProjectMaterial(Guid projectMaterialID)
    {
        var deletedProjectMaterial = GetProjectMaterialByProjectMaterialId(projectMaterialID);
        if (deletedProjectMaterial != null)
        {
            this.ProjectMaterials.Remove(deletedProjectMaterial);
            this.SaveChanges();
        }
    }
    public float CalculateProjectMaterialPrice(Guid projectID)
    {
        List<ProjectMaterial> projectMaterials = GetProjectMaterialByProjectId(projectID).ToList();
        float sum = 0;
        foreach (ProjectMaterial projectMaterial in projectMaterials)
        {
            int quantity = projectMaterial.Quantity;
            Material material = MaterialManagementService.Instance.GetMaterialById(projectMaterial.MaterialId);
            sum += quantity * material.MediumPrice;
        }
        return sum;
    }
    public void UpdateProjectTotalPrice(Guid projectId)
    {
        var updatedProject = GetProjectById(projectId);
        float projectMaterialPrice = CalculateProjectMaterialPrice(projectId);
        if (updatedProject != null)
        {
            updatedProject.TotalPrice = projectMaterialPrice + (updatedProject.LaborSalaryPerMonth * updatedProject.MonthDuration * updatedProject.NumOfLabors);
            this.SaveChanges();
        }
    }

    public Project GetProjectByQuoteId(Guid quoteid)
    {
        Quotation _selectedQuotation = this.Quotations.FirstOrDefault(q => q.QuotationId == quoteid);
        Project _selectedProject = this.Projects.FirstOrDefault(p => p.ProjectID == _selectedQuotation.ProjectId);
        return _selectedProject;
    }

}
