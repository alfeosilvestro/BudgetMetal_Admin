using Com.BudgetMetal.Common;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.Blogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.Blogs
{
    public interface IBlogService
    {
        Task<VmBlogPage> GetBlogsByPage(string keyword, int page, int totalRecords);

        Task<VmBlogItem> GetBlogById(int Id);

        VmGenericServiceResult Insert(VmBlogItem vmBlogItem);

        Task<VmGenericServiceResult> Update(VmBlogItem BlogItem);

        Task Delete(int I);
    }
}
