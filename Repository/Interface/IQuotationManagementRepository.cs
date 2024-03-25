
﻿using BussinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IQuotationManagementRepository
    {
        IEnumerable<Quotation> GetAllQuotations();
        Quotation AddQuotation (Quotation quotation);
        Quotation GetQuotation(Guid id);
        List<Quotation> GetProcessingQuotes();
        List<Quotation> GetAppliedQuotes();
        Customer GetCustomerByEmail(string email);
        Quotation FindQuotationById(Guid id);
        void UpdateNote(Quotation quotation);
    }
}
