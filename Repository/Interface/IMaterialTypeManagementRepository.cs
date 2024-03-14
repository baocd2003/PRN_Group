using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface;

public interface IMaterialTypeManagementRepository
{
    IEnumerable<MaterialType> GetAllMaterialTypes();
    MaterialType GetMaterialTypeById(Guid id);
}
