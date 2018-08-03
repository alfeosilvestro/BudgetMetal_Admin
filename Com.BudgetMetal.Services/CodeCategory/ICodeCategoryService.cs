using Com.BudgetMetal.Common;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.CodeCategory;
using System;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.Code_Category
{
    public interface ICodeCategoryService
    {
        Task<VmCodeCategoryPage> GetCodeCategoryByPage(string keyword, int page, int totalRecords);

        Task<VmCodeCategoryItem> GetCodeCategoryById(int Id);

        VmGenericServiceResult Insert(VmCodeCategoryItem vmCodeCategoryItem);

        Task<VmGenericServiceResult> Update(VmCodeCategoryItem codeCategoryItem);

        Task Delete(int Id);
    }
}
