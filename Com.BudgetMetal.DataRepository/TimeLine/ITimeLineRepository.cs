using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.TimeLine
{
    public interface ITimeLineRepository : IGenericRepository<Com.BudgetMetal.DBEntities.TimeLine>
    {
        Task<PageResult<Com.BudgetMetal.DBEntities.TimeLine>> GetTimeLineData(int page, int companyId, int numberOfDate, string keyword);
    }
}
