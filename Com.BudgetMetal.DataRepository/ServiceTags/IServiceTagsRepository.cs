using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.ServiceTags
{
    public interface IServiceTagsRepository : IGenericRepository<Com.BudgetMetal.DBEntities.ServiceTags>
    {
        Task<List<Com.BudgetMetal.DBEntities.ServiceTags>> GetServiceTagsByIndustry(int Id);
    }
}
