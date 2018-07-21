using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Com.BudgetMetal.DB.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Roles
{
    public class RoleRepository : GenericRepository<roles>, IRoleRepository
    {
        public RoleRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "GalleryRepository")
        {

        }

        public PageResult<roles> GetRolesByPage(string keyword, int page, int totalRecords)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                keyword = string.Empty;
                //return await base.GetPage(keyword, page, totalRecords);
            }

            var records = this.DbContext.roles.Where(e => 
                e.IsActive==true &&
                (keyword == string.Empty ||
                e.Role.Contains(keyword) ||
                e.RoleCode.Contains(keyword))
            );

            var recordList = records
            .Select(r =>
                new roles()
                {
                    Id = r.Id,
                    Role = r.Role,
                    RoleCode = r.RoleCode
                })
            .OrderBy(e => e.RoleCode)
            .OrderBy(e => e.CreatedDate)
            .Skip((totalRecords * page) - totalRecords)
            .Take(totalRecords)
            .ToList();
            //DetailImage = (getDetailImage ? r.DetailImage : null),

            var count = records.Count();

            var nextPage = 0;
            var prePage = 0;
            if (page > 1)
            {
                prePage = page - 1;
            }

            var totalPage = (count + totalRecords - 1) / totalRecords;
            if (page < totalPage)
            {
                nextPage = page + 1;
            }

            var result = new PageResult<roles>()
            {
                Records = recordList,
                TotalPage = totalPage,
                CurrentPage = page,
                PreviousPage = prePage,
                NextPage = nextPage,
                TotalRecords = count
            };

            return result;
        }

        public roles GetRoleById(int Id)
        {
            var records = this.DbContext.roles.Where(x=>x.IsActive==true).Select(r =>
                new roles()
                {
                    Id = r.Id,
                    Role = r.Role,
                    RoleCode = r.RoleCode,
                    IsActive = r.IsActive,
                    CreatedBy = r.CreatedBy,
                    CreatedDate = r.CreatedDate,
                    UpdatedBy = r.UpdatedBy,
                    UpdatedDate = r.UpdatedDate,
                    Version = r.Version
                })
                .Single(e =>
                e.Id == Id);

            return records;
        }

        public roles GetRoleFileById(int Id)
        {
            var records = this.DbContext.roles.Select(r =>
                new roles()
                {
                    Id = r.Id,
                    Role = r.Role,
                    RoleCode = r.RoleCode
                    
                })
                .Single(e =>
                e.Id == Id);

            return records;
        }
    }
}
