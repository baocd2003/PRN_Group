using BusinessObject.Entity;
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
}
