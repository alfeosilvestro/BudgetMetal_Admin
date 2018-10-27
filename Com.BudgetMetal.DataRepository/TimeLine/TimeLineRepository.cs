using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.TimeLine
{
    public class TimeLineRepository : GenericRepository<Com.BudgetMetal.DBEntities.TimeLine>, ITimeLineRepository
    {
        public TimeLineRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "TimeLineRepository")
        {

        }
        
        public async Task<PageResult<Com.BudgetMetal.DBEntities.TimeLine>> GetTimeLineData(int page, int companyId, int numberOfDate, string keyword)
        {
            var records = await this.entities
                            .Include(e => e.User)
                            .Include(e => e.Company)
                            .Include(e=> e.Document)
                            .Where(e =>
                            (e.IsActive == true && e.Company_Id == companyId)
                            && (keyword == "" || e.Message.ToLower().Contains(keyword.ToLower())))
                            .OrderByDescending(e => e.CreatedDate)
                            .ToListAsync();

            var dateList = records.Select(e => e.CreatedDate.Date).Distinct().ToList();

            var filterDateList = dateList.Skip((numberOfDate * page) - numberOfDate)
                                .Take(numberOfDate).ToList();

            var recordList = records.Where(e=> filterDateList.Contains(e.CreatedDate.Date)).ToList();

            var count = dateList.Count();

            var nextPage = 0;
            var prePage = 0;
            if (page > 1)
            {
                prePage = page - 1;
            }

            var totalPage = (count + numberOfDate - 1) / numberOfDate;
            if (page < totalPage)
            {
                nextPage = page + 1;
            }

            var result = new PageResult<Com.BudgetMetal.DBEntities.TimeLine>()
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
