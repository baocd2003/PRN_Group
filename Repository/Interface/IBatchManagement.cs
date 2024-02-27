using BussinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IBatchManagement
    {
        public void CreateBatch(DateTime importDate, List<BatchDetail> batchDetails);
        public Batch GetLastBatch();
        public void CreateBatchDetails(BatchDetail batchDetail);
        public List<BatchDetail> GetBatchDetailsByBatchId(Guid batchId);
        public void AddMoreDetailsInBatch(List<BatchDetail> batchDetails);
    }
}
