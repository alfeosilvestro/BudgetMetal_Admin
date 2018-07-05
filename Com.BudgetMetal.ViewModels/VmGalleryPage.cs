using System;
using Com.BudgetMetal.Common;

namespace Com.BudgetMetal.ViewModels
{
    public class VmGalleryPage : ViewModelBase
    {
        public PageResult<VmGalleryItem> Result { get; set; }
    }


    public class VmGalleryDetailPage: ViewModelBase
    {
        public VmGalleryDetail Result { get; set; }
    }

    public class VmGalleryDownloadPage : ViewModelBase
    {
        public VmGalleryDownload Result { get; set; }
    }
}
