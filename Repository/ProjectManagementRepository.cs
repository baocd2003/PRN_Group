using BussinessObject.Entity;
using DataAccessLayer.Service;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository;

public class ProjectManagementRepository : IProjectManagementRepository
{
    public Project AddProject(Project project)
    {
        return ProjectManagementService.Instance.AddProject(project);
    }

    public void AddProjectMaterial(ProjectMaterial projetMaterial)
    {
        ProjectManagementService.Instance.AddProjectMaterial(projetMaterial);
    }

    public IEnumerable<Project> GetAllProjects()
    {
        return ProjectManagementService.Instance.GetAllProjects();
    }

    public Project GetProjectById(Guid id)
    {
        return ProjectManagementService.Instance.GetProjectById(id);
    }
}
