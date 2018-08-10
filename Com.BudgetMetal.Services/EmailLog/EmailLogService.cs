using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.EmailLog;
using Com.BudgetMetal.Services.Base;
using Com.BudgetMetal.ViewModels.EmailLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.EmailLog
{
    public class EmailLogService : BaseService, IEmailLogService
    {
        private readonly IEmailsLogRepository repo;
        private readonly EmailLogRepository repos;
        public EmailLogService(IEmailsLogRepository repo, EmailLogRepository repos)
        {
            this.repo = repo;
            this.repos = repos;
        }

        public async Task<VmEmailLogPage> GetEmailLogByPage(string keyword, int page, int totalRecords)
        {
            var dbPageResult = await repo.GetPage(keyword,
                (page == 0 ? Constants.app_firstPage : page),
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords));

            //var dbPageResult = await repo.GetByPage(keyword,
            //    (page == 0 ? Constants.app_firstPage : page),
            //    (totalRecords == 0 ? Constants.app_totalRecords : totalRecords));

            if (dbPageResult == null)
            {
                return new VmEmailLogPage();
            }

            var resultObj = new VmEmailLogPage();
            //resultObj.ApplicationToken = applicationToken;
            
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

        //public async Task<VmEmailLogItem> GetEmailLogById(int Id)
        //{
        //    var dbPageResult = await repo.Get(Id);

        //    if (dbPageResult == null)
        //    {
        //        return new VmEmailLogItem();
        //    }

        //    var resultObj = new VmEmailLogItem();

        //    Copy<Com.BudgetMetal.DBEntities.EmailLog, VmEmailLogItem>(dbPageResult, resultObj);

        //    return resultObj;
        //}
    }
}
