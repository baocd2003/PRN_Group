using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Service
{
    public class BatchManagementService : applicationDbContext
    {
        private static BatchManagementService instance;
        public static readonly object instanceLock = new object();
        private readonly applicationDbContext _db = new applicationDbContext();
        public BatchManagementService() { }
        public static BatchManagementService Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance is null)
                    {
                        instance = new BatchManagementService();
                    }
                    return instance;
                }
            }
        }

        public void CreateBatch(DateTime ImportDate, List<BatchDetail> batchDetails)
        {
            Batch batch = new Batch();
            batch.ImportDate = ImportDate;
            batch.BatchDetails = batchDetails;
            //foreach(BatchDetail batchDetail in batchDetails)
            //{
            //    CreateBatchDetails(batchDetail);
            //}
            _db.Batches.Add(batch);
            _db.SaveChanges();
        }

        public Batch GetLastBatch()
        {
            Batch batch = _db.Batches.OrderByDescending(b => b.BatchId).FirstOrDefault(); ;
            return batch;
        }

        public void CreateBatchDetails(BatchDetail batchDetail)
        {
            _db.BatchDetails.Add(batchDetail);
            _db.SaveChanges();
        }

        public List<BatchDetail> GetBatchDetailsByBatchId(Guid batchId)
        {
            List<BatchDetail> batchDetails = _db.BatchDetails.Where(bd => bd.BatchId == batchId).Include(bd => bd.Materials).ToList();
            return batchDetails;
        }

        public void AddMoreDetailsInBatch(List<BatchDetail> batchDetails)
        {
            foreach (BatchDetail batchDetail in batchDetails)
            {
                _db.BatchDetails.Add(batchDetail);
                _db.SaveChanges();
            }
        }

        public Batch GetBatchById(Guid batchId)
        {
            Batch batch = _db.Batches.FirstOrDefault(b => b.BatchId == batchId);
            return batch;
        }

        public void UpdateBatchDetail(BatchDetail batchDetail)
        {
            BatchDetail selectedDetail = _db.BatchDetails.FirstOrDefault(bd => bd.BatchDetailId == batchDetail.BatchDetailId);
            if (selectedDetail != null)
            {
                selectedDetail.Price = batchDetail.Price;
                selectedDetail.Quantity = batchDetail.Quantity;
            }
            _db.SaveChanges();
        }

        public bool CheckOverlapBatch(Batch batch)
        {
            if (_db.Batches.FirstOrDefault(b => b.ImportDate.Date == batch.ImportDate.Date) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<Quotation> GetRequestQuotation()
        {
            return _db.Quotations.Where(q => q.Status == 0).Include(q => q.Project).OrderByDescending(q => q.RequestDate).ToList();
        }
        public List<Batch> CheckAvailableQuantityBatch()
        {
            List<Batch> batchList = GetBatchesDateAsc();
            List<Batch> availableList = new List<Batch>();
            foreach (Batch b in batchList)
            {
                List<BatchDetail> batchDetails = GetBatchDetailsByBatchId(b.BatchId);
                bool flag = true;
                foreach (BatchDetail bd in batchDetails)
                {
                    if (bd.Quantity != 0)
                    {
                        flag = false;
                        break;
                    }
                }
                if (!flag)
                {
                    availableList.Add(b);
                }
            }
            return availableList;
        }
        public List<Batch> GetBatchesDateAsc()
        {
            return _db.Batches
          .OrderBy(b => b.ImportDate)
          .Include(b => b.BatchDetails)
              .ThenInclude(bd => bd.Materials)
          .ToList();
        }
        public bool CheckAvailableBatchForQuote(Guid quotationId, List<Guid> batchIds)
        {
            Quotation _selectedQuotation = _db.Quotations.FirstOrDefault(q => q.QuotationId == quotationId);
            List<ProjectMaterial> quoteMaterials = ProjectManagementService.Instance.GetProjectMaterialByProjectId(_selectedQuotation.ProjectId).ToList();

            foreach (var quoteMaterial in quoteMaterials)
            {
                double remainingQuantity = quoteMaterial.Quantity;
                double totalBatchQuantity = 0;

                foreach (Guid batchId in batchIds)
                {
                    Batch batch = _db.Batches.FirstOrDefault(q => q.BatchId == batchId);
                    List<BatchDetail> batchDetails = GetBatchDetailsByBatchId(batch.BatchId);
                    BatchDetail batchDetail = batchDetails.FirstOrDefault(bd => bd.MaterialId == quoteMaterial.MaterialId);

                    if (batchDetail != null)
                    {
                        totalBatchQuantity += batchDetail.Quantity;
                    }
                }

                if (totalBatchQuantity < remainingQuantity)
                {
                    return false;
                }
            }

            return true;
        }

        public List<Guid> SortBatchsIdByDate(List<Guid> batchIds)
        {

            List<Batch> batches = _db.Batches.Where(b => batchIds.Contains(b.BatchId)).ToList();
            batches = batches.OrderBy(b => b.ImportDate).ToList();
            List<Guid> sortedBatchIds = batches.Select(b => b.BatchId).ToList();
            return sortedBatchIds;
        }

        public void UpdateQuantityInBatch(Guid quotationId, List<Guid> batchIds, Guid staffId, Project prj)
        {
            Quotation _selectedQuotation = _db.Quotations.Include(q => q.Project).ThenInclude(p => p.ProjectMaterials)
                .ThenInclude(pm => pm.Materials).FirstOrDefault(q => q.QuotationId == quotationId);
            Project _selectedProject = _db.Projects.FirstOrDefault(p => p.ProjectID == _selectedQuotation.ProjectId);
            List<ProjectMaterial> quoteMaterials = ProjectManagementService.Instance.GetProjectMaterialByProjectId(_selectedQuotation.ProjectId).ToList();
            Staff staff = _db.Staffs.FirstOrDefault(s => s.StaffId == staffId);
            List<Guid> remainingBatchIds = SortBatchsIdByDate(batchIds);
            List<Batch> affectedBatchs = new List<Batch>();
            double price = 0;
            foreach (var quoteMaterial in quoteMaterials)
            {
                double remainingQuantity = quoteMaterial.Quantity;
                //double materialBatchQuantity = 
                foreach (Guid batchId in remainingBatchIds)
                {
                    Batch batch = _db.Batches.FirstOrDefault(q => q.BatchId == batchId);
                    List<BatchDetail> batchDetails = GetBatchDetailsByBatchId(batch.BatchId);
                    BatchDetail batchDetail = batchDetails.FirstOrDefault(bd => bd.MaterialId == quoteMaterial.MaterialId);
                    if (batchDetail != null)
                    {
                        double quantityToUpdate = batchDetail.Quantity;
                        if (quantityToUpdate >= remainingQuantity) // lo du quantity
                        {
                            quantityToUpdate -= remainingQuantity;
                            affectedBatchs.Add(batch);
                            price += remainingQuantity * batchDetail.Price;
                            _selectedQuotation.Project.ProjectMaterials.Add(new ProjectMaterial()
                            {
                                ProjectMaterialId = new Guid(),
                                MaterialId = batchDetail.MaterialId,
                                ProjectId = _selectedProject.ProjectID,
                                Quantity = Convert.ToInt32(remainingQuantity)
                            });
                            //_db.SaveChanges();
                            remainingQuantity = 0;
                        }
                        else // lo thieu quantity
                        {
                            if (quantityToUpdate > 0)
                            {
                                remainingQuantity -= quantityToUpdate;
                                _selectedQuotation.Project.ProjectMaterials.FirstOrDefault(p => p.MaterialId == batchDetail.MaterialId).Quantity = Convert.ToInt32(quantityToUpdate);
                                price += quantityToUpdate * batchDetail.Price;
                                quantityToUpdate = 0;
                                affectedBatchs.Add(batch);
                                
                                //_db.SaveChanges();
                            }
                            else
                            {
                                remainingQuantity -= batchDetail.Quantity;
                                quantityToUpdate = 0;
                                //_db.SaveChanges();
                            }

                        }
                    }
                    if (remainingQuantity == 0)
                    {
                        break;
                    }
                }
            }
            if (staff.Quotations == null)
            {
                staff.Quotations = new List<Quotation>();
            }
            staff.Quotations.Add(_selectedQuotation);
            _selectedQuotation.Batchs = affectedBatchs;
            _selectedQuotation.CompletePrice = price + (prj.LaborSalaryPerMonth * prj.MonthDuration * prj.NumOfLabors);
            _selectedQuotation.Status = 1;
            _db.Entry(_selectedQuotation).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public double PreviewPrice(Guid quotationId, List<Guid> batchIds, Project prj)
        {
            Quotation _selectedQuotation = _db.Quotations.FirstOrDefault(q => q.QuotationId == quotationId);
            Project _selectedProject = _db.Projects.FirstOrDefault(p => p.ProjectID == _selectedQuotation.ProjectId);
            List<ProjectMaterial> quoteMaterials = ProjectManagementService.Instance.GetProjectMaterialByProjectId(_selectedQuotation.ProjectId).ToList();
            List<Guid> remainingBatchIds = SortBatchsIdByDate(batchIds);
            List<Batch> affectedBatchs = new List<Batch>();
            double price = 0;
            foreach (var quoteMaterial in quoteMaterials)
            {
                double remainingQuantity = quoteMaterial.Quantity;
                //double materialBatchQuantity = 
                foreach (Guid batchId in remainingBatchIds)
                {
                    Batch batch = _db.Batches.FirstOrDefault(q => q.BatchId == batchId);
                    List<BatchDetail> batchDetails = GetBatchDetailsByBatchId(batch.BatchId);
                    BatchDetail batchDetail = batchDetails.FirstOrDefault(bd => bd.MaterialId == quoteMaterial.MaterialId);
                    if (batchDetail != null)
                    {
                        double quantityToUpdate = batchDetail.Quantity;
                        if (quantityToUpdate >= remainingQuantity)
                        {
                            quantityToUpdate -= remainingQuantity;
                            affectedBatchs.Add(batch);
                            price += remainingQuantity * batchDetail.Price;
                            //_db.SaveChanges();
                            remainingQuantity = 0;
                        }
                        else
                        {
                            if (quantityToUpdate > 0)
                            {
                                remainingQuantity -= quantityToUpdate;
                                price += quantityToUpdate * batchDetail.Price;
                                quantityToUpdate = 0;
                                affectedBatchs.Add(batch);
                                //_db.SaveChanges();
                            }
                            else
                            {
                                remainingQuantity -= batchDetail.Quantity;
                                quantityToUpdate = 0;
                                //_db.SaveChanges();
                            }

                        }
                    }
                    if (remainingQuantity == 0)
                    {
                        break;
                    }
                }
            }
            return price + (prj.LaborSalaryPerMonth * prj.MonthDuration * prj.NumOfLabors);
        }

        public void MinusQuantityInBatch(Guid quotationId)
        {
            Quotation _selectedQuotation = _db.Quotations
                                  .Include(q => q.Batchs)
                                  .FirstOrDefault(q => q.QuotationId == quotationId); ;
            List<ProjectMaterial> quoteMaterials = ProjectManagementService.Instance.GetProjectMaterialByProjectId(_selectedQuotation.ProjectId).ToList();

            List<Batch> batchesInQuote = _selectedQuotation.Batchs.OrderBy(b => b.ImportDate).ToList();
            List<Guid> remainingBatchIds = new List<Guid>();
            foreach (Batch batch in batchesInQuote)
            {
                remainingBatchIds.Add(batch.BatchId);
            }
            double price = 0;
            foreach (var quoteMaterial in quoteMaterials)
            {
                double remainingQuantity = quoteMaterial.Quantity;
                //double materialBatchQuantity = 
                foreach (Guid batchId in remainingBatchIds)
                {
                    Batch batch = _db.Batches.FirstOrDefault(q => q.BatchId == batchId);
                    List<BatchDetail> batchDetails = GetBatchDetailsByBatchId(batch.BatchId);
                    BatchDetail batchDetail = batchDetails.FirstOrDefault(bd => bd.MaterialId == quoteMaterial.MaterialId);
                    if (batchDetail != null)
                    {
                        if (batchDetail.Quantity >= remainingQuantity)
                        {
                            batchDetail.Quantity -= remainingQuantity;
                            price += remainingQuantity * batchDetail.Price;
                            _db.SaveChanges();
                            remainingQuantity = 0;
                        }
                        else
                        {
                            if (batchDetail.Quantity > 0)
                            {
                                remainingQuantity -= batchDetail.Quantity;
                                price += batchDetail.Quantity * batchDetail.Price;
                                batchDetail.Quantity = 0;
                                _db.SaveChanges();
                            }
                            else
                            {
                                remainingQuantity -= batchDetail.Quantity;
                                batchDetail.Quantity = 0;
                                _db.SaveChanges();
                            }
                        }
                    }
                    if (remainingQuantity == 0)
                    {
                        break;
                    }
                }
            }
            _selectedQuotation.Status = 2;
            _db.SaveChanges();
        }

        public void ClearAffectedBatches(Guid quotationId)
        {
            Quotation _selectedQuotation = _db.Quotations.Include(q => q.Batchs).FirstOrDefault(q => q.QuotationId == quotationId);
            _selectedQuotation.Batchs.Clear();
            _db.SaveChanges();
        }
        public void DeleteQuotation(Guid quotationId)
        {
            Quotation _selectedQuotation = _db.Quotations.FirstOrDefault(q => q.QuotationId == quotationId);
            ClearAffectedBatches(quotationId);
            _selectedQuotation.Status = 3;
            _db.SaveChanges();
        }
        public Staff StaffApplyQuote(String loggedEmail)
        {
            Staff staff = _db.Staffs.FirstOrDefault(c => c.Email == loggedEmail);
            return staff;
        }

        public Quotation GetQuotationWithProject(Guid id)
        {
            return _db.Quotations
                  .Include(q => q.Project).FirstOrDefault(m => m.QuotationId == id);
        }

        public BatchDetail GetDetailById(Guid id)
            => _db.BatchDetails.FirstOrDefault(bd => bd.BatchDetailId == id);

        public List<Batch> GetBatchesByQuote(Guid quoteId)
        {
            return _db.Batches.Where(b => b.Quotations.Any(q => q.QuotationId == quoteId)).Include(b => b.BatchDetails).ToList();
        }

    }
}

