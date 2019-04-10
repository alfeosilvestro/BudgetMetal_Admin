using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Com.BudgetMetal.DBEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.EmailTemplates
{
    public class EmailTemplateRepository : GenericRepository<EmailTemplate>, IEmailTemplateRepository
    {
        public EmailTemplateRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "EmailTemplateRepository")
        {

        }

        public async Task<EmailTemplate> GetEmailTemplateByPurpose(string purpose)
        {
            var record = await this.entities.Where(e => e.IsActive == true 
                                                && e.Purpose.ToLower() == purpose.ToLower())
                                                .FirstOrDefaultAsync();

            return record; 
        }
    }
}
