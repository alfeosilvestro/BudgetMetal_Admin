using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Attachment;
using Com.BudgetMetal.DataRepository.Document;
using Com.BudgetMetal.DataRepository.InvitedSupplier;
using Com.BudgetMetal.DataRepository.Penalty;
using Com.BudgetMetal.DataRepository.Requirement;
using Com.BudgetMetal.DataRepository.RFQ;
using Com.BudgetMetal.DataRepository.RfqPriceSchedule;
using Com.BudgetMetal.DataRepository.Sla;
using Com.BudgetMetal.DBEntities;
using Com.BudgetMetal.Services.Base;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.Document;
using Com.BudgetMetal.ViewModels.CodeTable;
using Com.BudgetMetal.ViewModels.Rfq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Com.BudgetMetal.ViewModels.Attachment;
using Com.BudgetMetal.ViewModels.Requirement;
using Com.BudgetMetal.ViewModels.Sla;
using Com.BudgetMetal.ViewModels.RfqPenalty;
using Com.BudgetMetal.ViewModels.RfqPriceSchedule;
using Com.BudgetMetal.ViewModels.InvitedSupplier;
using Com.BudgetMetal.DataRepository.DocumentUser;
using Com.BudgetMetal.ViewModels.DocumentUser;
using Com.BudgetMetal.ViewModels.User;
using Com.BudgetMetal.ViewModels.Role;
using System.Linq;
using Com.BudgetMetal.DataRepository.Users;
using Com.BudgetMetal.DataRepository.Roles;
using Com.BudgetMetal.ViewModels.Quotation;
using Com.BudgetMetal.ViewModels.QuotationPriceSchedule;
using Com.BudgetMetal.DataRepository.Quotation;
using Com.BudgetMetal.DataRepository.QuotationPriceSchedule;
using Com.BudgetMetal.DataRepository.Company;

namespace Com.BudgetMetal.Services.Quotation
{
    public class QuotationService : BaseService, IQuotationService
    {

        public readonly IRfqRepository repoRfq;
        public readonly IDocumentRepository repoDocument;
        public readonly IQuotationRepository repoQuotation;
        private readonly IAttachmentRepository repoAttachment;
        private readonly IDocumentUserRepository repoDocumentUser;
        private readonly IQuotationPriceScheduleRepository repoPriceSchedule;
        private readonly ICompanyRepository repoCompany;
        private readonly IUserRepository repoUser;
        private readonly IRoleRepository repoRole;

        public QuotationService(IRfqRepository repoRfq, IDocumentRepository repoDocument, IQuotationRepository repoQuotation, IAttachmentRepository repoAttachment, IDocumentUserRepository repoDocumentUser, IQuotationPriceScheduleRepository repoPriceSchedule, IUserRepository repoUser, IRoleRepository repoRole, ICompanyRepository repoCompany)
        {
            this.repoRfq = repoRfq;
            this.repoDocument = repoDocument;
            this.repoQuotation = repoQuotation;
            this.repoDocumentUser = repoDocumentUser;
            this.repoAttachment = repoAttachment;
            this.repoPriceSchedule = repoPriceSchedule;
            this.repoRole = repoRole;
            this.repoUser = repoUser;
            this.repoCompany = repoCompany;
        }

        public async Task<VmQuotationPage> GetQuotationByPage(int documentOwner, int page, int totalRecords)
        {
            var dbPageResult = await repoQuotation.GetQuotationByPage(documentOwner,
                (page == 0 ? Constants.app_firstPage : page),
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords));

            //var dbPageResult = repo.GetCodeTableByPage(keyword,
            //    (page == 0 ? Constants.app_firstPage : page),
            //    (totalRecords == 0 ? Constants.app_totalRecords : totalRecords));

            if (dbPageResult == null)
            {
                return new VmQuotationPage();
            }

            var resultObj = new VmQuotationPage();
            resultObj.RequestId = DateTime.Now.ToString("yyyyMMddHHmmss");
            resultObj.RequestDate = DateTime.Now;
            resultObj.Result = new PageResult<VmQuotationItem>();
            resultObj.Result.Records = new List<VmQuotationItem>();

            Copy<PageResult<Com.BudgetMetal.DBEntities.Quotation>, PageResult<VmQuotationItem>>(dbPageResult, resultObj.Result, new string[] { "Records" });

