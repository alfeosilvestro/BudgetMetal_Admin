using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Com.BudgetMetal.Common;
using Microsoft.EntityFrameworkCore;

namespace Com.BudgetMetal.DataRepository.EmailLog
{
    public class EmailLogRepository : GenericRepository<Com.BudgetMetal.DBEntities.EmailLog>, IEmailsLogRepository
    {
        public EmailLogRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "EmailLogRepository")
        {

        }

        //public PageResult<Com.BudgetMetal.DBEntities.EmailLog> GetByPage(string keyword, int page, int totalRecords)
        //{
        //    if (string.IsNullOrEmpty(keyword))
        //    {
        //        keyword = string.Empty;
        //        //return await base.GetPage(keyword, page, totalRecords);
        //    }

        //    var records = entities.Where(e =>
        //        (keyword == string.Empty ||
        //        e.Message.Contains(keyword) ||
        //        e.ToEmailAddress.Contains(keyword))
        //    ).ToList();

            
        //    //DetailImage = (getDetailImage ? r.DetailImage : null),

        //    var count = records.Count();

        //    var nextPage = 0;
        //    var prePage = 0;
        //    if (page > 1)
        //    {
        //        prePage = page - 1;
        //    }

        //    var totalPage = (count + totalRecords - 1) / totalRecords;
        //    if (page < totalPage)
        //    {
        //        nextPage = page + 1;
        //    }

        //    var result = new PageResult<Com.BudgetMetal.DBEntities.EmailLog>()
        //    {
        //        Records = records,
        //        TotalPage = totalPage,
        //        CurrentPage = page,
        //        PreviousPage = prePage,
        //        NextPage = nextPage,
        //        TotalRecords = count
        //    };

        //    return result;
        //}

    }
}
