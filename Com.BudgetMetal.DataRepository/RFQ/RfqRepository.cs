using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Com.BudgetMetal.DataRepository.RFQ
{
   
    public class RfqRepository : GenericRepository<Com.BudgetMetal.DBEntities.Rfq>, IRfqRepository
    {
        public RfqRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "RfqRepository")
        {

        }

        public override async Task<PageResult<Com.BudgetMetal.DBEntities.Rfq>> GetPage(string keyword, int page, int totalRecords = 10)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                keyword = string.Empty;
                //return await base.GetPage(keyword, page, totalRecords);
            }

            var records = entities
               .Include(ct => ct.Document)
               .Where(e =>
                 (e.IsActive == true) &&
                 (keyword == string.Empty || e.InternalProjectName.Contains(keyword) || e.InternalRefrenceNo.Contains(keyword))
               )
               .OrderBy(e => new { e.InternalRefrenceNo, e.CreatedDate })
               .Skip((totalRecords * page) - totalRecords)
               .Take(totalRecords);

            var recordList = records.ToList();

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

            var result = new PageResult<Com.BudgetMetal.DBEntities.Rfq>()
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
