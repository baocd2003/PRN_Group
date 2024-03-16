using BusinessObject.Entity;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Service;

public class MaterialTypeManagementService : applicationDbContext
{
    private static MaterialTypeManagementService instance = null;
    private static readonly object instanceLock = new object();
    private MaterialTypeManagementService()
    {

    }
    public static MaterialTypeManagementService Instance
    {
        get
        {
            lock (instanceLock)
            {
                if (instance == null)
                {
                    instance = new MaterialTypeManagementService();
                }
                return instance;
            }
        }
    }
    public IEnumerable<MaterialType> GetAllMaterialTypes()
    {
        return this.MaterialTypes.ToList();
    }

    public MaterialType GetMaterialTypeById(Guid id)
    {
        return this.MaterialTypes.FirstOrDefault(c => c.MaterialTypeId.Equals(id));
    }
    public void AddMaterialType(MaterialType materialType)
    {
        this.MaterialTypes.Add(materialType);
        this.SaveChanges();
    }
    public void UpdateMaterialType(MaterialType materialType)
    {
        var updatedMaterialType = GetMaterialTypeById(materialType.MaterialTypeId);
        if (updatedMaterialType != null)
        {
            updatedMaterialType.MaterialTypeName = materialType.MaterialTypeName;
            updatedMaterialType.UnitType = materialType.UnitType;
            this.SaveChanges(true);
        }
    }
    public bool checkMaterialTypeExist(MaterialType materialType)
    {
        return (GetAllMaterialTypes().Where(m => (m.MaterialTypeName.Equals(materialType.MaterialTypeName))).Count() > 0);
    }
    public bool checkUpdatedMaterialTypeExist(MaterialType materialType)
    {
        var allMaterialTypes = GetAllMaterialTypes();
        return allMaterialTypes.Any(m => m.MaterialTypeName == materialType.MaterialTypeName && m.MaterialTypeId != materialType.MaterialTypeId);
    }
}
