using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Com.BudgetMetal.DBEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Code_Table
{
    public class CodeTableRepository : GenericRepository<CodeTable>, ICodeTableRepository
    {
        public CodeTableRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "CodeTableRepository")
        {
        }

        public override async Task<PageResult<CodeTable>> GetPage(string keyword, int page, int totalRecords = 10)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                keyword = string.Empty;
                //return await base.GetPage(keyword, page, totalRecords);
            }

            var records = this.DbContext.CodeTable
                .Include("CodeCategory")
                .Where(e =>
                  (keyword == string.Empty ||
                  e.Name.Contains(keyword))
            );

            var recordList = records
            .Select(r =>
                new CodeTable()
                {
                    Id = r.Id,
                    Name = r.Name,
                    CodeCategory_Id = r.CodeCategory_Id,
                    CodeCategory = r.CodeCategory
                })
            .OrderBy(e => new { e.Name, e.CreatedDate })
            .Skip((totalRecords * page) - totalRecords)
            .Take(totalRecords)
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

            var result = new PageResult<CodeTable>()
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
