using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Single_Sign_On
{
    

    public interface ISingle_Sign_OnRepository : IGenericRepository<single_sign_on>
    {
        single_sign_on GetSingleSignOnByToken(string token);
    }
}
