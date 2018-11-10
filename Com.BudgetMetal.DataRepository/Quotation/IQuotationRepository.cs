using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Quotation
{
    public interface IQuotationRepository : IGenericRepository<Com.BudgetMetal.DBEntities.Quotation>
    {
        Task<PageResult<Com.BudgetMetal.DBEntities.Quotation>> GetQuotationByPage(int userId, int companyId, int page, int totalRecords, bool isCompany, int statusId, string keyword);

        Task<PageResult<Com.BudgetMetal.DBEntities.Quotation>> GetQuotationForBuyerByPage(int userId, int buyerId, int page, int totalRecords, bool isCompany, int statusId, string keyword);

        Task<PageResult<Com.BudgetMetal.DBEntities.Quotation>> GetQuotationByRfqId(int RfqId, int page, int totalRecords, int statusId, string keyword);

        Task<List<Com.BudgetMetal.DBEntities.Quotation>> GetQuotationByRfqId(int RfqId);

        Task<Com.BudgetMetal.DBEntities.Quotation> GetSingleQuotationById(int id);

        Task<Com.BudgetMetal.DBEntities.Quotation> GetQuotationBy_RfqId_CompanyId(int rfqId, int companyId);

        int GetRfqOwnerId(int documentId);

        Task<int> GetQuotationByDocumentId(int documentId);

    }
}