            foreach (var dbItem in dbPageResult.Records)
            {
                var resultItem = new VmQuotationItem();

                Copy<Com.BudgetMetal.DBEntities.Quotation, VmQuotationItem>(dbItem, resultItem);

                if (dbItem.Document != null)
                {
                    resultItem.Document = new ViewModels.Document.VmDocumentItem()
                    {
                        DocumentNo = dbItem.Document.DocumentNo,
                        DocumentStatus = new ViewModels.CodeTable.VmCodeTableItem()
                        {
                            Name = dbItem.Document.DocumentStatus.Name
                        },
                        DocumentType = new ViewModels.CodeTable.VmCodeTableItem()
                        {
                            Name = dbItem.Document.DocumentStatus.Name
                        },
                        Company = new ViewModels.Company.VmCompanyItem()
                        {
                            Name = dbItem.Document.Company.Name
                        }
                    };

                }

                resultObj.Result.Records.Add(resultItem);
            }

            return resultObj;
        }


        public async Task<VmQuotationItem> InitialLoadByRfqId(int RfqId)
        {
            var resultObj = new VmQuotationItem();

            var dbResult = await repoRfq.GetSingleRfqById(RfqId);

            var resultRfq = new VmRfqItem();

            Copy<Com.BudgetMetal.DBEntities.Rfq, VmRfqItem>(dbResult, resultRfq, new string[] { "Document", "InvitedSupplier", "Penalty", "Requirement", "RfqPriceSchedule", "Sla" });

            var resultDocument = new VmDocumentItem();

            Copy<Com.BudgetMetal.DBEntities.Document, VmDocumentItem>(dbResult.Document, resultDocument, new string[] { "DocumentStatus", "DocumentType", "Attachment", "DocumentUser", "DocumentStatus", "DocumentType", "Company" });

            resultRfq.Document = resultDocument;

            resultObj.Rfq = resultRfq;
            resultObj.Rfq_Id = resultRfq.Id;
            var listPriceSchdule = new List<VmQuotationPriceScheduleItem>();

            if (dbResult.RfqPriceSchedule != null)
            {
                foreach (var dbItem in dbResult.RfqPriceSchedule)
                {
                    var resultPriceSchedule = new VmQuotationPriceScheduleItem();

                    Copy<Com.BudgetMetal.DBEntities.RfqPriceSchedule, VmQuotationPriceScheduleItem>(dbItem, resultPriceSchedule, new string[] { "Rfq_Id", "Rfq_Id", "ItemAmount", "Quotation_Id" });

                    listPriceSchdule.Add(resultPriceSchedule);
                }
            }
            resultObj.QuotationPriceSchedule = listPriceSchdule;



            return resultObj;
        }


