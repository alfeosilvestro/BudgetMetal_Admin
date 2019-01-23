using System;
using Com.BudgetMetal.Common;

namespace Com.BudgetMetal.ViewModels.Blogs
{
    public class VmBlogPage : ViewModelBase
    {
        public PageResult<VmBlogItem> Result { get; set; }
    }
}
