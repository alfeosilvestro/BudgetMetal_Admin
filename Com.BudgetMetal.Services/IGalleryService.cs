using Com.BudgetMetal.Common;
using Com.BudgetMetal.ViewModels;
using System;

namespace Com.BudgetMetal.Services
{
    public interface IGalleryService
    {
        VmGalleryPage GetGalleriesByPage(string keyword, int page, int totalRecords, bool getDetailImage);

        VmGalleryDetailPage GetGalleryById(int Id);
    }
}
