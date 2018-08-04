using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Company;
using Com.BudgetMetal.Services.Base;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.Company;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.Company
{
    public class CompanyService : BaseService, ICompanyService
    {
        private readonly ICompanyRepository repo;

        public CompanyService(ICompanyRepository repo)
        {
            this.repo = repo;
        }

        public async Task<VmCompanyPage> GetCompanyByPage(string keyword, int page, int totalRecords)
        {
            var dbPageResult = await repo.GetPage(keyword,
                (page == 0 ? Constants.app_firstPage : page),
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords));

            if (dbPageResult == null)
            {
                return new VmCompanyPage();
            }

            var resultObj = new VmCompanyPage();
            resultObj.RequestId = DateTime.Now.ToString("yyyyMMddHHmmss");
            resultObj.RequestDate = DateTime.Now;
            resultObj.Result = new PageResult<VmCompanyItem>();
            resultObj.Result.Records = new List<VmCompanyItem>();

            Copy<PageResult<Com.BudgetMetal.DBEntities.Company>, PageResult<VmCompanyItem>>(dbPageResult, resultObj.Result, new string[] { "Records" });

            foreach (var dbItem in dbPageResult.Records)
            {
                var resultItem = new VmCompanyItem();

                Copy<Com.BudgetMetal.DBEntities.Company, VmCompanyItem>(dbItem, resultItem);

                resultObj.Result.Records.Add(resultItem);
            }

            return resultObj;
        }

        public async Task<VmCompanyItem> GetCompanyById(int Id)
        {
            var dbPageResult = await repo.Get(Id);

            if (dbPageResult == null)
            {
                return new VmCompanyItem();
            }

            var resultObj = new VmCompanyItem();

            Copy<Com.BudgetMetal.DBEntities.Company, VmCompanyItem>(dbPageResult, resultObj);

            return resultObj;
        }

        public VmGenericServiceResult Insert(VmCompanyItem vmItem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                Com.BudgetMetal.DBEntities.Company r = new Com.BudgetMetal.DBEntities.Company();

                Copy<VmCompanyItem, Com.BudgetMetal.DBEntities.Company>(vmItem, r);

                //r.Id = 1;// repo.GetLastId();
                r.AwardedQuotation = 0;
                r.SubmittedQuotation = 0;
                r.BuyerAvgRating = 0;
                r.SupplierAvgRating = 0;

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

        public async Task<VmGenericServiceResult> Update(VmCompanyItem vmCodeTableItem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                Com.BudgetMetal.DBEntities.Company r = await repo.Get(vmCodeTableItem.Id);

                Copy<VmCompanyItem, Com.BudgetMetal.DBEntities.Company>(vmCodeTableItem, r);

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
            Com.BudgetMetal.DBEntities.Company r = await repo.Get(Id);
            r.IsActive = false;
            repo.Update(r);
            repo.Commit();
        }
    }
}
