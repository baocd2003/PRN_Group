using BussinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface;

public interface IProjectManagementRepository
{
    IEnumerable<Project> GetAllProjects();
    Project GetProjectById(Guid id);
    Project AddProject(Project project);
    void AddProjectMaterial(ProjectMaterial projetMaterial);
    bool checkProjectExist(Project project);
    bool checkUpdatedProjectExist(Project project);
    void UpdateProject(Project project);
    IEnumerable<ProjectMaterial> GetAllProjectMaterial();
    IEnumerable<ProjectMaterial> GetProjectMaterialByProjectId(Guid projectID);
    ProjectMaterial GetProjectMaterialByProjectMaterialId(Guid projectMaterialID);
    void ChangeProjectStatus(Project project);
    void UpdateProjectMaterial(ProjectMaterial projectMaterial);
    void DeleteProjectMaterial(Guid projectMaterialID);
    float CalculateProjectMaterialPrice(Guid projectID);
    void UpdateProjectTotalPrice(Guid id);
}