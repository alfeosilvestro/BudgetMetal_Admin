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
        Task<PageResult<Com.BudgetMetal.DBEntities.Quotation>> GetQuotationByPage(int documentOwner, int page, int totalRecords);

        Task<PageResult<Com.BudgetMetal.DBEntities.Quotation>> GetQuotationByRfqId(int RfqId, int page, int totalRecords, int statusId, string keyword);

        Task<List<Com.BudgetMetal.DBEntities.Quotation>> GetQuotationByRfqId(int RfqId);

        Task<Com.BudgetMetal.DBEntities.Quotation> GetSingleQuotationById(int id);
    }
}
