using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.Code_Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.Services.Code_Table
{
    public interface ICodeTableService
    {
        VmCodeTablePage GetCodeTableByPage(string keyword, int page, int totalRecords);

        VmCodeTableItem GetCodeTableById(int Id);

        VmGenericServiceResult Insert(VmCodeTableItem vmCodeTableItem);

        VmGenericServiceResult Update(VmCodeTableItem codeTableItem);

        void Delete(int Id);
    }
}
