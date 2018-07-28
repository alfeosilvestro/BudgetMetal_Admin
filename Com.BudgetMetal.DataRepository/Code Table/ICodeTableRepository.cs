using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DBEntities;

namespace Com.BudgetMetal.DataRepository.Code_Table
{
    public interface ICodeTableRepository : IGenericRepository<CodeTable>
    {
        PageResult<CodeTable> GetCodeTableByPage(string keyword, int page, int totalRecords);

        CodeTable GetCodeTableById(int Id);

        CodeTable GetCodeTableFileById(int Id);

        int GetLastId();
    }
}
