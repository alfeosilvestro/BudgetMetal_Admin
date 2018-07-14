using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Gallery;
using Com.BudgetMetal.DataRepository.Single_Sign_On;
using Com.BudgetMetal.DB.Entities;
using Com.BudgetMetal.Services.Base;
using Com.BudgetMetal.ViewModels;
using System.Collections.Generic;
using System;

namespace Com.BudgetMetal.Services.Gallery
{
    public class GalleryService : BaseService, IGalleryService
    {
        private readonly IGalleryRepository repo;

        private readonly ISingle_Sign_OnRepository repoBM;

        public GalleryService(IGalleryRepository repo, ISingle_Sign_OnRepository repoBM)
        {
            this.repo = repo;
            this.repoBM = repoBM;
        }

        public VmGalleryPage GetGalleriesByPage(string keyword, int page, int totalRecords, string applicationToken)
        {
            var dbPageResult = repo.GetGalleriesByPage(keyword,
                (page == 0 ? Constants.app_firstPage : page),
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords));

            if (dbPageResult == null)
            {
                return new VmGalleryPage();
            }

            var resultObj = new VmGalleryPage();
            resultObj.ApplicationToken = applicationToken;
            resultObj.RequestId = DateTime.Now.ToString("yyyyMMddHHmmss");
            resultObj.RequestDate = DateTime.Now;
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

        public VmGalleryDetailPage GetGalleryById(int Id, string applicationToken)
        {
            var dbPageResult = repo.GetGalleryById(Id);

            if (dbPageResult == null)
            {
                return new VmGalleryDetailPage();
            }

            var resultObj = new VmGalleryDetailPage();
            resultObj.ApplicationToken = applicationToken;
            resultObj.RequestId = DateTime.Now.ToString("yyyyMMddHHmmss");
            resultObj.RequestDate = DateTime.Now;
            var resultItem = new VmGalleryDetail();

            Copy<bm_gallery, VmGalleryDetail>(dbPageResult, resultItem);

            resultObj.Result = resultItem;

            return resultObj;
        }

        public VmGalleryDownloadPage GetGalleryFileById(int Id, string applicationToken, string authenticationToken)
        {
            var dbPageResult = repo.GetGalleryFileById(Id);

            if (dbPageResult == null)
            {
                return new VmGalleryDownloadPage();
            }

            var resultObj = new VmGalleryDownloadPage();
            resultObj.ApplicationToken = applicationToken;
            resultObj.AuthenticationToken = authenticationToken;
            resultObj.RequestId = DateTime.Now.ToString("yyyyMMddHHmmss");
            resultObj.RequestDate = DateTime.Now;
            var resultItem = new VmGalleryDownload();

            Copy<bm_gallery, VmGalleryDownload>(dbPageResult, resultItem);

            resultObj.Result = resultItem;

            return resultObj;
        }

        public bool CheckAuthentication( string authenticationToken)
        {
            var dbResult = repoBM.GetSingleSignOnByToken(authenticationToken);

            if(dbResult == null)
            {
                return false;
            }

            if(dbResult.Timeout < DateTime.Now)
            {
                return false;
            }
            else
            {
                return true;
            }
           
        }

    }
}
