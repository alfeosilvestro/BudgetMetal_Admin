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
using System.Linq;

namespace Com.BudgetMetal.DataRepository.RFQ
{

    public class RfqRepository : GenericRepository<Com.BudgetMetal.DBEntities.Rfq>, IRfqRepository
    {
        public RfqRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "RfqRepository")
        {

        }
        public async Task<PageResult<Com.BudgetMetal.DBEntities.Rfq>> GetRfqByPage(int documentOwner, int page, int totalRecords)
        {
            var records = await this.entities
                            .Include(e => e.Document)
                            .Include(e => e.Document.DocumentStatus)
                            .Include(e => e.Document.DocumentType)
                            .Include(e => e.Document.Company)
                            .Where(e =>
                              (e.IsActive == true)
                              && (e.Document.IsActive == true)
                            )
                            .OrderByDescending(e => e.CreatedDate)
                            .ToListAsync();


            var recordList = records
                .Skip((totalRecords * page) - totalRecords)
                .Take(totalRecords).ToList();

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

        public async Task<Com.BudgetMetal.DBEntities.Rfq> GetSingleRfqById(int documentId)
        {
            var record = await this.entities
                            .Include(e => e.Document)
                            .Include(e => e.Document.DocumentStatus)
                            .Include(e => e.Document.DocumentType)
                            .Include(e => e.Document.Company)
                            .Include(e => e.Document.DocumentUser)
                            .Include(e => e.Document.DocumentUser).ThenInclude(u => u.User)
                            .Include(e => e.Document.DocumentUser).ThenInclude(u => u.Role)
                            .Include(e=>e.Document.Attachment)
                            .Include(e=>e.Requirement)
                            .Include(e => e.Penalty)
                            .Include(e => e.Quotation)
                            .Include(e => e.Sla)
                            .Include(e => e.RfqPriceSchedule)
                            .Include(e => e.InvitedSupplier)
                            .SingleOrDefaultAsync(e =>
                              (e.IsActive == true)
                              && (e.Document_Id == documentId)
                            );
            return record;
        }
    }
}
