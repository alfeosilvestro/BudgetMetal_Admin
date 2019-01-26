using Com.BudgetMetal.ViewModels.Facebook;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.Facebook
{
    public interface IFacebookService
    {
        string Publish(string textToPublish, string imageUrl);
        Task SimplePost(string textToPublish);
    }
}
