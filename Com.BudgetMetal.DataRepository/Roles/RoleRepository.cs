﻿using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Com.BudgetMetal.DBEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Roles
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "RoleRepository")
        {

        }

        public override async Task<PageResult<Com.BudgetMetal.DBEntities.Role>> GetPage(string keyword, int page, int totalRecords = 10)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                keyword = string.Empty;
                //return await base.GetPage(keyword, page, totalRecords);
            }

            var records = entities
               .Where(e =>
                 (e.IsActive == true) &&
                 (keyword == string.Empty || e.Name.Contains(keyword) || e.Code.Contains(keyword))
               )
               .OrderBy(e => new { e.Name, e.CreatedDate })
               .Skip((totalRecords * page) - totalRecords)
               .Take(totalRecords);



            var recordList = records.ToList();

            var count = entities.Where(e =>
                 (e.IsActive == true) &&
                 (keyword == string.Empty || e.Name.Contains(keyword)))
                 .ToList().Count();
            //await records.CountAsync();

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

            var result = new PageResult<Com.BudgetMetal.DBEntities.Role>()
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
    }
}
