using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.DataRepository.InvitedSupplier
{
   
    public class InvitedSupplierRepository : GenericRepository<Com.BudgetMetal.DBEntities.InvitedSupplier>, IInvitedSupplierRepository
    {
        public InvitedSupplierRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "InvitedSupplierRepository")
        {

        }

    }
}
