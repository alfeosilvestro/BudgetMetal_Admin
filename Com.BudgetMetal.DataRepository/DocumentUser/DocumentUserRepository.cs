using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.BudgetMetal.DataRepository.DocumentUser
{
   
    public class DocumentUserRepository : GenericRepository<Com.BudgetMetal.DBEntities.DocumentUser>, IDocumentUserRepository
    {
        public DocumentUserRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "DocumentUserRepository")
        {

        }

        public void InactiveByDocumentId(int documentId,string UpdatedBy)
        {
            var dbResult = this.entities.Where(e=>e.IsActive == true && e.Document_Id == documentId).ToList();
            dbResult.ForEach(e=>
            {
                e.IsActive = false;
                e.UpdatedDate = DateTime.Now;
                e.UpdatedBy = UpdatedBy;
            }
            );
        }

    }
}
