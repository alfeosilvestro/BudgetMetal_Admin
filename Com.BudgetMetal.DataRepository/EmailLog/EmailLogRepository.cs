using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.EmailLog
{
    public class EmailLogRepository : GenericRepository<Com.BudgetMetal.DBEntities.EmailLog>, IEmailLogRepository
    {
        public EmailLogRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "EmailLogRepository")
        {

        }

        public override async Task<PageResult<Com.BudgetMetal.DBEntities.EmailLog>> GetPage(string keyword, int page, int totalRecords = 10)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                keyword = string.Empty;
            }

            var records = entities               
               .Where(e =>
                 (e.IsActive == true) &&
                 (keyword == string.Empty || e.Message.Contains(keyword) || e.ToEmailAddress.Contains(keyword))
               )
               .OrderBy(e => new { e.CreatedDate })
               .Skip((totalRecords * page) - totalRecords)
               .Take(totalRecords);

            

            var recordList = records.ToList();

            var count = entities.Where(e =>
                 (e.IsActive == true) &&
                 (keyword == string.Empty || e.Message.Contains(keyword) || e.ToEmailAddress.Contains(keyword)))
                 .ToList().Count();

            var nextPage = 0;
            var prePage = 0;
            if (page > 1)
            {
                prePage = page - 1;
            }

            var totalPage = (count + totalRecords - 1) / totalRecords;
            if (page < totalPage)
            {
                nextPage = page + 1;
            }

            var result = new PageResult<Com.BudgetMetal.DBEntities.EmailLog>()
            {
                Records = recordList,
                TotalPage = totalPage,
                CurrentPage = page,
                PreviousPage = prePage,
                NextPage = nextPage,
                TotalRecords = count
            };

            return result;
        }
    }
}
