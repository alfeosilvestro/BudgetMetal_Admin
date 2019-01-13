using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<Com.BudgetMetal.DBEntities.InvitedSupplier>> GetByDocumentId(int documentId)
        {
            var result = await this.entities.Include(e => e.Rfq).Where(e => e.Rfq.Document_Id == documentId && e.IsActive == true).ToListAsync();

            return result;
        }

        public void NotRelevantRfq(int rfqId, int companyId, string UpdatedBy)
        {
            var dbResult = this.entities.Where(e => e.IsActive == true && e.Rfq_Id == rfqId && e.Company_Id == companyId).ToList();
            dbResult.ForEach(e =>
            {
                e.NotRelevant = true;
                e.UpdatedDate = DateTime.Now;
                e.UpdatedBy = UpdatedBy;
            }
            );
            this.Commit();
        }
        
    }
}
