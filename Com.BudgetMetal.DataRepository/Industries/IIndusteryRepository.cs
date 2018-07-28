using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.DataRepository.Industries
{
    public interface IIndustryRepository : IGenericRepository<Industry>
    {
        PageResult<Industry> GetInsustriesByPage(string keyword, int page, int totalRecords);

        Industry GetIndustryById(int Id);
    }
}
