using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DBEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Rating
{
    public interface IRatingRepository : IGenericRepository<Com.BudgetMetal.DBEntities.Rating>
    {
        Task<List<Com.BudgetMetal.DBEntities.Rating>> GetRagintByCompany(int Id);

        Task<PageResult<Com.BudgetMetal.DBEntities.Rating>> GetRatingData(int page, int companyId, int numberOfRecord, string keyword);
    }
}
