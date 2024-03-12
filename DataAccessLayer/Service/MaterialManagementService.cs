using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Service;

public class MaterialManagementService : applicationDbContext
{
    private static MaterialManagementService instance = null;
    private static readonly object instanceLock = new object();
    private MaterialManagementService()
    {

    }
    public static MaterialManagementService Instance
    {
        get
        {
            lock (instanceLock)
            {
                if (instance == null)
                {
                    instance = new MaterialManagementService();
                }
                return instance;
            }
        }
    }
    public IEnumerable<Material> GetAllMaterials()
    {
        return this.Materials.ToList();
    }
    public IEnumerable<Material> GetOthersMaterial(List<BatchDetail> batchDetails)
    {
        var selectedMaterials = batchDetails.Select(b => b.MaterialId);

        var otherMaterials = this.Materials.Where(m => !selectedMaterials.Contains(m.MaterialId));

        return otherMaterials;
    }
    public Material AddMaterial(Material material)
    {
        this.Materials.Add(material);
        this.SaveChanges();
        return material;
    }
    public bool checkMaterialExist(Material material)
    {
        return (GetAllMaterials().Where(m => (m.Name.Equals(material.Name))).Count() > 0);
    }
    public bool checkUpdatedMaterialExist(Material material)
    {
        var allMaterials = GetAllMaterials();
        return allMaterials.Any(m => m.Name == material.Name && m.MaterialId != material.MaterialId);
    }
}
