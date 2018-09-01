using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.BudgetMetal.DataRepository.InvitedSupplier
{
   
    public class InvitedSupplierRepository : GenericRepository<Com.BudgetMetal.DBEntities.InvitedSupplier>, IInvitedSupplierRepository
    {
        public InvitedSupplierRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "InvitedSupplierRepository")
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
