﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.Quotation;
using Com.BudgetMetal.ViewModels.Rfq;

namespace Com.BudgetMetal.Services.Quotation
{
    public interface IQuotationService
    {

        Task<VmQuotationPage> GetQuotationByPage(int documentOwner, int page, int totalRecords);

        Task<VmQuotationItem> InitialLoadByRfqId(int RfqId);

        string SaveQuotation(VmQuotationItem quotation);

        Task<VmQuotationItem> GetSingleQuotationById(int id);

        string UpdateQuotation(VmQuotationItem quotationItem);
    }
}