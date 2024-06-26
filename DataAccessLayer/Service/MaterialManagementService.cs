﻿using BusinessObject.DTO;
using BusinessObject.Entity;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
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
        return this.Materials.Include(m => m.MaterialTypes).ToList();
    }
    public async Task<List<MaterialDTO>> GetMaterialDTOsPaged(int pageNumber, int pageSize)
    {
        return this.Materials.OrderBy(m => m.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(material => new MaterialDTO
            {
                MaterialId = material.MaterialId,
                MaterialTypeId = material.MaterialTypeId,
                Name = material.Name,
                MediumPrice = material.MediumPrice,
                MaterialTypeName = material.MaterialTypes.MaterialTypeName,
                UnitType = material.MaterialTypes.UnitType
            })
            .ToList();
    }
    public Material GetMaterialById(Guid id)
    {
        return this.Materials.SingleOrDefault(c => c.MaterialId.Equals(id));
    }
    public IEnumerable<Material> GetOthersMaterial(List<BatchDetail> batchDetails)
    {
        var selectedMaterials = batchDetails.Select(b => b.MaterialId);

        var otherMaterials = this.Materials.Where(m => !selectedMaterials.Contains(m.MaterialId));

        return otherMaterials;
    }
    public Material AddMaterial(Material material)
    {
        try
        {
            using (applicationDbContext _db = new applicationDbContext())
            {
                _db.Materials.Add(material);
                _db.SaveChanges();
                return material;
            }

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

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
    public void UpdateMaterial(Material material)
    {
        var updatedMaterial = GetMaterialById(material.MaterialId);
        if (updatedMaterial != null)
        {
            updatedMaterial.Name = material.Name;
            updatedMaterial.MediumPrice = material.MediumPrice;
            updatedMaterial.MaterialTypeId = material.MaterialTypeId;
            this.SaveChanges(true);
        }
    }

    public static float GetMediumPriceById(Guid id)
    {
        using (applicationDbContext _db = new applicationDbContext())
        {
            return _db.Materials.FirstOrDefault(m => m.MaterialId == id).MediumPrice;
        }
    }
}
