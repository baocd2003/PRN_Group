﻿using BussinessObject.Entity;
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
}