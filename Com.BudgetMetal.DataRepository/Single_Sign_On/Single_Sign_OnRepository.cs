using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Com.BudgetMetal.DB.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Single_Sign_On
{
    

    public class Single_Sign_OnRepository : GenericRepository_BM<single_sign_on>, ISingle_Sign_OnRepository
    {
        public Single_Sign_OnRepository(DataContext_BM contextBm, ILoggerFactory loggerFactory) : base(contextBm, loggerFactory, "SingleSignOnRepository")
        {

        }

       

        public single_sign_on GetSingleSignOnByToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }
            var records = this.DbContext_BM.single_sign_on.OrderByDescending(e=>e.Id).Single(e =>
                e.Authentication_Token == token && e.Status == 1);

            return records;
        }

       
    }
}
