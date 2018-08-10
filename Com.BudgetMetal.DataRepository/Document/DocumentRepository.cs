using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.DataRepository.Document
{
   
    public class DocumentRepository : GenericRepository<Com.BudgetMetal.DBEntities.Document>, IDocumentRepository
    {
        public DocumentRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "DocumentRepository")
        {

        }

    }
}
