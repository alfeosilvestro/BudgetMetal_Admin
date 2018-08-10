using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Users
{
    public interface IUserRepository : IGenericRepository<User>
    {
        //PageResult<User> GetUsersByPage1(string keyword, int page, int totalRecords);

        //User GetUserById(int Id );

        //Role GetRoleFileById(int Id);
    }
}
