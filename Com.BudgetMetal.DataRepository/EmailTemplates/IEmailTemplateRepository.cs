using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.EmailTemplates
{
    public interface IEmailTemplateRepository : IGenericRepository<EmailTemplate>
    {
        Task<EmailTemplate> GetEmailTemplateByPurpose(string purpose);
    }
}
