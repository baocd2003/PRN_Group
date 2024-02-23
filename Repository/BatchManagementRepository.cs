using BussinessObject.Entity;
using DataAccessLayer.Service;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class BatchManagementRepository : IBatchManagement
    {

        public void CreateBatch(DateTime importDate, List<BatchDetail> batchDetails)
       => BatchManagementService.Instance.CreateBatch(importDate,batchDetails);

        public void CreateBatchDetails(BatchDetail batchDetail)
        =>BatchManagementService.Instance.CreateBatchDetails(batchDetail);

        public Batch GetLastBatch()
        =>BatchManagementService.Instance.GetLastBatch();
    }
}
