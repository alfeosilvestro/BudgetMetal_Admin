using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.RFQ
{
    public interface IRfqRepository : IGenericRepository<Com.BudgetMetal.DBEntities.Rfq>
    {
        Task<PageResult<Com.BudgetMetal.DBEntities.Rfq>> GetRfqByPage(int userId, int documentOwner, int page, int totalRecords, bool isCompany, int statusId, string keyword);

        Task<PageResult<Com.BudgetMetal.DBEntities.Rfq>> GetRfqByPageForDashboard(int userId, int documentOwner, int page, int totalRecords, bool isCompany, int statusId, string keyword);

        Task<PageResult<Com.BudgetMetal.DBEntities.Rfq>> GetRfqForSupplierByPage(int supplierId, int page, int totalRecords, int statusId, string keyword);

        Task<PageResult<Com.BudgetMetal.DBEntities.Rfq>> GetPublicRfqByPage(int page, int totalRecords, int statusId, string keyword);


        Task<List<Com.BudgetMetal.DBEntities.Rfq>> GetAllOpenRFQ();


        Task<Com.BudgetMetal.DBEntities.Rfq> GetSingleRfqById(int id); 

        //Task<PageResult<Com.BudgetMetal.DBEntities.Rfq>> GetPublicRfqByCompany(int page, int companyId, int totalRecords, int statusId, string keyword);

        Task<List<Com.BudgetMetal.DBEntities.Company>> GetSelectedSupplier(int rfqId);

        Task<int> GetSingleRfqByDocumentId(int documentId);

        Task<Com.BudgetMetal.DBEntities.Rfq> GetRfqByQuotation_DocumentId(int documentId);

        Task<PageResult<Com.BudgetMetal.DBEntities.Rfq>> GetLoadTenderNoticBoardPublicRFQ(int count);
    }
}
