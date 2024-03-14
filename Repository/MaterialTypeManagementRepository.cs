using BusinessObject.Entity;
using Repository.Interface;
using DataAccessLayer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository;

public class MaterialTypeManagementRepository : IMaterialTypeManagementRepository
{
    public IEnumerable<MaterialType> GetAllMaterialTypes()
    {
        return MaterialTypeManagementService.Instance.GetAllMaterialTypes();
    }

    public MaterialType GetMaterialTypeById(Guid id)
    {
        return MaterialTypeManagementService.Instance.GetMaterialTypeById(id);
    }
}
