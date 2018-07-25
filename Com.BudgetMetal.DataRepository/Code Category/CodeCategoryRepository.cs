using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Com.BudgetMetal.DB.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Com.BudgetMetal.DataRepository.Code_Category
{
    public class CodeCategoryRepository : GenericRepository<CodeCategory>, ICodeCategoryRepository
    {
        public CodeCategoryRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "CodeCategoryRepository")
        {

        }

        public PageResult<CodeCategory> GetCodeCategoryByPage(string keyword, int page, int totalRecords)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                keyword = string.Empty;
                //return await base.GetPage(keyword, page, totalRecords);
            }

            var records = this.DbContext.codeCategorie.Where(e =>
                e.IsActive == true &&
                (keyword == string.Empty ||
                e.Name.Contains(keyword) ||
                e.Description.Contains(keyword))
            );

            var recordList = records
            .Select(r =>
                new CodeCategory()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description
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

            var result = new PageResult<CodeCategory>()
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

        public CodeCategory GetCodeCategoryById(int Id)
        {
            var records = this.DbContext.codeCategorie.Where(x => x.IsActive == true).Select(r =>
                    new CodeCategory()
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Description = r.Description,
                        IsActive = r.IsActive,
                        CreatedBy = r.CreatedBy,
                        CreatedDate = r.CreatedDate,
                        UpdatedBy = r.UpdatedBy,
                        UpdatedDate = r.UpdatedDate,
                        Version = r.Version
                    })
                .Single(e =>
                e.Id == Id);

            return records;
        }

        public CodeCategory GetCodeCategoryFileById(int Id)
        {
            var records = this.DbContext.codeCategorie.Select(r =>
                new CodeCategory()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description

                })
                .Single(e =>
                e.Id == Id);

            return records;
        }
    }
}
