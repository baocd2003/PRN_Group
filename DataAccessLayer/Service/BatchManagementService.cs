using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using System;
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

        
    }
}
