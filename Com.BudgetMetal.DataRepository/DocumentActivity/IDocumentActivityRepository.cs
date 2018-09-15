using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.DocumentActivity
{
    public interface IDocumentActivityRepository : IGenericRepository<Com.BudgetMetal.DBEntities.DocumentActivity>
    {
       Task<PageResult<Com.BudgetMetal.DBEntities.DocumentActivity>> GetDocumentActivityWithDocumentId(int DocumentId, bool IsRfq);
    }
}
