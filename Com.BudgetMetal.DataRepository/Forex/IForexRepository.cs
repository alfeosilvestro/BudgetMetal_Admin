using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Forex
{
    public interface IForexRepository
    {
        Task<string> GetForexDataFromBankApi();
    }
}
