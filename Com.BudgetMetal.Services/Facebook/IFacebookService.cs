using Com.BudgetMetal.ViewModels.Facebook;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.Facebook
{
    public interface IFacebookService
    {
        Task<Account> GetAccountAsync(string accessToken);
        Task PostOnWallAsync(string accessToken, string message);
    }
}
