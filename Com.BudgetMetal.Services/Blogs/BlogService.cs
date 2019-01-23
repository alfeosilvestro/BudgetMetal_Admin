using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Blogs;
using Com.BudgetMetal.DataRepository.Single_Sign_On;
using Com.BudgetMetal.DBEntities;
using Com.BudgetMetal.Services.Base;
using Com.BudgetMetal.ViewModels.Blogs;
using System.Collections.Generic;
using System;
using Com.BudgetMetal.ViewModels;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.Blogs
{
    public class BlogService : BaseService, IBlogService
    {
        private readonly IBlogRepository repo;

        public BlogService(IBlogRepository repo)
        {
            this.repo = repo;
        }

        public async Task<VmBlogPage> GetBlogsByPage(string keyword, int page, int totalRecords)
        {
            var dbPageResult = await repo.GetPage(keyword,
                (page == 0 ? Constants.app_firstPage : page),
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords));

            if (dbPageResult == null)
            {
                return new VmBlogPage();
            }

            var resultObj = new VmBlogPage();
            //resultObj.ApplicationToken = applicationToken;
            resultObj.RequestId = DateTime.Now.ToString("yyyyMMddHHmmss");
            resultObj.RequestDate = DateTime.Now;
            resultObj.Result = new PageResult<VmBlogItem>();
            resultObj.Result.Records = new List<VmBlogItem>();

            Copy<PageResult<Blog>, PageResult<VmBlogItem>>(dbPageResult, resultObj.Result, new string[] { "Records" });

            foreach (var dbItem in dbPageResult.Records)
            {
                var resultItem = new VmBlogItem();

                Copy<Blog, VmBlogItem>(dbItem, resultItem);

                resultObj.Result.Records.Add(resultItem);
            }

            return resultObj;
        }

        public async Task<VmBlogItem> GetBlogById(int Id)
        {
            var dbPageResult = await repo.Get(Id);

            if (dbPageResult == null)
            {
                return new VmBlogItem();
            }

            var resultObj = new VmBlogItem();

            Copy<Blog, VmBlogItem>(dbPageResult, resultObj);

            return resultObj;
        }

        public VmGenericServiceResult Insert(VmBlogItem vmBlogItem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                Blog r = new Blog();

                Copy<VmBlogItem, Blog>(vmBlogItem, r);

                if (r.CreatedBy.IsNullOrEmpty())
                {
                    r.CreatedBy = r.UpdatedBy = "System";
                }

                repo.Add(r);

                repo.Commit();

                result.IsSuccess = true;
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Error = e;
            }

            return result;
        }

        public async Task<VmGenericServiceResult> Update(VmBlogItem vmBlogItem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                Blog r = await repo.Get(vmBlogItem.Id);

                Copy<VmBlogItem, Blog>(vmBlogItem, r);

                if (r.UpdatedBy.IsNullOrEmpty())
                {
                    r.UpdatedBy = "System";
                }

                repo.Update(r);

                repo.Commit();

                result.IsSuccess = true;
            }
            catch (Exception e) {
                result.IsSuccess = false;
                result.Error = e;
            }

            return result;
        }

        public async Task Delete(int Id)
        {
            Blog r = await repo.Get(Id);
            r.IsActive = false;
            repo.Update(r);
            repo.Commit();
        }
    }
}
