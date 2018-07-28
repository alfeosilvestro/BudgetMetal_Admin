using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Industries;
using Com.BudgetMetal.DB.Entities;
using Com.BudgetMetal.Services.Base;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.Industries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.Services.Industries
{
    public class IndustryService : BaseService, IIndustryService
    {
        private readonly IIndustryRepository repo;

        public IndustryService(IIndustryRepository repo)
        {
            this.repo = repo;
        }

        public VmIndustryPage GetIndustriesByPage(string keyword, int page, int totalRecords)
        {
            var dbPageResult = repo.GetInsustriesByPage(keyword,
                (page == 0 ? Constants.app_firstPage : page),
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords));

            if (dbPageResult == null)
            {
                return new VmIndustryPage();
            }

            var resultObj = new VmIndustryPage();
            resultObj.RequestId = DateTime.Now.ToString("yyyyMMddHHmmss");
            resultObj.RequestDate = DateTime.Now;
            resultObj.Result = new PageResult<VmIndustryItem>();
            resultObj.Result.Records = new List<VmIndustryItem>();

            Copy<PageResult<Industry>, PageResult<VmIndustryItem>>(dbPageResult, resultObj.Result, new string[] { "Records" });

            foreach (var dbItem in dbPageResult.Records)
            {
                var resultItem = new VmIndustryItem();

                Copy<Industry, VmIndustryItem>(dbItem, resultItem);

                resultObj.Result.Records.Add(resultItem);
            }

            return resultObj;
        }

        public VmIndustryItem GetIndustryById(int Id)
        {
            var dbPageResult = repo.GetIndustryById(Id);

            if (dbPageResult == null)
            {
                return new VmIndustryItem();
            }

            var resultObj = new VmIndustryItem();

            Copy<Industry, VmIndustryItem>(dbPageResult, resultObj);

            return resultObj;
        }

        public VmGenericServiceResult Insert(VmIndustryItem vmtem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                Industry r = new Industry();

                Copy<VmIndustryItem, Industry>(vmtem, r);

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

        public VmGenericServiceResult Update(VmIndustryItem vmtem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                Industry r = repo.GetIndustryById(vmtem.Id);

                Copy<VmIndustryItem, Industry>(vmtem, r);

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

        public void Delete(int Id)
        {
            Industry r = repo.GetIndustryById(Id);
            r.IsActive = false;
            repo.Update(r);
            repo.Commit();
        }
    }
}
