using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.DataRepository.UserRoles
{
    public class UserRolesRepository : GenericRepository<Com.BudgetMetal.DBEntities.UserRoles>, IUserRolesRepository
    {
        public UserRolesRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "UserRolesRepository")
        {

        }
    }
}
