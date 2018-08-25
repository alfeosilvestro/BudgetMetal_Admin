using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.BudgetMetal.DataRepository.Requirement
{
   
    public class RequirementRepository : GenericRepository<Com.BudgetMetal.DBEntities.Requirement>, IRequirementRepository
    {
        public RequirementRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "RequirementRepository")
        {

        }

        public void InactiveByRFQId(int rfqId, string UpdatedBy)
        {
            var dbResult = this.entities.Where(e => e.IsActive == true && e.Rfq_Id == rfqId).ToList();
            dbResult.ForEach(e =>
            {
                e.IsActive = false;
                e.UpdatedDate = DateTime.Now;
                e.UpdatedBy = UpdatedBy;
            }
            );
        }
    }
}
