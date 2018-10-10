using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.TimeLine;
using Com.BudgetMetal.Services.Base;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.TimeLine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.TimeLine
{
    public class TimeLineService : BaseService, ITimeLineService
    {
        private readonly ITimeLineRepository repo;

        public TimeLineService(ITimeLineRepository repo)
        {
            this.repo = repo;
        }

        
    }
}