        public string SaveQuotation(VmQuotationItem quotation)
        {
            var dbDocument = new Com.BudgetMetal.DBEntities.Document();
            string documentNo = "";
            documentNo = GenerateDocumentNo(quotation.Document.Company_Id);
            quotation.Document.DocumentNo = documentNo;
            quotation.Document.WorkingPeriod = GetCurrentWeek();
            Copy<VmDocumentItem, Com.BudgetMetal.DBEntities.Document>(quotation.Document, dbDocument);
            repoDocument.Add(dbDocument);
            repoDocument.Commit();

            quotation.Document_Id = dbDocument.Id;

            var dbQuotation = new Com.BudgetMetal.DBEntities.Quotation();
            Copy<VmQuotationItem, Com.BudgetMetal.DBEntities.Quotation>(quotation, dbQuotation);
            repoQuotation.Add(dbQuotation);
            repoQuotation.Commit();


            if (quotation.Document.Attachment.Count > 0)
            {
                foreach (var item in quotation.Document.Attachment)
                {
                    var dbAttachment = new Com.BudgetMetal.DBEntities.Attachment();

                    Copy<VmAttachmentItem, Com.BudgetMetal.DBEntities.Attachment>(item, dbAttachment);
                    dbAttachment.Document_Id = dbDocument.Id;
                    dbAttachment.CreatedBy = dbAttachment.UpdatedBy = dbQuotation.CreatedBy;
                    repoAttachment.Add(dbAttachment);
                }
                repoAttachment.Commit();
            }

            if (quotation.Document.DocumentUser.Count > 0)
            {
                foreach (var item in quotation.Document.DocumentUser)
                {
                    var dbDocumentUser = new Com.BudgetMetal.DBEntities.DocumentUser();

                    Copy<VmDocumentUserItem, Com.BudgetMetal.DBEntities.DocumentUser>(item, dbDocumentUser);
                    dbDocumentUser.Document_Id = dbDocument.Id;
                    dbDocumentUser.CreatedBy = dbDocumentUser.UpdatedBy = dbQuotation.CreatedBy;
                    repoDocumentUser.Add(dbDocumentUser);
                }
                repoDocumentUser.Commit();
            }

            if (quotation.QuotationPriceSchedule.Count > 0)
            {
                foreach (var item in quotation.QuotationPriceSchedule)
                {
                    var dbQPriceSchedule = new Com.BudgetMetal.DBEntities.QuotationPriceSchedule();

                    Copy<VmQuotationPriceScheduleItem, Com.BudgetMetal.DBEntities.QuotationPriceSchedule>(item, dbQPriceSchedule);
                    dbQPriceSchedule.Quotation_Id = dbQuotation.Id;
                    dbQPriceSchedule.CreatedBy = dbQPriceSchedule.UpdatedBy = dbQuotation.CreatedBy;
                    repoPriceSchedule.Add(dbQPriceSchedule);

                }
                repoPriceSchedule.Commit();
            }

            return documentNo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quotation"></param>
        /// <returns></returns>
        public string UpdateQuotation(VmQuotationItem quotation)
        {
            var dbDocument = new Com.BudgetMetal.DBEntities.Document();
            Copy<VmDocumentItem, Com.BudgetMetal.DBEntities.Document>(quotation.Document, dbDocument);
            repoDocument.Update(dbDocument);
            repoDocument.Commit();

            quotation.Document_Id = dbDocument.Id;

            var dbQuotation = new Com.BudgetMetal.DBEntities.Quotation();
            Copy<VmQuotationItem, Com.BudgetMetal.DBEntities.Quotation>(quotation, dbQuotation);
            repoQuotation.Update(dbQuotation);
            repoQuotation.Commit();

            repoAttachment.InactiveByDocumentId(dbDocument.Id, dbDocument.UpdatedBy);
            repoAttachment.Commit();
            if (quotation.Document.Attachment.Count > 0)
            {
                foreach (var item in quotation.Document.Attachment)
                {
                    var dbAttachment = new Com.BudgetMetal.DBEntities.Attachment();

                    Copy<VmAttachmentItem, Com.BudgetMetal.DBEntities.Attachment>(item, dbAttachment);
                    dbAttachment.Document_Id = dbDocument.Id;
                    dbAttachment.CreatedBy = dbAttachment.UpdatedBy = dbQuotation.UpdatedBy;
                    if (dbAttachment.Id < 1)
                    {
                        repoAttachment.Add(dbAttachment);
                    }
                    else
                    {
                        repoAttachment.UpdateDescription(dbAttachment);
                    }

                }
                repoAttachment.Commit();
            }
            repoAttachment.DeleteByDocumentId(dbDocument.Id);

            repoDocumentUser.InactiveByDocumentId(dbDocument.Id, dbDocument.UpdatedBy);
            repoDocumentUser.Commit();

            if (quotation.Document.DocumentUser.Count > 0)
            {
                foreach (var item in quotation.Document.DocumentUser)
                {
                    var dbDocumentUser = new Com.BudgetMetal.DBEntities.DocumentUser();

                    Copy<VmDocumentUserItem, Com.BudgetMetal.DBEntities.DocumentUser>(item, dbDocumentUser);
                    dbDocumentUser.Document_Id = dbDocument.Id;
                    dbDocumentUser.CreatedBy = dbDocumentUser.UpdatedBy = dbQuotation.UpdatedBy;
                    repoDocumentUser.Add(dbDocumentUser);
                }
                repoDocumentUser.Commit();
            }


           

            repoPriceSchedule.InactiveByQuotationId(dbQuotation.Id, dbQuotation.UpdatedBy);
            repoPriceSchedule.Commit();
            if (quotation.QuotationPriceSchedule.Count > 0)
            {
                foreach (var item in quotation.QuotationPriceSchedule)
                {
                    var dbQPriceSchedule = new Com.BudgetMetal.DBEntities.QuotationPriceSchedule();

                    Copy<VmQuotationPriceScheduleItem, Com.BudgetMetal.DBEntities.QuotationPriceSchedule>(item, dbQPriceSchedule);
                    dbQPriceSchedule.Quotation_Id = dbQuotation.Id;
                    dbQPriceSchedule.CreatedBy = dbQPriceSchedule.UpdatedBy = dbQuotation.CreatedBy;
                    repoPriceSchedule.Add(dbQPriceSchedule);

                }
                repoPriceSchedule.Commit();
            }


            return quotation.Document.DocumentNo;
        }

        private string GenerateDocumentNo(int companyId)
        {
            int countRfq = repoDocument.GetQuotationCountByCompany(companyId);
            countRfq = countRfq + 1;
            string documentNo = "Quotation_" + companyId.ToString().PadLeft(4, '0') + "_" + countRfq.ToString().PadLeft(4, '0');
            return documentNo;
        }


        public bool CheckQuotationLimit(int companyId)
        {
            string currentWeek = GetCurrentWeek();
            int documentCount = repoDocument.GetQuotationCountByCompanyAndWorkingPeriod(companyId, currentWeek);


            var dbResult = repoCompany.Get(companyId);

            int QuoLimitPerWeek = (dbResult.Result.MaxQuotationPerWeek == null) ? 0 : Convert.ToInt32(dbResult.Result.MaxQuotationPerWeek);

            bool QuotationLimit = true;

            if (documentCount >= QuoLimitPerWeek)
            {
                QuotationLimit = false;
            }

            return QuotationLimit;
        }

        public async Task<VmQuotationItem> GetSingleQuotationById(int id)
        {
            var dbResult = await repoQuotation.GetSingleQuotationById(id);

            var resultObject = new VmQuotationItem();

            Copy<Com.BudgetMetal.DBEntities.Quotation, VmQuotationItem>(dbResult, resultObject, new string[] { "Document","QuotationPriceSchedule" });

            var resultDocument = new VmDocumentItem();

            Copy<Com.BudgetMetal.DBEntities.Document, VmDocumentItem>(dbResult.Document, resultDocument, new string[] { "DocumentStatus", "DocumentType", "Attachment" });

            var documentStaus = new VmCodeTableItem
            {
                Id = dbResult.Document.DocumentStatus.Id,
                Name = dbResult.Document.DocumentStatus.Name
            };
            resultDocument.DocumentStatus = documentStaus;

            var documentType = new VmCodeTableItem
            {
                Id = dbResult.Document.DocumentType.Id,
                Name = dbResult.Document.DocumentType.Name
            };
            resultDocument.DocumentType = documentType;

            var listAttachment = new List<VmAttachmentItem>();
            if (dbResult.Document.Attachment != null)
            {
                foreach (var item in dbResult.Document.Attachment)
                {
                    var itemAttachment = new VmAttachmentItem();
                    Copy<Com.BudgetMetal.DBEntities.Attachment, VmAttachmentItem>(item, itemAttachment, new string[] { "Document", "FileBinary" });
                    listAttachment.Add(itemAttachment);
                }
            }
            resultDocument.Attachment = listAttachment;

            var listDocumentUser = new List<VmDocumentUserDisplay>();
            if (dbResult.Document.DocumentUser != null)
            {
                List<int> UserList = new List<int>();
                UserList = dbResult.Document.DocumentUser.Select(e => e.User_Id).Distinct().ToList();
                foreach (var itemUser in UserList)
                {
                    var documentUser = new VmDocumentUserDisplay();
                    documentUser.User_Id = itemUser;

                    var user = new User();
                    user = await repoUser.Get(itemUser);
                    documentUser.UserName = user.ContactName;

                    documentUser.Roles_Id = string.Join(",", dbResult.Document.DocumentUser.Where(e => e.User_Id == itemUser).Select(e => e.Role_Id).ToArray());

                    var roles = new List<string>();
                    foreach (var roleId in dbResult.Document.DocumentUser.Where(e => e.User_Id == itemUser).Select(e => e.Role_Id).ToList())
                    {
                        var role = new Role();
                        role = await repoRole.Get(roleId);
                        roles.Add(role.Name);
                    }
                    documentUser.Roles = roles;
                    listDocumentUser.Add(documentUser);
                }
            }
            resultDocument.DocumentUserDisplay = listDocumentUser;

            resultObject.Document = resultDocument;
            

            var listQuotationPriceSchedule = new List<VmQuotationPriceScheduleItem>();
            if (dbResult.QuotationPriceSchedule != null)
            {
                foreach (var item in dbResult.QuotationPriceSchedule)
                {
                    var itemQuotationPriceSchedule = new VmQuotationPriceScheduleItem();
                    Copy<Com.BudgetMetal.DBEntities.QuotationPriceSchedule, VmQuotationPriceScheduleItem>(item, itemQuotationPriceSchedule, new string[] { "Quotation" });
                    listQuotationPriceSchedule.Add(itemQuotationPriceSchedule);
                }
            }
            resultObject.QuotationPriceSchedule = listQuotationPriceSchedule;


            var dbResultRFQ = await repoRfq.GetSingleRfqById(resultObject.Rfq_Id);

            var resultRfq = new VmRfqItem();

            Copy<Com.BudgetMetal.DBEntities.Rfq, VmRfqItem>(dbResultRFQ, resultRfq, new string[] { "Document", "InvitedSupplier", "Penalty", "Requirement", "RfqPriceSchedule", "Sla" });

            var resultDocumentRFQ = new VmDocumentItem();

            Copy<Com.BudgetMetal.DBEntities.Document, VmDocumentItem>(dbResultRFQ.Document, resultDocumentRFQ, new string[] { "DocumentStatus", "DocumentType", "Attachment", "DocumentUser", "DocumentStatus", "DocumentType", "Company" });

            resultRfq.Document = resultDocumentRFQ;

            resultObject.Rfq = resultRfq;

            return resultObject;
        }
    }
}
