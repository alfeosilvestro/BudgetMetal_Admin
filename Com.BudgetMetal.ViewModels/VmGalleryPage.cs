using System;
using Com.BudgetMetal.Common;

namespace Com.BudgetMetal.ViewModels
{
    public class VmGalleryPage : ViewModelBase
    {
        public PageResult<VmGalleryItem> Result { get; set; }
    }
}
