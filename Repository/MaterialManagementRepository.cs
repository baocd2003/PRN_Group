using BussinessObject.Entity;
using DataAccessLayer.Service;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository;

public class MaterialManagementRepository : IMaterialManagementRepository
{
    public IEnumerable<Material> GetAllMaterials()
    {
        return MaterialManagementService.Instance.GetAllMaterials();
    }
}
