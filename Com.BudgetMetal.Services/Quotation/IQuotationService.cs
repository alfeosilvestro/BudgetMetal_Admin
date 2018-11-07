using System;
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

        Task<VmQuotationPage> GetQuotationByPage(int userId, int companyId, int page, int totalRecords, bool isCompany, int statusId = 0, string keyword = "");

        Task<VmQuotationPage> GetQuotationForBuyerByPage(int userId, int buyerId, int page, int totalRecords, bool isCompany, int statusId = 0, string keyword = "");

        Task<VmQuotationPage> GetQuotationByRfqId(int RfqId, int page, int totalRecords, int statusId = 0, string keyword = "");

        Task<VmQuotationItem> InitialLoadByRfqId(int RfqId);

        VmGenericServiceResult SaveQuotation(VmQuotationItem quotation);

        Task<VmQuotationItem> GetSingleQuotationById(int id);

        VmGenericServiceResult UpdateQuotation(VmQuotationItem quotationItem);

        bool CheckQuotationLimit(int companyId);

        Task<VmGenericServiceResult> CancelQuotation(int documentId, int userId, string userName);

        Task<VmGenericServiceResult> DecideQuotation(int documentId, int userId, string userName, bool isAccept);

        Task<VmGenericServiceResult> AddClarification(int documentId, int userId, string userName, string clarification, int commentId);

        Task<VmGenericServiceResult> CheckPermissionForQuotation(int companyId, int C_BussinessType, int userId, int RfqId, bool companyAdmin);
    }
}
