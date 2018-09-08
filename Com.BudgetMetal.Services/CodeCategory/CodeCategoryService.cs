using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Code_Category;
using Com.BudgetMetal.DBEntities;
using Com.BudgetMetal.Services.Base;
using Com.BudgetMetal.ViewModels.CodeCategory;
using System.Collections.Generic;
using System;
using Com.BudgetMetal.ViewModels;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.Code_Category
{
    public class CodeCategoryService : BaseService, ICodeCategoryService
    {
        private readonly ICodeCategoryRepository repo;

        public CodeCategoryService(ICodeCategoryRepository repo)
        {
            this.repo = repo;
        }

        public async Task<VmCodeCategoryPage> GetCodeCategoryByPage(string keyword, int page, int totalRecords)
        {
            var dbPageResult = await repo.GetPage(keyword,
                (page == 0 ? Constants.app_firstPage : page),
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords));

            if (dbPageResult == null)
            {
                return new VmCodeCategoryPage();
            }

            var resultObj = new VmCodeCategoryPage();
            resultObj.RequestId = DateTime.Now.ToString("yyyyMMddHHmmss");
            resultObj.RequestDate = DateTime.Now;
            resultObj.Result = new PageResult<VmCodeCategoryItem>();
            resultObj.Result.Records = new List<VmCodeCategoryItem>();

            Copy<PageResult<CodeCategory>, PageResult<VmCodeCategoryItem>>(dbPageResult, resultObj.Result, new string[] { "Records" });
            
            foreach (var dbItem in dbPageResult.Records)
            {
                var resultItem = new VmCodeCategoryItem();

                Copy<CodeCategory, VmCodeCategoryItem>(dbItem, resultItem);

                resultObj.Result.Records.Add(resultItem);
            }

            return resultObj;
        }

        public async Task<VmCodeCategoryItem> GetCodeCategoryById(int Id)
        {
            var dbPageResult = await repo.Get(Id);

            if (dbPageResult == null)
            {
                return new VmCodeCategoryItem();
            }

            var resultObj = new VmCodeCategoryItem();

            Copy<CodeCategory, VmCodeCategoryItem>(dbPageResult, resultObj);

            return resultObj;
        }

        public VmGenericServiceResult Insert(VmCodeCategoryItem vmCodeCategoryItem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                CodeCategory r = new CodeCategory();

                Copy<VmCodeCategoryItem, CodeCategory>(vmCodeCategoryItem, r);

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
                var repoEntity = repo.Get(vmCodeCategoryItem.Id);
                if (repoEntity != null)
                {
                    Exception ee = new Exception("Id is already existed!");

                    result.Error = ee;
                }
                else
                {
                    result.Error = e;
                }
            }

            return result;
        }

        public async Task<VmGenericServiceResult> Update(VmCodeCategoryItem vmCodeCategoryItem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                CodeCategory r = await repo.Get(vmCodeCategoryItem.Id);

                Copy<VmCodeCategoryItem, CodeCategory>(vmCodeCategoryItem, r);

                if (r.UpdatedBy.IsNullOrEmpty())
                {
                    r.UpdatedBy = "System";
                }

                repo.Update(r);

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

        public async Task Delete(int Id)
        {
            CodeCategory r = await repo.Get(Id);
            r.IsActive = false;
            repo.Update(r);
            repo.Commit();
        }
    }
}
