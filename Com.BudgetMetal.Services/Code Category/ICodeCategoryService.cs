using Com.BudgetMetal.Common;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.Code_Category;
using System;

namespace Com.BudgetMetal.Services.Code_Category
{
    public interface ICodeCategoryService
    {
        VmCodeCategoryPage GetCodeCategoryByPage(string keyword, int page, int totalRecords);

        VmCodeCategoryItem GetCodeCategoryById(int Id);

        VmGenericServiceResult Insert(VmCodeCategoryItem vmCodeCategoryItem);

        VmGenericServiceResult Update(VmCodeCategoryItem codeCategoryItem);

        void Delete(int I);
    }
}
