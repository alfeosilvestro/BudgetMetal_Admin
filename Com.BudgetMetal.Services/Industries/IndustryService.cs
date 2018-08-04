using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Industries;
using Com.BudgetMetal.DBEntities;
using Com.BudgetMetal.Services.Base;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.Industries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.Industries
{
    public class IndustryService : BaseService, IIndustryService
    {
        private readonly IIndustryRepository repo;

        public IndustryService(IIndustryRepository repo)
        {
            this.repo = repo;
        }

        public async Task<VmIndustryPage> GetIndustriesByPage(string keyword, int page, int totalRecords)
        {
            var dbPageResult = await repo.GetPage(keyword,
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

        public async Task<VmIndustryItem> GetIndustryById(int Id)
        {
            var dbPageResult = await repo.Get(Id);

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

        public async Task<VmGenericServiceResult> Update(VmIndustryItem vmtem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                Industry r = await repo.Get(vmtem.Id);

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

        public async Task Delete(int Id)
        {
            Industry r = await repo.Get(Id);
            r.IsActive = false;
            repo.Update(r);
            repo.Commit();
        }

        public List<VmIndustryItem> GetActiveIndustries()
        {
            var dbResult =  repo.GetAll();

            var resultList = new List<VmIndustryItem>();
            foreach(var item in dbResult)
            {
                var resultItem = new VmIndustryItem();

                Copy<Industry, VmIndustryItem>(item, resultItem);

                resultList.Add(resultItem);
            }

            return resultList;
        }
    }
}
