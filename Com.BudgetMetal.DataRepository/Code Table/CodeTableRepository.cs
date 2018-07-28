using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Com.BudgetMetal.DB.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Com.BudgetMetal.DataRepository.Code_Table
{
    public class CodeTableRepository : GenericRepository<CodeTable>, ICodeTableRepository
    {
        public CodeTableRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "CodeTableRepository")
        {

        }

        public PageResult<CodeTable> GetCodeTableByPage(string keyword, int page, int totalRecords)
        {
            //ICollection<CodeTable> ct = this.DbContext.CodeTable.Include(c => c.CodeCategory).ToList();
            if (string.IsNullOrEmpty(keyword))
            {
                keyword = string.Empty;
                //return await base.GetPage(keyword, page, totalRecords);
            }

            var records = this.DbContext.CodeTable.Where(e =>
                e.IsActive == true &&
                (keyword == string.Empty ||
                e.Name.Contains(keyword))
            );

            var recordList = records
            .Select(r =>
                new CodeTable()
                {
                    Id = r.Id,
                    CodeCategory_Id = r.CodeCategory_Id,
                    Name = r.Name,
                    CodeCategory = r.CodeCategory
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

        public CodeTable GetCodeTableById(int Id)
        {
            var records = this.DbContext.CodeTable.Where(x => x.IsActive == true)
                .Single(e =>
                e.Id == Id);

            return records;
        }

        public CodeTable GetCodeTableFileById(int Id)
        {
            var records = this.DbContext.CodeCategory.Select(r =>
                new CodeTable()
                {
                    Id = r.Id,
                    Name = r.Name

                })
                .Single(e =>
                e.Id == Id);

            return records;
        }

        public int GetLastId()
        {
            var record = this.DbContext.CodeTable.OrderByDescending(x => x.Id).FirstOrDefault();
            if (record != null)
            {
                return record.Id + 1;
            }
            else
            {
                return 1;
            }
        }

    }
}
