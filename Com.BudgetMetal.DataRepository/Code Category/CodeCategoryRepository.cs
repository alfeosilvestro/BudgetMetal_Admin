using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Com.BudgetMetal.DBEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Code_Category
{
    public class CodeCategoryRepository : GenericRepository<CodeCategory>, ICodeCategoryRepository
    {
        public CodeCategoryRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "CodeCategoryRepository")
        {

        }
        
    }
}
