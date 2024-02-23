using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
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
    }
}
