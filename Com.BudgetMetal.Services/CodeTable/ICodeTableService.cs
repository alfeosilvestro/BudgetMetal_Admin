using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.CodeTable;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.Code_Table
{
    public interface ICodeTableService
    {
        Task<VmCodeTablePage> GetCodeTableByPage(string keyword, int page, int totalRecords);

        Task<VmCodeTableItem> GetCodeTableById(int Id);

        VmGenericServiceResult Insert(VmCodeTableItem vmCodeTableItem);

        Task<VmGenericServiceResult> Update(VmCodeTableItem codeTableItem);

        Task Delete(int Id);

        Task<VmCodeTableItem> GetFormObject();
    }
}
