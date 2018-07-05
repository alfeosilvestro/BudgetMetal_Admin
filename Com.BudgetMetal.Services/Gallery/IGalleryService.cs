using Com.BudgetMetal.Common;
using Com.BudgetMetal.ViewModels;
using System;

namespace Com.BudgetMetal.Services.Gallery
{
    public interface IGalleryService
    {
        VmGalleryPage GetGalleriesByPage(string keyword, int page, int totalRecords,string applicationToken);

        VmGalleryDetailPage GetGalleryById(int Id, string applicationToken);

        VmGalleryDownloadPage GetGalleryFileById(int Id, string applicationToken, string authenticationToken);

        bool CheckAuthentication(string authenticationToken);
    }
}
