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
    public class BatchManagementService
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

        public List<BatchDetail> GetBatchDetailsByBatchId(Guid batchId) {
            List<BatchDetail> batchDetails = _db.BatchDetails.Where(bd => bd.BatchId == batchId).Include(bd => bd.Materials).ToList(); 
            return batchDetails;
        }

        //public List<BatchDetail> GetBatchDetailsByBatchIdWithMaterial(Guid batchId)
        //{
        //    List<BatchDetail> batchDetails = _db.BatchDetails.Where(bd => bd.BatchId == batchId).Include(bd => bd.MaterialId).ToList();
        //    return batchDetails;
        //}

        public void AddMoreDetailsInBatch(List<BatchDetail> batchDetails)
        {
            foreach(BatchDetail batchDetail in batchDetails)
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

        public IEnumerable<Quotation> GetRequestQuotation()
        {
            return _db.Quotations.Where(q => q.Status == 0).Include(q => q.Project).OrderByDescending(q => q.RequestDate).ToList();
        }

        public List<Batch> GetBatchesDateAsc()
        {
            return _db.Batches.OrderBy(b => b.ImportDate).ToList();
        }
        
        public void UpdateQuantityInBatch(Guid quotationId, List<Guid> batchIds)
        {
            Quotation _selectedQuotation = _db.Quotations.FirstOrDefault(q => q.QuotationId == quotationId);
            List<ProjectMaterial> quoteMaterials = ProjectManagementService.Instance.GetProjectMaterialByProjectId(_selectedQuotation.ProjectId).ToList();

            List<Guid> remainingBatchIds = new List<Guid>(batchIds);
            List<Batch> affectedBatchs = new List<Batch>();
            foreach (var quoteMaterial in quoteMaterials)
            {
                double remainingQuantity = quoteMaterial.Quantity;
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
                            affectedBatchs.Add(batch);
                            _db.SaveChanges();
                            remainingQuantity = 0;
                        }
                        else
                        {
                            if(remainingQuantity > 0)
                            {
                                remainingQuantity -= batchDetail.Quantity;
                                batchDetail.Quantity = 0;
                                affectedBatchs.Add(batch);
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
            _selectedQuotation.Batchs = affectedBatchs;
            _db.SaveChanges();
        }
    }
}
