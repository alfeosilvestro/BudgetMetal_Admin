using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.RFQ
{

    public class RfqRepository : GenericRepository<Com.BudgetMetal.DBEntities.Rfq>, IRfqRepository
    {
        public RfqRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "RfqRepository")
        {

        }
        public async Task<PageResult<Com.BudgetMetal.DBEntities.Rfq>> GetRfqByPage(int userId, int documentOwner, int page, int totalRecords, bool isCompanyAdmin, int statusId, string keyword)
        {
            var filterDocument = await this.DbContext.DocumentUser
                .Where(e => e.IsActive == true && e.User_Id == userId)
                .Select(e => e.Document_Id).Distinct().ToListAsync();


            var records = await this.entities
                            .Include(e => e.Document)                            
                            .Include(e => e.Document.DocumentStatus)
                            .Include(e => e.Document.DocumentType)
                            .Include(e => e.Document.Company)
                            .Where(e =>
                              (e.IsActive == true)
                              && (e.Document.IsActive == true)
                              && e.Document.Company_Id == documentOwner
                              && (statusId == 0 || e.Document.DocumentStatus_Id == statusId)
                              && (keyword == "" || e.Document.Title.ToLower().Contains(keyword.ToLower()) || e.Document.DocumentNo.ToLower().Contains(keyword.ToLower()) || e.Document.Company.Name.ToLower().Contains(keyword.ToLower()))
                            )
                            .OrderByDescending(e => e.CreatedDate)
                            .ToListAsync();

            if (!isCompanyAdmin)
            {
                records = records.Where(e => filterDocument.Contains(e.Document_Id)).ToList();
            }

            var recordList = records
                .Skip((totalRecords * page) - totalRecords)
                .Take(totalRecords).ToList();

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

            var result = new PageResult<Com.BudgetMetal.DBEntities.Rfq>()
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

        public async Task<PageResult<Com.BudgetMetal.DBEntities.Rfq>> GetRfqForSupplierByPage(int supplierId, int page, int totalRecords, int statusId, string keyword)
        {
            var filterRfq = await this.DbContext.InvitedSupplier
                .Where(e => e.IsActive == true && e.Company_Id == supplierId)
                .Select(e => e.Rfq_Id).Distinct().ToListAsync();


            var records = await this.entities
                            .Include(e => e.Document)
                            .Include(e => e.Document.DocumentStatus)
                            .Include(e => e.Document.DocumentType)
                            .Include(e => e.Document.Company)
                            .Where(e =>
                              (e.IsActive == true)
                              && (e.Document.IsActive == true)
                              && filterRfq.Contains(e.Id)
                              && (statusId == 0 || e.Document.DocumentStatus_Id == statusId)
                              && (e.Document.DocumentStatus_Id != Constants_CodeTable.Code_RFQ_Delete)
                              && (e.Document.DocumentStatus_Id != Constants_CodeTable.Code_RFQ_Draft)
                              && (keyword == "" || e.Document.Title.ToLower().Contains(keyword.ToLower()) || e.Document.DocumentNo.ToLower().Contains(keyword.ToLower()) || e.Document.Company.Name.ToLower().Contains(keyword.ToLower()))
                            )
                            .OrderByDescending(e => e.CreatedDate)
                            .ToListAsync();


            var recordList = records
                .Skip((totalRecords * page) - totalRecords)
                .Take(totalRecords).ToList();

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

            var result = new PageResult<Com.BudgetMetal.DBEntities.Rfq>()
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


        public async Task<PageResult<Com.BudgetMetal.DBEntities.Rfq>> GetPublicRfqByPage(int page, int totalRecords, int statusId, string keyword)
        {
            var records = await this.entities
                            .Include(e => e.Document)
                            .Include(e => e.Document.DocumentStatus)
                            .Include(e => e.Document.DocumentType)
                            .Include(e => e.Document.Company)
                            .Where(e =>
                              (e.IsActive == true)
                              && (e.Document.IsActive == true)
                              && e.IsPublic == true
                              && (statusId == 0 || e.Document.DocumentStatus_Id == statusId)
                              && (keyword == "" || e.Document.DocumentNo.Contains(keyword) || e.Document.Title.ToLower().Contains(keyword.ToLower()) || e.Document.Company.Name.ToLower().Contains(keyword.ToLower()))
                            )
                            .OrderByDescending(e => e.CreatedDate)
                            .ToListAsync();


            var recordList = records
                .Skip((totalRecords * page) - totalRecords)
                .Take(totalRecords).ToList();

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

            var result = new PageResult<Com.BudgetMetal.DBEntities.Rfq>()
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

        public async Task<Com.BudgetMetal.DBEntities.Rfq> GetSingleRfqById(int id)
        {
            var record = await this.entities
                            .Include(e => e.Document)
                            .Include(e => e.Document.DocumentStatus)
                            .Include(e => e.Document.DocumentType)
                            .Include(e => e.Document.Company)
                            .Include(e => e.Document.DocumentUser)
                            .Include(e => e.Document.Attachment)
                            .Include(e => e.Requirement)
                            .Include(e => e.Penalty)
                            .Include(e => e.Sla)
                            .Include(e => e.RfqPriceSchedule)
                            .Include(e => e.InvitedSupplier)
                            .Include(e => e.Document.DocumentActivity)
                            .Include(e=>e.Document.Clarification)
                            .SingleOrDefaultAsync(e =>
                              (e.IsActive == true)
                              && (e.Id == id)
                            );
            return record;
        }

        public async Task<Com.BudgetMetal.DBEntities.Rfq> GetRfqByQuotation_DocumentId(int documentId)
        {
            var quotation = this.DbContext.Quotation.Where(e => e.Document_Id == documentId).Single();
            var record = await this.entities
                            .Include(e => e.Document)
                            .SingleOrDefaultAsync(e =>
                              (e.IsActive == true)
                              && (e.Id == quotation.Rfq_Id)
                            );
            return record;
        }

        public async Task<PageResult<Com.BudgetMetal.DBEntities.Rfq>> GetPublicRfqByCompany(int page, int companyId, int totalRecords, int statusId, string keyword)
        {
            var records = await this.entities
                            .Include(e => e.Document)
                            .Include(e => e.Document.DocumentStatus)
                            .Include(e => e.Document.DocumentType)
                            .Include(e => e.Document.Company)
                            .Where(e =>
                              (e.IsActive == true)
                              && (e.Document.IsActive == true)
                              && e.IsPublic == true
                              && e.Document.Company_Id == companyId
                              && (statusId == 0 || e.Document.DocumentStatus_Id == statusId)
                              && (keyword == "" || e.Document.DocumentNo.Contains(keyword))
                            )
                            .OrderByDescending(e => e.CreatedDate)
                            .ToListAsync();

            var recordList = records
                .Skip((totalRecords * page) - totalRecords)
                .Take(totalRecords).ToList();

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

            var result = new PageResult<Com.BudgetMetal.DBEntities.Rfq>()
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


        public async Task<List<Com.BudgetMetal.DBEntities.Company>> GetSelectedSupplier(int rfqId)
        {
            var filterCompay = this.DbContext.InvitedSupplier.Where(e => e.IsActive == true && e.Rfq_Id == rfqId).Select(e => e.Company_Id).ToList();

            var result = await this.DbContext.Company.Where(e => filterCompay.Contains(e.Id)).ToListAsync();

            return result;
        }

        public async Task<int> GetSingleRfqByDocumentId(int documentId)
        {
            var record = await this.entities
                            .SingleOrDefaultAsync(e =>
                              (e.IsActive == true)
                              && (e.Document_Id == documentId)
                            );
            return record.Id;
        }

        public async Task<PageResult<Com.BudgetMetal.DBEntities.Rfq>> GetLoadTenderNoticBoardPublicRFQ(int intCount)
        {
            var records = await this.entities
                            .Include(e => e.Document)
                            .Include(e => e.Document.DocumentStatus)
                            .Include(e => e.Document.DocumentType)
                            .Include(e => e.Document.Company)
                            .Where(e =>
                              (e.IsActive == true)
                              && (e.Document.IsActive == true)
                              && e.IsPublic == true
                              && (e.Document.DocumentStatus_Id == Com.BudgetMetal.Common.Constants_CodeTable.Code_RFQ_Open)
                            )
                            .OrderByDescending(e => e.CreatedDate)
                            .ToListAsync();
            var recordList = records
                .Take(intCount).ToList();

            var count = records.Count();
            var result = new PageResult<Com.BudgetMetal.DBEntities.Rfq>()
            {
                Records = recordList,
                TotalPage = 1,
                CurrentPage = 1,
                PreviousPage = 1,
                NextPage = 1,
                TotalRecords = intCount
            };

            return result;
        }
    }
}
