using Com.BudgetMetal.DataRepository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.InvitedSupplier
{
    public interface IInvitedSupplierRepository : IGenericRepository<Com.BudgetMetal.DBEntities.InvitedSupplier>
    {
        void InactiveByRFQId(int rfqId, string UpdatedBy);

        Task<List<Com.BudgetMetal.DBEntities.InvitedSupplier>> GetByDocumentId(int documentId);

        void NotRelevantRfq(int rfqId, int companyId, string UpdatedBy);
    }
    
}
