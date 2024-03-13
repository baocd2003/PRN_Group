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
    public Material AddMaterial(Material material)
    {
        return MaterialManagementService.Instance.AddMaterial(material);
    }

    public bool checkMaterialExist(Material material)
    {
        return MaterialManagementService.Instance.checkMaterialExist(material);
    }

    public bool checkUpdatedMaterialExist(Material material)
    {
        return MaterialManagementService.Instance.checkUpdatedMaterialExist(material);
    }

    public IEnumerable<Material> GetAllMaterials()
    {
        return MaterialManagementService.Instance.GetAllMaterials();
    }

    public Material GetMaterialById(Guid id)
    {
        return MaterialManagementService.Instance.GetMaterialById(id);
    }

    public IEnumerable<Material> GetOthersMaterial(List<BatchDetail> batchDetails)
    {
        return MaterialManagementService.Instance.GetOthersMaterial(batchDetails);
    }

    public void UpdateMaterial(Material material)
    {
        MaterialManagementService.Instance.UpdateMaterial(material);
    }
}
