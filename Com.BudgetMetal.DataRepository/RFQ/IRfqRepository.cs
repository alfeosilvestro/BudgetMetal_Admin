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
        Task<PageResult<Com.BudgetMetal.DBEntities.Rfq>> GetRfqByPage(int documentOwner, int page, int totalRecords, int statusId, string keyword);

        Task<PageResult<Com.BudgetMetal.DBEntities.Rfq>> GetPublicRfqByPage(int page, int totalRecords, int statusId, string keyword);

        Task<Com.BudgetMetal.DBEntities.Rfq> GetSingleRfqById(int id);
    }
    
}
