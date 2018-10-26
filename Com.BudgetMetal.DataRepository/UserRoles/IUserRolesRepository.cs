using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.UserRoles
{
    public interface IUserRolesRepository : IGenericRepository<Com.BudgetMetal.DBEntities.UserRoles>
    {
        Task<List<Com.BudgetMetal.DBEntities.UserRoles>> GetUserRolesByUserId(int userId);
        Task<Com.BudgetMetal.DBEntities.UserRoles> GetUserRolesByUserIdRoleId(int userId, int roleId);
    }
}
