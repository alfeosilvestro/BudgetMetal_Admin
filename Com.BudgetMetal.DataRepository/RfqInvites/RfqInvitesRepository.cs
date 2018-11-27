using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Com.BudgetMetal.DataRepository.RfqInvites
{
    public class RfqInvitesRepository : GenericRepository<Com.BudgetMetal.DBEntities.RfqInvites>, IRfqInvitesRepository
    {
        public RfqInvitesRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "RfqInvitesRepository")
        {

        }

        public void InactiveByRFQId(int rfqId, string UpdatedBy)
        {
            var dbResult = this.entities.Where(e => e.IsActive == true && e.RfqId == rfqId).ToList();
            dbResult.ForEach(e =>
            {
                e.IsActive = false;
                e.Status = "";
                e.UpdatedDate = DateTime.Now;
                e.UpdatedBy = UpdatedBy;
            }
            );
        }

        public async Task<Com.BudgetMetal.DBEntities.RfqInvites> GetRfqInvitesWithEmailandAccessCode(string email, string accessCode)
        {
            var result = await this.entities.Where(e => e.EmailAddress == email && e.AccessCode==accessCode && e.IsActive == true).FirstOrDefaultAsync();

            return result;
        }

    }
}
