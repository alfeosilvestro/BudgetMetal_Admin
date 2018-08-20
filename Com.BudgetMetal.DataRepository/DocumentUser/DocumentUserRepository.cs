using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.DataRepository.DocumentUser
{
   
    public class DocumentUserRepository : GenericRepository<Com.BudgetMetal.DBEntities.DocumentUser>, IDocumentUserRepository
    {
        public DocumentUserRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "DocumentUserRepository")
        {

        }

    }
}
