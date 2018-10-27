using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.TimeLine;
using Com.BudgetMetal.Services.Base;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.TimeLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.TimeLine
{
    public class TimeLineService : BaseService, ITimeLineService
    {
        private readonly ITimeLineRepository repo;

        public TimeLineService(ITimeLineRepository repo)
        {
            this.repo = repo;
        }

        public async Task<VmTimeLinePage> GetTimeLineData(int page, int totalRecords, int statusId = 0, string keyword = "")
        {
            var dbPageResult = await repo.GetTimeLineData((page == 0 ? Constants.app_firstPage : page),
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords), statusId, keyword);


            if (dbPageResult == null)
            {
                return new VmTimeLinePage();
            }

            var resultObj = new VmTimeLinePage();
            resultObj.RequestId = DateTime.Now.ToString("yyyyMMddHHmmss");
            resultObj.RequestDate = DateTime.Now;
            resultObj.Result = new PageResult<VmTimeLineGroupByDate>();
            resultObj.Result.Records = new List<VmTimeLineGroupByDate>();

            Copy<PageResult<Com.BudgetMetal.DBEntities.TimeLine>, PageResult<VmTimeLineGroupByDate>>(dbPageResult, resultObj.Result, new string[] { "Records" });

            var dateList = dbPageResult.Records.Select(x => x.CreatedDate.Date).Distinct().ToList();

            foreach (var item in dateList)
            {
                var resultItem = new VmTimeLineGroupByDate();
                resultItem.GroupDate = item.Date;
                resultItem.StrGroupDate = item.ToString("dd MMM. yyyy");
                resultItem.Records = new List<VmTimeLineItemForPage>();
                foreach (var dbItem in dbPageResult.Records.Where(x=>x.CreatedDate.Date == item.Date))
                {
                    var detailItem = new VmTimeLineItemForPage();
                    detailItem.Message = dbItem.Message;
                    detailItem.UserName = dbItem.User.ContactName;
                    detailItem.DocumentNo = dbItem.Document.DocumentNo;
                    // Rfq and Quotation ID will get depend on Url is created depended on Message Type
                    detailItem.DocumentUrl = "";
                    detailItem.Time = dbItem.CreatedDate.ToString("HH:mm");
                    detailItem.IsRead = dbItem.IsRead;
                    detailItem.MessageType = dbItem.MessageType;
                    resultItem.Records.Add(detailItem);
                }

                resultObj.Result.Records.Add(resultItem);

            }
            
            return resultObj;
        }

    }
}
