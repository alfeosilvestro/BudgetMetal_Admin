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

namespace Com.BudgetMetal.DataRepository.Document
{
   
    public class DocumentRepository : GenericRepository<Com.BudgetMetal.DBEntities.Document>, IDocumentRepository
    {
        public DocumentRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "DocumentRepository")
        {

        }

        public override async Task<PageResult<Com.BudgetMetal.DBEntities.Document>> GetPage(string keyword, int page, int totalRecords = 10)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                keyword = string.Empty;
            }

            var records = entities
                .Include(ct => ct.Company)
                .Include(d => d.DocumentStatus)
                .Include(d => d.DocumentType)
                .Where(e =>
                  (e.IsActive == true) &&
                  (keyword == string.Empty || e.Title.Contains(keyword))
                )
                .OrderBy(e => new { e.Title, e.CreatedDate })
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

            var result = new PageResult<Com.BudgetMetal.DBEntities.Document>()
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
