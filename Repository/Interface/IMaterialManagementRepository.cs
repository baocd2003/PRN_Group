using BussinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface;

public interface IMaterialManagementRepository
{
    IEnumerable<Material> GetAllMaterials();
    IEnumerable<Material> GetOthersMaterial(List<BatchDetail> batchDetails);
}
