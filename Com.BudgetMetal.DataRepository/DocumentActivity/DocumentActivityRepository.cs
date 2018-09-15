using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Com.BudgetMetal.Common;

namespace Com.BudgetMetal.DataRepository.DocumentActivity
{
    public class DocumentActivityRepository : GenericRepository<Com.BudgetMetal.DBEntities.DocumentActivity>, IDocumentActivityRepository
    {
        public DocumentActivityRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "DocumentActivityRepository")
        {

        }
        public async Task<PageResult<Com.BudgetMetal.DBEntities.DocumentActivity>> GetDocumentActivityWithDocumentId(int DocumentId, bool IsRfq)
        {
            var records = await this.entities.Where(x=>x.IsActive && x.IsRfq && x.Document_Id == DocumentId)
                                   .ToListAsync();
            var recordList = records
                .ToList();

            var result = new PageResult<Com.BudgetMetal.DBEntities.DocumentActivity>()
            {
                Records = recordList,
                TotalPage = 0,
                CurrentPage = 0,
                PreviousPage = 0,
                NextPage = 0,
                TotalRecords = recordList.Count
            };

            return result;
        }
    }
}
