using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Gallery;
using Com.BudgetMetal.DB.Entities;
using Com.BudgetMetal.Services.Base;
using Com.BudgetMetal.ViewModels;
using System.Collections.Generic;
using System;

namespace Com.BudgetMetal.Services
{
    public class GalleryService : BaseService, IGalleryService
    {
        private readonly IGalleryRepository repo;

        public GalleryService(IGalleryRepository repo)
        {
            this.repo = repo;
        }

        public VmGalleryPage GetGalleriesByPage(string keyword, int page, int totalRecords, bool getDetailImage)
        {
            var dbPageResult = repo.GetGalleriesByPage(keyword, 
                (page == 0 ? Constants.app_firstPage : page),
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords),
                getDetailImage);

            if (dbPageResult == null)
            {
                return new VmGalleryPage();
            }

            var resultObj = new VmGalleryPage();
            resultObj.Result = new PageResult<VmGalleryItem>();
            resultObj.Result.Records = new List<VmGalleryItem>();

            Copy<PageResult<bm_gallery>, PageResult<VmGalleryItem>>(dbPageResult, resultObj.Result, new string[] { "Records" });

            foreach (var dbItem in dbPageResult.Records)
            {
                var resultItem = new VmGalleryItem();

                Copy<bm_gallery, VmGalleryItem>(dbItem, resultItem);

                resultObj.Result.Records.Add(resultItem);
            }

            return resultObj;
        }
    }
}
