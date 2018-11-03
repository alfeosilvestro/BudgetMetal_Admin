using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Com.BudgetMetal.DBEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Quotation
{
    public class QuotationRepository : GenericRepository<Com.BudgetMetal.DBEntities.Quotation>, IQuotationRepository
    {
        public QuotationRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "QuotationRepository")
        {

        }

        public async Task<PageResult<Com.BudgetMetal.DBEntities.Quotation>> GetQuotationByPage(int userId, int companyId, int page, int totalRecords, bool isCompany, int statusId = 0, string keyword = "")
        {
            var records = await this.entities
                            .Include(e => e.Document)
                            .Include(e => e.Document.DocumentStatus)
                            .Include(e => e.Document.DocumentType)
                            .Include(e => e.Document.Company)
                            .Include(e => e.Rfq)
                            .Where(e =>
                              (e.IsActive == true)
                              && (e.Document.IsActive == true)
                              && (e.Document.Company_Id == companyId)
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

            var result = new PageResult<Com.BudgetMetal.DBEntities.Quotation>()
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

        public async Task<PageResult<Com.BudgetMetal.DBEntities.Quotation>> GetQuotationForBuyerByPage(int userId, int buyerId, int page, int totalRecords, bool isCompany, int statusId = 0, string keyword = "")
        {
            var filterRfqId = this.DbContext.Rfq.Include(e => e.Document)
                .Where(e => e.Document.Company_Id == buyerId).Select(e => e.Id).ToList();

            

            var records = await this.entities
                            .Include(e => e.Document)
                            .Include(e => e.Document.DocumentStatus)
                            .Include(e => e.Document.DocumentType)
                            .Include(e => e.Document.Company)
                            .Include(e=> e.Rfq)
                            .Where(e =>
                              (e.IsActive == true)
                              && (e.Document.IsActive == true)
                              && (filterRfqId.Contains(e.Rfq_Id))
                              && (e.Document.DocumentStatus_Id != Constants_CodeTable.Code_Quotation_Draft)
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

            var result = new PageResult<Com.BudgetMetal.DBEntities.Quotation>()
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

        public async Task<PageResult<Com.BudgetMetal.DBEntities.Quotation>> GetQuotationByRfqId(int RfqId, int page, int totalRecords, int statusId, string keyword)
        {
            var records = await this.entities
                            .Include(e => e.Document)
                            .Include(e => e.Document.DocumentStatus)
                            .Include(e => e.Document.DocumentType)
                            .Include(e => e.Document.Company)
                            .Include(e=> e.Document.DocumentActivity)
                            .Where(e =>
                              (e.IsActive == true)
                              && (e.Document.IsActive == true)
                              && (e.Rfq_Id == RfqId)
                              && (statusId == 0 || e.Document.DocumentStatus_Id != Constants_CodeTable.Code_Quotation_Draft)
                              && (keyword == "" || e.Document.DocumentNo == keyword)
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

            var result = new PageResult<Com.BudgetMetal.DBEntities.Quotation>()
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


        public async Task<List<Com.BudgetMetal.DBEntities.Quotation>> GetQuotationByRfqId(int RfqId)
        {
            var records = await this.entities
                            .Include(e => e.Document)
                            .Include(e => e.Document.DocumentStatus)
                            .Include(e => e.Document.DocumentType)
                            .Include(e => e.Document.Company)
                            .Include(e => e.QuotationPriceSchedule)
                            .Include(e => e.QuotationRequirement)
                            .Where(e =>
                              (e.IsActive == true)
                              && (e.Document.IsActive == true)
                              && (e.Rfq_Id == RfqId)
                            )
                            .OrderByDescending(e => e.CreatedDate)
                            .ToListAsync();
            
            return records;
        }

        public async Task<Com.BudgetMetal.DBEntities.Quotation> GetSingleQuotationById(int id)
        {
            var record = await this.entities
                            .Include(e => e.Document)
                            .Include(e => e.Document.DocumentStatus)
                            .Include(e => e.Document.DocumentType)
                            .Include(e => e.Document.Company)
                            .Include(e => e.Document.DocumentUser)
                            .Include(e => e.Document.Attachment)
                            .Include(e => e.QuotationRequirement)
                            .Include(e => e.QuotationPriceSchedule)
                            .Include(e => e.Document.DocumentActivity)
                            .Include(e=>e.Document.Clarification)
                            .SingleOrDefaultAsync(e =>
                              (e.IsActive == true)
                              && (e.Id == id)
                            );
            return record;
        }

        public async Task<Com.BudgetMetal.DBEntities.Quotation> GetQuotationBy_RfqId_CompanyId(int rfqId, int companyId)
        {
            var filterStatus = new List<int>();
            filterStatus.Add(Constants_CodeTable.Code_Quotation_Accepted);
            filterStatus.Add(Constants_CodeTable.Code_Quotation_Draft);
            filterStatus.Add(Constants_CodeTable.Code_Quotation_Rejected);
            filterStatus.Add(Constants_CodeTable.Code_Quotation_Submitted);

           
            var record = await this.entities.Include(e => e.Document)
                .Where(e => e.IsActive == true
                && e.Document.IsActive ==  true
                && e.Rfq_Id == rfqId
                && e.Document.Company_Id == companyId
                && filterStatus.Contains( e.Document.DocumentStatus_Id)
                ).SingleOrDefaultAsync();

            return record;
        }

        public int GetRfqOwnerId(int documentId)
        {
            var rfqDocumentId = this.entities.Where(e => e.Document_Id == documentId).First().Rfq_Id;

            return this.DbContext.Rfq.Include(e => e.Document).Where(e => e.Id == rfqDocumentId).First().Document.Company_Id;
        }

        public async Task<int> GetQuotationByDocumentId(int documentId)
        {
            var record = await this.entities
                            .SingleOrDefaultAsync(e =>
                            (e.IsActive == true)
                            && (e.Document_Id == documentId)
                            );
            return record.Id;
        }
    }
}
