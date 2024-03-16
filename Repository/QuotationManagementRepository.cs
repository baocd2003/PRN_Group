﻿using BussinessObject.Entity;
using DataAccessLayer.Service;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class QuotationManagementRepository : IQuotationManagementRepository
    {
        public Quotation AddQuotation(Quotation quotation)
        {
            return QuotationManagementService.Instance.AddQuotation(quotation);
        }

        public IEnumerable<Quotation> GetAllQuotations()
        {
            return QuotationManagementService.Instance.GetAllQuotations();
        }

        public Customer GetCustomerByEmail(string email)
        {
            return QuotationManagementService.Instance.GetCustomerByEmail(email);
        }

        public Quotation GetQuotation(Guid id)
       => QuotationManagementService.Instance.GetQuotation(id);
    }
}
