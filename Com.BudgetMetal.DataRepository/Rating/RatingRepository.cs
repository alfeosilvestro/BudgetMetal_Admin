using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Com.BudgetMetal.Common;

namespace Com.BudgetMetal.DataRepository.Rating
{
    public class RatingRepository : GenericRepository<Com.BudgetMetal.DBEntities.Rating>, IRatingRepository
    {
        public RatingRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "RatingRepository")
        {
        }

        public async Task<List<Com.BudgetMetal.DBEntities.Rating>> GetRagintByCompany(int Id)
        {
            var records = await entities
            .Include(c => c.User)
            .Where(e => e.IsActive == true
            && e.Company_Id == Id)
            .OrderBy(e => e.CreatedDate)
            .ToListAsync();

            return records;
        }

        public async Task<PageResult<Com.BudgetMetal.DBEntities.Rating>> GetRatingData(int page, int companyId, int numberOfRecord, string keyword)
        {
            var records = await this.entities
                            .Include(u => u.User)
                            .Where(e =>
                            (e.IsActive == true && e.Company_Id == companyId)
                            && (keyword == "" || e.Title.ToLower().Contains(keyword.ToLower())))
                            .OrderByDescending(e => e.CreatedDate)
                            .ToListAsync();

            var dateList = records.Select(e => e.CreatedDate.Date).Distinct().ToList();

            var filterDateList = dateList.Skip((numberOfRecord * page) - numberOfRecord)
                                .Take(numberOfRecord).ToList();

            var recordList = records.Where(e => filterDateList.Contains(e.CreatedDate.Date)).ToList();

            var count = dateList.Count();

            var nextPage = 0;
            var prePage = 0;
            if (page > 1)
            {
                prePage = page - 1;
            }

            var totalPage = (count + numberOfRecord - 1) / numberOfRecord;
            if (page < totalPage)
            {
                nextPage = page + 1;
            }

            var result = new PageResult<Com.BudgetMetal.DBEntities.Rating>()
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
