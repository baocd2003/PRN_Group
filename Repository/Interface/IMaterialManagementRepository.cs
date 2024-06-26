using BusinessObject.DTO;
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
    Task<List<MaterialDTO>> GetMaterialDTOsPaged(int pageNumber, int pageSize);
    IEnumerable<Material> GetOthersMaterial(List<BatchDetail> batchDetails);
    Material AddMaterial(Material material);
    bool checkMaterialExist(Material material);
    bool checkUpdatedMaterialExist(Material material);
    Material GetMaterialById(Guid id);
    void UpdateMaterial(Material material);
    Material GetById(Guid id);
}