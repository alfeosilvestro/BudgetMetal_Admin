using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Code_Category
{
    public interface ICodeCategoryRepository : IGenericRepository<CodeCategory>
    {
        PageResult<CodeCategory> GetCodeCategoryByPage(string keyword, int page, int totalRecords);

        CodeCategory GetCodeCategoryById(int Id);

        CodeCategory GetCodeCategoryFileById(int Id);

        int GetLastId();
    }
}
