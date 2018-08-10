using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Industries;
using Com.BudgetMetal.DataRepository.ServiceTags;
using Com.BudgetMetal.Services.Base;
using Com.BudgetMetal.Services.Industries;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.Industries;
using Com.BudgetMetal.ViewModels.ServiceTags;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.ServiceTags
{
    public class ServiceTagsService : BaseService, IServiceTagsService
    {
        private readonly IServiceTagsRepository repo;
        private readonly IIndustryRepository industryRepo;

        public ServiceTagsService(IServiceTagsRepository repo, IIndustryRepository inRepo)
        {
            this.repo = repo;
            this.industryRepo = inRepo;
        }

        public async Task<VmServiceTagsPage> GetServiceTagsByPage(string keyword, int page, int totalRecords)

        {
            var dbPageResult = await repo.GetPage(keyword,
                (page == 0 ? Constants.app_firstPage : page),
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords));

            if (dbPageResult == null)
            {
                return new VmServiceTagsPage();
            }

            var resultObj = new VmServiceTagsPage();
            resultObj.RequestId = DateTime.Now.ToString("yyyyMMddHHmmss");
            resultObj.RequestDate = DateTime.Now;
            resultObj.Result = new PageResult<VmServiceTagsItem>();
            resultObj.Result.Records = new List<VmServiceTagsItem>();

            Copy<PageResult<Com.BudgetMetal.DBEntities.ServiceTags>, PageResult<VmServiceTagsItem>>(dbPageResult, resultObj.Result, new string[] { "Records" });

            foreach (var dbItem in dbPageResult.Records)
            {
                var resultItem = new VmServiceTagsItem();

                Copy<Com.BudgetMetal.DBEntities.ServiceTags, VmServiceTagsItem>(dbItem, resultItem);

                if (dbItem.Industry != null)
                {
                    resultItem.Industry = new ViewModels.Industries.VmIndustryItem()
                    {
                        Name = dbItem.Industry.Name
                    };
                }

                resultObj.Result.Records.Add(resultItem);
            }

            return resultObj;
        }

        public async Task<VmServiceTagsItem> GetServiceTagsById(int Id)
        {
            var dbPageResult = await repo.Get(Id);

            if (dbPageResult == null)
            {
                return new VmServiceTagsItem();
            }

            

            var resultObj = new VmServiceTagsItem();

            Copy<Com.BudgetMetal.DBEntities.ServiceTags, VmServiceTagsItem>(dbPageResult, resultObj);

            var dbIndustryList = await industryRepo.GetAll();

            if (dbIndustryList == null) return resultObj;

            resultObj.IndustryList = new List<VmIndustryItem>();

            foreach (var dbcat in dbIndustryList)
            {
                VmIndustryItem cat = new VmIndustryItem()
                {
                    Id = dbcat.Id,
                    Name = dbcat.Name
                };

                resultObj.IndustryList.Add(cat);
            }

            return resultObj;
        }

        public VmGenericServiceResult Insert(VmServiceTagsItem vmtem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                Com.BudgetMetal.DBEntities.ServiceTags r = new Com.BudgetMetal.DBEntities.ServiceTags();

                Copy<VmServiceTagsItem, Com.BudgetMetal.DBEntities.ServiceTags>(vmtem, r);

                if (r.CreatedBy.IsNullOrEmpty())
                {
                    r.CreatedBy = r.UpdatedBy = "System";
                }
               // r.Industry_Id = 1;//hard code
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

        public async Task<VmGenericServiceResult> Update(VmServiceTagsItem vmtem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                Com.BudgetMetal.DBEntities.ServiceTags r = await repo.Get(vmtem.Id);

                Copy<VmServiceTagsItem, Com.BudgetMetal.DBEntities.ServiceTags>(vmtem, r);

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
            Com.BudgetMetal.DBEntities.ServiceTags r = await repo.Get(Id);
            r.IsActive = false;
            repo.Update(r);
            repo.Commit();
        }

        public async Task<VmServiceTagsItem> GetFormObject()
        {
            VmServiceTagsItem result = new VmServiceTagsItem();

            var dbCatList = await industryRepo.GetAll();

            if (dbCatList == null) return result;

            result.IndustryList = new List<VmIndustryItem>();

            foreach (var dbcat in dbCatList)
            {
                VmIndustryItem _industry = new VmIndustryItem()
                {
                    Id = dbcat.Id,
                    Name = dbcat.Name
                };

                result.IndustryList.Add(_industry);
            }

            return result;
        }

        public async Task<List<VmServiceTagsItem>> GetVmServiceTagsByIndustry(int Id)
        {
            var dbResult = await repo.GetServiceTagsByIndustry(Id);

            var resultList = new List<VmServiceTagsItem>();
            foreach (var item in dbResult)
            {
                var resultItem = new VmServiceTagsItem();

                Copy<Com.BudgetMetal.DBEntities.ServiceTags, VmServiceTagsItem>(item, resultItem);

                resultList.Add(resultItem);
            }

            return resultList;
        }
    }
}
