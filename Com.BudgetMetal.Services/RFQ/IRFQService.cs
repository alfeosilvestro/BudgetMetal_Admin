using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.Company;
using Com.BudgetMetal.ViewModels.Rfq;

namespace Com.BudgetMetal.Services.RFQ
{
    public interface IRFQService
    {
        Task<VmRfqPage> GetRfqByPage(int userId, int documentOwner, int page, int totalRecords, bool isCompany, int statusId = 0, string keyword = "");

        Task<VmRfqPage> GetRfqForSupplierByPage(int supplierId, int page, int totalRecords, int statusId = 0, string keyword = "");

        Task<VmRfqPage> GetPublicRfqByPage(int page, int totalRecords, int statusId = 0, string keyword = "");

        Task<VmGenericServiceResult> SaveRFQ(VmRfqItem rfq);

        Task<VmGenericServiceResult> UpdateRFQ(VmRfqItem rfq);

        //Task<VmRfqPage> GetRfqByPage(string keyword, int page, int totalRecords);

        Task<VmRfqItem> GetRfqtById(int Id);

        VmGenericServiceResult Insert(VmRfqItem vmItem);

        Task<VmGenericServiceResult> Update(VmRfqItem vmItem);

        Task Delete(int Id);

        Task<VmRfqItem> GetFormObject();

        Task<VmRfqItem> GetSingleRfqById(int documentId);

        bool CheckRFQLimit(int companyId);

        Task<VmRfqItem> GetPublicPortalSingleRfqById(int documentId);

        Task<VmRfqPage> GetPublicRfqByCompany(int page, int companyId, int totalRecords, int statusId = 0, string keyword = "");

        Task<List<VmCompanyItem>> LoadSelectedSupplier(int rfqId);

        Task<VmGenericServiceResult> WithdrawnRfq(int documentId, int userId, string userName);

        Task<VmGenericServiceResult> ApproveRfq(int documentId, int userId, string userName);

        Task<VmGenericServiceResult> DeleteRfq(int documentId, int userId, string userName);

        Task<VmGenericServiceResult> CheckQuotationByRfqId(int rfqId, int companyId);

        Task<VmGenericServiceResult> NotRelevantRfq(int rfqId, int companyId, string UpdatedBy);

        Task<VmGenericServiceResult> AddClarification(int documentId, int userId, string userName, string clarification, int commentId);

        Task<VmGenericServiceResult> CheckPermissionForRFQ(int companyId, int C_BussinessType, int userId, int RfqId, bool companyAdmin);

        Task<VmRfqPage> GetLoadTenderNoticBoardPublicRFQ(int page);

        Task<string> ResendEmail(string email, int rfqId);

        Task<VmGenericServiceResult> AddInvitationUser(int rfqId, string name, string email, string createdBy);
    }
}
