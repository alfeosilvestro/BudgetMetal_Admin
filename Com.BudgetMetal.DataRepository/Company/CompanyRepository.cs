using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DataRepository.Code_Table;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.DataRepository.Company
{
    public class CompanyRepository : GenericRepository<Com.BudgetMetal.DBEntities.Company>, ICompanyRepository
    {
        public CompanyRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "CompanyRepository")
        {
        }
    }
}
