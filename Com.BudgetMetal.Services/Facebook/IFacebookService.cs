using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.Facebook;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.Facebook
{
    public interface IFacebookService
    {
        Task<string> GetAccessTokenAsync();
        Task<VmGenericServiceResult> PostMessage(string message);
    }
}
