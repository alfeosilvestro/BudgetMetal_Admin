using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Roles
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        PageResult<Role> GetRolesByPage(string keyword, int page, int totalRecords);

        Role GetRoleById(int Id );

        Role GetRoleFileById(int Id);
    }
}
