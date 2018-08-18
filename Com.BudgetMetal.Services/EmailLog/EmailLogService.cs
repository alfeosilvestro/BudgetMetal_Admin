using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.EmailLog;
using Com.BudgetMetal.Services.Base;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.EmailLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.EmailLog
{
    public class EmailLogService : BaseService, IEmailLogService
    {
        private readonly IEmailLogRepository repo;

        public EmailLogService(IEmailLogRepository repo)
        {
            this.repo = repo;
        }

        public async Task<VmEmailLogPage> GetEmailLogByPage(string keyword, int page, int totalRecords)
        {
            var dbPageResult = await repo.GetPage(keyword,
                (page == 0 ? Constants.app_firstPage : page),
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords));

            if (dbPageResult == null)
            {
                return new VmEmailLogPage();
            }

            var resultObj = new VmEmailLogPage();
            
            resultObj.Result = new PageResult<VmEmailLogItem>();
            resultObj.Result.Records = new List<VmEmailLogItem>();

            Copy<PageResult<Com.BudgetMetal.DBEntities.EmailLog>, PageResult<VmEmailLogItem>>(dbPageResult, resultObj.Result, new string[] { "Records" });

            foreach (var dbItem in dbPageResult.Records)
            {
                var resultItem = new VmEmailLogItem();

                Copy<Com.BudgetMetal.DBEntities.EmailLog, VmEmailLogItem>(dbItem, resultItem);

                resultObj.Result.Records.Add(resultItem);
            }

            return resultObj;
        }

        public async Task<VmEmailLogItem> GetEmailLogById(int Id)
        {
            var dbPageResult = await repo.Get(Id);

            if (dbPageResult == null)
            {
                return new VmEmailLogItem();
            }

            var resultObj = new VmEmailLogItem();

            Copy<Com.BudgetMetal.DBEntities.EmailLog, VmEmailLogItem>(dbPageResult, resultObj);

            return resultObj;
        }

        public VmGenericServiceResult Insert(VmEmailLogItem vmtem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                Com.BudgetMetal.DBEntities.EmailLog r = new Com.BudgetMetal.DBEntities.EmailLog();

                Copy<VmEmailLogItem, Com.BudgetMetal.DBEntities.EmailLog>(vmtem, r);

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

        public async Task<VmGenericServiceResult> Update(VmEmailLogItem vmtem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                Com.BudgetMetal.DBEntities.EmailLog r = await repo.Get(vmtem.Id);

                Copy<VmEmailLogItem, Com.BudgetMetal.DBEntities.EmailLog>(vmtem, r);

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
            Com.BudgetMetal.DBEntities.EmailLog r = await repo.Get(Id);
            r.IsActive = false;
            repo.Update(r);
            repo.Commit();
        }
    }
}
