using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Roles
{
    public interface IRoleRepository : IGenericRepository<roles>
    {
        PageResult<roles> GetRolesByPage(string keyword, int page, int totalRecords);

        roles GetRoleById(int Id );

        roles GetRoleFileById(int Id);

        
    }
}
