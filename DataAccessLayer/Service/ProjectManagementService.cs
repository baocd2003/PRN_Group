using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
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
    public IEnumerable<Project> GetAllProjects()
    {
        return this.Projects.Where(p => p.Status == 0).ToList();
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
}
