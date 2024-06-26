﻿using BussinessObject.Entity;
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
        public Batch GetBatchById(Guid batchId);
        public void UpdateBatchDetail(BatchDetail batchDetail);
        public IEnumerable<Quotation> GetRequestQuotation();
        public List<Batch> GetBatchesDateAsc();
        public void UpdateQuantityInBatch(Guid quotationId, List<Guid> batchIds, Guid staffId,Project prj);
        public void MinusQuantityInBatch(Guid quotationId);
        public void ClearAffectedBatches(Guid quotationId);
        public void DeleteQuotation(Guid quotationId);
        public bool CheckOverlapBatch(Batch batch);
        public bool CheckAvailableBatchForQuote(Guid quotationId, List<Guid> batchIds);
        public Staff StaffApplyQuote(String loggedEmail);

        public double PreviewPrice(Guid quotationId, List<Guid> batchIds, Project prj);
        public Quotation GetQuotationWithProject(Guid id);
        public List<Batch> CheckAvailableQuantityBatch();
        public BatchDetail GetDetailById(Guid id);
    }
}
