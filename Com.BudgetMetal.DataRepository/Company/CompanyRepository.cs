using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DataRepository.Code_Table;
using Com.BudgetMetal.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Company
{
    public class CompanyRepository : GenericRepository<Com.BudgetMetal.DBEntities.Company>, ICompanyRepository
    {
        public CompanyRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "CompanyRepository")
        {
        }

        public PageResult<Com.BudgetMetal.DBEntities.Company> GetSupplierByServiceTagsId(string serviceTagsId, int page, int totalRecords, string searchKeyword)
        {
            var filterServiceTags = new List<int>();
            var arrServiceTags = serviceTagsId.Split(',');
            foreach (string Id in arrServiceTags)
            {
                filterServiceTags.Add(Convert.ToInt32(Id.Trim()));
            }

            var filterCompany = this.DbContext.SupplierServiceTags.Where(e => e.IsActive == true & filterServiceTags.Contains(e.ServiceTags_Id)).Select(e => e.Company_Id).Distinct().ToList();

            searchKeyword = (searchKeyword == null) ? "" : searchKeyword.ToLower().Trim();
            var records = this.entities.Include(e=>e.SupplierIndustryCertification).Where(e => e.IsActive == true && e.C_BusinessType == Constants_CodeTable.Code_C_Supplier && filterCompany.Contains(e.Id) && (searchKeyword == "" || e.Name.ToLower().Contains(searchKeyword) || e.RegNo.ToLower().Contains(searchKeyword)));

            //var recordList = records
            //    .OrderBy(e=>e.Name)
            //.Skip((totalRecords * page) - totalRecords)
            //.Take(totalRecords)
            //.ToList();

            //var count = records.Count();
            //var totalPage = (count + totalRecords - 1) / totalRecords;


            var recordList = records
               .OrderBy(e => e.Name)
               .ToList();

            var count = recordList.Count();

            var nextPage = 0;
            var prePage = 0;
            if (page > 1)
            {
                prePage = page - 1;
            }

            var totalPage = 1;

            if (page < totalPage)
            {
                nextPage = page + 1;
            }

            var result = new PageResult<Com.BudgetMetal.DBEntities.Company>()
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

        public override async Task<PageResult<Com.BudgetMetal.DBEntities.Company>> GetPage(string keyword, int page, int totalRecords = 10)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                keyword = string.Empty;
                //return await base.GetPage(keyword, page, totalRecords);
            }

            var records = await entities
                .Include(e => e.SupplierIndustryCertification)
               .Where(e =>
                 (e.IsActive == true) &&
                 (keyword == string.Empty || e.Name.Contains(keyword))
               )
               .OrderBy(e => new { e.Name, e.CreatedDate })
               .Skip((totalRecords * page) - totalRecords)
               .Take(totalRecords).ToListAsync();

            var recordList = records.ToList();

            var count = entities.Where(e =>
                 (e.IsActive == true) &&
                 (keyword == string.Empty || e.Name.Contains(keyword)))
                 .ToList().Count();

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

            var result = new PageResult<Com.BudgetMetal.DBEntities.Company>()
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

        public async Task<PageResult<Com.BudgetMetal.DBEntities.Company>> GetSupplierList(string keyword, int page, int totalRecords = 10)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                keyword = string.Empty;
                //return await base.GetPage(keyword, page, totalRecords);
            }

            var records = await entities
                .Include(e => e.SupplierIndustryCertification)
               .Where(e =>
                 (e.IsActive == true) &&
                 (e.C_BusinessType == Com.BudgetMetal.Common.Constants_CodeTable.Code_C_Supplier) &&
                 (e.Name.StartsWith(keyword) || keyword == "")
               ).OrderBy(e => new { e.Name, e.CreatedDate })
              .ToListAsync();

            var recordList = records.ToList()
               .Skip((totalRecords * page) - totalRecords)
               .Take(totalRecords).ToList();

            var count = records.ToList().Count();

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

            var result = new PageResult<Com.BudgetMetal.DBEntities.Company>()
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

        public async Task<Com.BudgetMetal.DBEntities.Company> GetCompanyByUEN(string RegNo)
        {
            return await this.entities.Where(e => e.RegNo.ToLower() == RegNo.ToLower()).FirstOrDefaultAsync();

        }

        public async Task<Com.BudgetMetal.DBEntities.Company> GetSingleCompany(int id)
        {


            var result = await entities
                .Include(e => e.SupplierIndustryCertification)
               .Where(e =>
                 (e.IsActive == true) && e.Id == id).SingleOrDefaultAsync();





            return result;
        }

        public async Task<PageResult<Com.BudgetMetal.DBEntities.Company>> GetSupplierByCompanyId(int companyId, int page, int totalRecords, string keyword)
        {

            var filterCompany = await this.DbContext.CompanySupplier.Where(e => e.IsActive == true && e.Company_Id == companyId).Select(e => e.Supplier_Id).Distinct().ToListAsync();
            string _keyword = (!string.IsNullOrEmpty(keyword)) ? keyword : "";
            var records = await this.entities.Include(e => e.SupplierIndustryCertification).Where(e => e.IsActive == true && filterCompany.Contains(e.Id) && e.Name.ToLower().Contains(_keyword.ToLower())).ToListAsync();

            var recordList = records
                .OrderBy(e => e.Name)
                .Skip((totalRecords * page) - totalRecords)
                .Take(totalRecords)
                .ToList();

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

            var result = new PageResult<Com.BudgetMetal.DBEntities.Company>()
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
