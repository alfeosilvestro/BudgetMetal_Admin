using Com.BudgetMetal.DataRepository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.DataRepository.DocumentUser
{
    public interface IDocumentUserRepository : IGenericRepository<Com.BudgetMetal.DBEntities.DocumentUser>
    {
        void InactiveByDocumentId(int documentId, string UpdatedBy);
    }


}
