using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Com.BudgetMetal.DataRepository.Rating
{
    public class RatingRepository : GenericRepository<Com.BudgetMetal.DBEntities.Rating>, IRatingRepository
    {
        public RatingRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "RatingRepository")
        {
        }

        public async Task<List<Com.BudgetMetal.DBEntities.Rating>> GetRagintByCompany(int Id)
        {
            var records = await entities
            .Include(c => c.User)
            .Where(e => e.IsActive == true
            && e.Company_Id == Id)
            .OrderBy(e => e.CreatedDate)
            .ToListAsync();

            return records;
        }
    }
}
