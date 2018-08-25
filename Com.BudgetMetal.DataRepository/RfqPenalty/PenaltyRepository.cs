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

namespace Com.BudgetMetal.DataRepository.Penalty
{
   
    public class PenaltyRepository : GenericRepository<Com.BudgetMetal.DBEntities.Penalty>, IPenaltyRepository
    {
        public PenaltyRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "PenaltyRepository")
        {

        }


        public override async Task<PageResult<Com.BudgetMetal.DBEntities.Penalty>> GetPage(string keyword, int page, int totalRecords = 10)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                keyword = string.Empty;
                //return await base.GetPage(keyword, page, totalRecords);
            }

            var records = entities
                .Include(ct => ct.Rfq)
                .Where(e =>
                  (e.IsActive == true) &&
                  (keyword == string.Empty || e.Description.Contains(keyword) || e.BreachOfServiceDefinition.Contains(keyword))
                )
                .OrderBy(e => new { e.BreachOfServiceDefinition, e.CreatedDate })
                .Skip((totalRecords * page) - totalRecords)
                .Take(totalRecords);

            var recordList = records
            .Select(r =>
                new Com.BudgetMetal.DBEntities.Penalty()
                {
                    Id = r.Id,
                    BreachOfServiceDefinition = r.BreachOfServiceDefinition,
                    PenaltyAmount = r.PenaltyAmount,
                    Description = r.Description,
                    Rfq_Id = r.Rfq_Id,
                    Rfq = r.Rfq
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

            var result = new PageResult<Com.BudgetMetal.DBEntities.Penalty>()
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


        public void InactiveByRFQId(int rfqId, string UpdatedBy)
        {
            var dbResult = this.entities.Where(e => e.IsActive == true && e.Rfq_Id == rfqId).ToList();
            dbResult.ForEach(e =>
            {
                e.IsActive = false;
                e.UpdatedDate = DateTime.Now;
                e.UpdatedBy = UpdatedBy;
            }
            );
        }
    }
}
