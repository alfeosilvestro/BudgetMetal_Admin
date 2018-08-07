using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Com.BudgetMetal.DataRepository.ServiceTags
{
    public class ServiceTagsRepository : GenericRepository<Com.BudgetMetal.DBEntities.ServiceTags>, IServiceTagsRepository
    {
        public ServiceTagsRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "ServiceTagsRepository")
        {

        }

        public async Task<List<Com.BudgetMetal.DBEntities.ServiceTags>> GetServiceTagsByIndustry(int Id)
        {
            //return await Task.Run(() =>
            //{
            //    return entities.Where(e => e.IsActive == true && e.Industry_Id == Id).ToList();
            //});
            return await this.entities.Where(e => e.IsActive == true && e.Industry_Id == Id).ToListAsync();
        }
    }
}
