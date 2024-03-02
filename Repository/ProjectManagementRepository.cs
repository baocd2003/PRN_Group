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

    public void ChangeProjectStatus(Project project)
    {
        ProjectManagementService.Instance.ChangeProjectStatus(project);
    }

    public bool checkProjectExist(Project project)
    {
        return ProjectManagementService.Instance.checkProjectExist(project);
    }

    public bool checkUpdatedProjectExist(Project project)
    {
        return ProjectManagementService.Instance.checkUpdatedProjectExist(project);
    }

    public void DeleteProjectMaterial(Guid projectMaterialID)
    {
        ProjectManagementService.Instance.DeleteProjectMaterial(projectMaterialID);
    }

    public IEnumerable<ProjectMaterial> GetAllProjectMaterial()
    {
        return ProjectManagementService.Instance.GetAllProjectMaterial();
    }

    public IEnumerable<Project> GetAllProjects()
    {
        return ProjectManagementService.Instance.GetAllProjects();
    }

    public Project GetProjectById(Guid id)
    {
        return ProjectManagementService.Instance.GetProjectById(id);
    }

    public IEnumerable<ProjectMaterial> GetProjectMaterialByProjectId(Guid projectID)
    {
        return ProjectManagementService.Instance.GetProjectMaterialByProjectId(projectID);
    }

    public ProjectMaterial GetProjectMaterialByProjectMaterialId(Guid projectMaterialID)
    {
        return ProjectManagementService.Instance.GetProjectMaterialByProjectMaterialId(projectMaterialID);
    }

    public void UpdateProject(Project project)
    {
        ProjectManagementService.Instance.UpdateProject(project);
    }

    public void UpdateProjectMaterial(ProjectMaterial projectMaterial)
    {
        ProjectManagementService.Instance.UpdateProjectMaterial(projectMaterial);
    }
}
