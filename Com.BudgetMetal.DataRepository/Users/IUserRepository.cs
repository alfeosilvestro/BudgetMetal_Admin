using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Users
{
    public interface IUserRepository : IGenericRepository<user>
    {
        PageResult<user> GetUsersByPage(string keyword, int page, int totalRecords);

        user GetUserById(int Id );

        roles GetRoleFileById(int Id);

        
    }
}
