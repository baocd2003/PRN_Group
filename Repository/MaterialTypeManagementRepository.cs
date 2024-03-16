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
    public void AddMaterialType(MaterialType materialType)
    {
        MaterialTypeManagementService.Instance.AddMaterialType(materialType);
    }

    public bool checkMaterialTypeExist(MaterialType materialType)
    {
        return MaterialTypeManagementService.Instance.checkMaterialTypeExist(materialType);
    }

    public bool checkUpdatedMaterialTypeExist(MaterialType materialType)
    {
        return MaterialTypeManagementService.Instance.checkUpdatedMaterialTypeExist(materialType);
    }

    public IEnumerable<MaterialType> GetAllMaterialTypes()
    {
        return MaterialTypeManagementService.Instance.GetAllMaterialTypes();
    }

    public MaterialType GetMaterialTypeById(Guid id)
    {
        return MaterialTypeManagementService.Instance.GetMaterialTypeById(id);
    }

    public void UpdateMaterialType(MaterialType materialType)
    {
        MaterialTypeManagementService.Instance.UpdateMaterialType(materialType);
    }
}
