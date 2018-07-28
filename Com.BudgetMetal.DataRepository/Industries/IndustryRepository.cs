using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Com.BudgetMetal.DB.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Com.BudgetMetal.Common;

namespace Com.BudgetMetal.DataRepository.Industries
{
    public class IndustryRepository : GenericRepository<Industry>, IIndustryRepository
    {
        public IndustryRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "IndustryRepository")
        {

        }

        public PageResult<Industry> GetInsustriesByPage(string keyword, int page, int totalRecords)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                keyword = string.Empty;
                //return await base.GetPage(keyword, page, totalRecords);
            }

            var records = this.DbContext.Industry.Where(e =>
                e.IsActive == true &&
                (keyword == string.Empty ||
                e.Name.Contains(keyword))
            );

            var recordList = records
            .Select(r =>
                new Industry()
                {
                    Id = r.Id,
                    Name = r.Name
                })
            .OrderBy(e => e.Name)
            .OrderBy(e => e.CreatedDate)
            .Skip((totalRecords * page) - totalRecords)
            .Take(totalRecords)
            .ToList();
            //DetailImage = (getDetailImage ? r.DetailImage : null),

            var count = records.Count();

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

            var result = new PageResult<Industry>()
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

        public Industry GetIndustryById(int Id)
        {
            var records = this.DbContext.Industry.Where(x => x.IsActive == true)
                .Single(e =>
                e.Id == Id);

            return records;
        }
    }
}
