using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Com.BudgetMetal.DataRepository.ServiceTags
{
    public class ServiceTagsRepository : GenericRepository<Com.BudgetMetal.DBEntities.ServiceTags>, IServiceTagsRepository
    {
        public ServiceTagsRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "ServiceTagsRepository")
        {

        }

        public override async Task<PageResult<Com.BudgetMetal.DBEntities.ServiceTags>> GetPage(string keyword, int page, int totalRecords = 10)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                keyword = string.Empty;
                //return await base.GetPage(keyword, page, totalRecords);
            }

            var records = entities
                .Include(ct => ct.Industry)
                .Where(e =>
                  (e.IsActive == true) &&
                  (keyword == string.Empty || e.Name.Contains(keyword))
                )
                .OrderBy(e => new { e.Name, e.CreatedDate })
                .Skip((totalRecords * page) - totalRecords)
                .Take(totalRecords);

            var recordList = records
            .Select(r =>
                new Com.BudgetMetal.DBEntities.ServiceTags()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    Industry_Id = r.Industry_Id,
                    Industry = r.Industry
                })
            .ToList();

            var count = await records.CountAsync();

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

            var result = new PageResult<Com.BudgetMetal.DBEntities.ServiceTags>()
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
