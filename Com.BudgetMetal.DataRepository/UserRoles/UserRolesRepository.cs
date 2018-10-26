using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Com.BudgetMetal.DataRepository.UserRoles
{
    public class UserRolesRepository : GenericRepository<Com.BudgetMetal.DBEntities.UserRoles>, IUserRolesRepository
    {
        public UserRolesRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "UserRolesRepository")
        {

        }

        public async Task<List<Com.BudgetMetal.DBEntities.UserRoles>> GetUserRolesByUserId(int userId)
        {
            var records = await entities.Where(e => e.IsActive == true
                         && e.User_Id == userId)
                        .ToListAsync();

            return records;
        }

        public async Task<Com.BudgetMetal.DBEntities.UserRoles> GetUserRolesByUserIdRoleId(int userId, int roleId)
        {
            var record = await this.entities
                            .SingleOrDefaultAsync(e =>
                              (e.IsActive == true)
                              && (e.Role_Id == roleId)
                              && (e.User_Id == userId)
                            );
            return record;
        }
    }
}
