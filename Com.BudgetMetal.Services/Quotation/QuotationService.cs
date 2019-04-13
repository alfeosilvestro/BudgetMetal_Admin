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
using Com.BudgetMetal.DataRepository.QuotationRequirement;
using Com.BudgetMetal.ViewModels.QuotationRequirement;
using Com.BudgetMetal.DataRepository.DocumentActivity;
using Com.BudgetMetal.ViewModels.DocumentActivity;
using Com.BudgetMetal.DataRepository.TimeLine;
using Com.BudgetMetal.DataRepository.Clarification;
using Com.BudgetMetal.ViewModels.Clarification;
using Com.BudgetMetal.ViewModels.QuotationCommercial;
using Com.BudgetMetal.ViewModels.QuotationSupport;
using Com.BudgetMetal.DataRepository.QuotationSupport;
using Com.BudgetMetal.DataRepository.QuotationCommercial;
using Com.BudgetMetal.DataRepository.EmailTemplates;

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
        private readonly IQuotationRequirementRepository repoRequirement;
        private readonly IQuotationSupportRepository repoSupport;
        private readonly IQuotationCommercialRepository repoCommercial;
        private readonly IDocumentActivityRepository repoDocumentActivity;
        private readonly ITimeLineRepository repoTimeLine;
        private readonly ICompanyRepository repoCompany;
        private readonly IUserRepository repoUser;
        private readonly IRoleRepository repoRole;
        private readonly IClarificationRepository repoClarification;
        private readonly IEmailTemplateRepository repoEmailTemplate;

        public QuotationService(IRfqRepository repoRfq, IDocumentRepository repoDocument, IQuotationRepository repoQuotation, IAttachmentRepository repoAttachment, IDocumentUserRepository repoDocumentUser, IQuotationPriceScheduleRepository repoPriceSchedule, IUserRepository repoUser, IRoleRepository repoRole, IQuotationRequirementRepository repoQuotationRequirement, IQuotationSupportRepository repoSupport, IQuotationCommercialRepository repoCommercial, IDocumentActivityRepository repoDocumentActivity, ICompanyRepository repoCompany, ITimeLineRepository repoTimeLine, IClarificationRepository repoClarification,
            IEmailTemplateRepository repoEmailTemplate)
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
            this.repoRequirement = repoQuotationRequirement;
            this.repoSupport = repoSupport;
            this.repoCommercial = repoCommercial;
            this.repoDocumentActivity = repoDocumentActivity;
            this.repoTimeLine = repoTimeLine;
            this.repoClarification = repoClarification;
            this.repoEmailTemplate = repoEmailTemplate;
        }

        public async Task<VmQuotationPage> GetQuotationByPage(int userId, int companyId, int page, int totalRecords, bool isCompany, int statusId = 0, string keyword = "")
        {
            var dbPageResult = await repoQuotation.GetQuotationByPage(userId, companyId,
                (page == 0 ? Constants.app_firstPage : page),
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords), isCompany, statusId, keyword);

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
                    if (dbItem.Rfq != null)
                    {
                        var documentEntity = repoDocument.Get(dbItem.Rfq.Document_Id);
                        if (documentEntity != null)
                        {
                            resultItem.Rfq = new VmRfqItem()
                            {
                                ValidRfqdate = resultItem.ValidToDate,
                                Document = new VmDocumentItem()
                                {
                                    DocumentNo = documentEntity.Result.DocumentNo,
                                }
                            };
                        }
                    }

                    resultItem.Document = new ViewModels.Document.VmDocumentItem()
                    {
                        DocumentNo = dbItem.Document.DocumentNo,
                        UpdatedDate = dbItem.Document.UpdatedDate,
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
                            Id = dbItem.Document.Company_Id,
                            Name = dbItem.Document.Company.Name
                        }
                    };

                }

                resultObj.Result.Records.Add(resultItem);
            }

            return resultObj;
        }

        public async Task<VmGenericServiceResult> CancelQuotation(int documentId, int userId, string userName)
        {
            var result = new VmGenericServiceResult();
            try
            {
                //Change Status
                var dbDocument = await repoDocument.Get(documentId);
                dbDocument.DocumentStatus_Id = Constants_CodeTable.Code_Quotation_Cancelled;
                dbDocument.UpdatedBy = userName;
                repoDocument.Update(dbDocument);

                //Add Document Activity
                var dbDocumentActivity = new Com.BudgetMetal.DBEntities.DocumentActivity()
                {
                    Action = "Cancel",
                    IsRfq = false,
                    User_Id = userId,
                    Document_Id = documentId,
                    CreatedBy = userName,
                    UpdatedBy = userName
                };
                repoDocumentActivity.Add(dbDocumentActivity);
                repoDocumentActivity.Commit();

                //Add Timeline
                var timeline = new Com.BudgetMetal.DBEntities.TimeLine()
                {
                    Company_Id = dbDocument.Company_Id,
                    User_Id = userId,
                    Message = "Document " + dbDocument.DocumentNo + " is successfully cancelled.",
                    MessageType = Constants_CodeTable.Code_TM_Quotation,
                    IsRead = false,
                    Document_Id = dbDocument.Id,
                    CreatedBy = userName,
                    UpdatedBy = userName
                };
                repoTimeLine.Add(timeline);
                repoTimeLine.Commit();

                var buyerId = repoQuotation.GetRfqOwnerId(documentId);

                var timelineforbuyer = new Com.BudgetMetal.DBEntities.TimeLine()
                {
                    Company_Id = buyerId,
                    User_Id = userId,
                    Message = "Cancel quotation for RFQ ",
                    MessageType = Constants_CodeTable.Code_TM_Quotation,
                    IsRead = false,
                    Document_Id = documentId,
                    CreatedBy = userName,
                    UpdatedBy = userName
                };
                repoTimeLine.Add(timelineforbuyer);

                repoTimeLine.Commit();
                //end adding timeline

                //get rfq owner admin email
                var resultBuyerAdmin = repoUser.GetBuyerAdmin(buyerId);
                var sendMail = new SendingMail();
                if (resultBuyerAdmin != null)
                {
                    string emailSubject = "Cancel Quotation for RFQ.";
                    string emailBody = "Email Template need to provide.";
                    foreach (var item in resultBuyerAdmin)
                    {
                        sendMail.SendMail(item, "", emailSubject, emailBody);
                    }
                }


                result.IsSuccess = true;
                result.MessageToUser = "Your Rfq is successfully updated.";
            }
            catch
            {
                result.IsSuccess = false;
                result.MessageToUser = "Your quotation is failed to update.";
            }

            return result;

        }

        public async Task<VmGenericServiceResult> DecideQuotation(int documentId, int userId, string userName, bool isAccept)
        {
            var result = new VmGenericServiceResult();
            try
            {
                string emailSubject = "";
                string documentAction = "";
                string timelineMessage = "";
                //Change Status
                var dbDocument = await repoDocument.Get(documentId);
                if (isAccept)
                {
                    dbDocument.DocumentStatus_Id = Constants_CodeTable.Code_Quotation_Accepted;
                    emailSubject = "Quotation Accepted.";
                    documentAction = "Accepted";
                    timelineMessage = "Document " + dbDocument.DocumentNo + " is accepted.";
                }
                else
                {
                    dbDocument.DocumentStatus_Id = Constants_CodeTable.Code_Quotation_Rejected;
                    emailSubject = "Quotation Rejected";
                    documentAction = "Rejected";
                    timelineMessage = "Document " + dbDocument.DocumentNo + " is rejected.";
                }
                dbDocument.UpdatedBy = userName;
                repoDocument.Update(dbDocument);

                //Add Document Activity
                var dbDocumentActivity = new Com.BudgetMetal.DBEntities.DocumentActivity()
                {
                    Action = documentAction,
                    IsRfq = false,
                    User_Id = userId,
                    Document_Id = documentId,
                    CreatedBy = userName,
                    UpdatedBy = userName
                };
                repoDocumentActivity.Add(dbDocumentActivity);
                repoDocumentActivity.Commit();

                //Add Timeline
                var timeline = new Com.BudgetMetal.DBEntities.TimeLine()
                {
                    Company_Id = dbDocument.Company_Id,
                    User_Id = userId,
                    Message = timelineMessage,
                    MessageType = Constants_CodeTable.Code_TM_Quotation,
                    IsRead = false,
                    Document_Id = dbDocument.Id,
                    CreatedBy = userName,
                    UpdatedBy = userName
                };
                repoTimeLine.Add(timeline);
                repoTimeLine.Commit();

                var buyerId = repoQuotation.GetRfqOwnerId(documentId);

                var timelineforbuyer = new Com.BudgetMetal.DBEntities.TimeLine()
                {
                    Company_Id = buyerId,
                    User_Id = userId,
                    Message =  timelineMessage,
                    MessageType = Constants_CodeTable.Code_TM_Quotation,
                    IsRead = false,
                    Document_Id = documentId,
                    CreatedBy = userName,
                    UpdatedBy = userName
                };
                repoTimeLine.Add(timelineforbuyer);

                repoTimeLine.Commit();
                //end adding timeline

                //get rfq owner admin email
                var resultBuyerAdmin = repoUser.GetBuyerAdmin(buyerId);
                var sendMail = new SendingMail();
                if (resultBuyerAdmin != null)
                {
                    
                    string emailBody = "Email Template need to provide.";
                    foreach (var item in resultBuyerAdmin)
                    {
                        sendMail.SendMail(item, "", emailSubject, emailBody);
                    }
                }
                //update status of rfq
                var rfqItem = await repoRfq.GetRfqByQuotation_DocumentId(documentId);
                var dbRfqDocument = await repoDocument.Get(rfqItem.Document_Id);
                if (isAccept)
                {
                    dbRfqDocument.DocumentStatus_Id = Constants_CodeTable.Code_Quotation_Accepted;
                    documentAction = "Awarded";
                }
                dbRfqDocument.UpdatedBy = userName;
                repoDocument.Update(dbRfqDocument);
                var dbRfqDocumentActivity = new Com.BudgetMetal.DBEntities.DocumentActivity()
                {
                    Action = documentAction,
                    IsRfq = false,
                    User_Id = userId,
                    Document_Id = rfqItem.Document_Id,
                    CreatedBy = userName,
                    UpdatedBy = userName
                };
                repoDocumentActivity.Add(dbRfqDocumentActivity);
                repoDocumentActivity.Commit();

                result.IsSuccess = true;
                result.MessageToUser = "Your Quotation is successfully updated.";
            }
            catch
            {
                result.IsSuccess = false;
                result.MessageToUser = "Your Rfq is failed to update.";
            }

            return result;

        }

        public async Task<VmQuotationPage> GetQuotationForBuyerByPage(int userId, int buyerId, int page, int totalRecords, bool isCompany, int statusId = 0, string keyword = "")
        {
            var dbPageResult = await repoQuotation.GetQuotationForBuyerByPage(userId, buyerId,
                 (page == 0 ? Constants.app_firstPage : page),
                 (totalRecords == 0 ? Constants.app_totalRecords : totalRecords), isCompany, statusId, keyword);

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

                if(dbItem.Rfq != null)
                {
                    var documentEntity = repoDocument.Get(dbItem.Rfq.Document_Id);
                    if(documentEntity != null)
                    {
                        resultItem.Rfq = new VmRfqItem()
                        {
                            ValidRfqdate = resultItem.ValidToDate,
                            Document = new VmDocumentItem()
                            {
                                DocumentNo = documentEntity.Result.DocumentNo,
                            }
                        };
                    }
                }

                if (dbItem.Document != null)
                {
                    resultItem.Document = new ViewModels.Document.VmDocumentItem()
                    {
                        DocumentNo = dbItem.Document.DocumentNo,
                        UpdatedDate = dbItem.Document.UpdatedDate,
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
                            Id = dbItem.Document.Company_Id,
                            Name = dbItem.Document.Company.Name
                        }
                    };

                }

                resultItem.QuotationPriceSchedule = new List<VmQuotationPriceScheduleItem>();
                if(dbItem.QuotationPriceSchedule != null)
                {
                    foreach(var item in dbItem.QuotationPriceSchedule)
                    {
                        var vmQPS = new VmQuotationPriceScheduleItem();

                        Copy<Com.BudgetMetal.DBEntities.QuotationPriceSchedule, VmQuotationPriceScheduleItem>(item, vmQPS);

                        resultItem.QuotationPriceSchedule.Add(vmQPS);
                    }
                }

                resultItem.QuotationSupport = new List<VmQuotationSupportItem>();
                if (dbItem.QuotationSupport != null)
                {
                    foreach (var item in dbItem.QuotationSupport)
                    {
                        var vmQPS = new VmQuotationSupportItem();

                        Copy<Com.BudgetMetal.DBEntities.QuotationSupport, VmQuotationSupportItem>(item, vmQPS);

                        resultItem.QuotationSupport.Add(vmQPS);
                    }
                }

                resultItem.QuotationCommercial = new List<VmQuotationCommercialItem>();
                if (dbItem.QuotationCommercial != null)
                {
                    foreach (var item in dbItem.QuotationCommercial)
                    {
                        var vmQPS = new VmQuotationCommercialItem();

                        Copy<Com.BudgetMetal.DBEntities.QuotationCommercial, VmQuotationCommercialItem>(item, vmQPS);

                        resultItem.QuotationCommercial.Add(vmQPS);
                    }
                }

                resultObj.Result.Records.Add(resultItem);
            }

            return resultObj;
        }

        public async Task<VmQuotationPage> GetQuotationByRfqId(int RfqId, int page, int totalRecords, int statusId = 0, string keyword = "")
        {
            var dbPageResult = await repoQuotation.GetQuotationByRfqId(RfqId,
                (page == 0 ? Constants.app_firstPage : page),
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords), statusId, keyword);

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
                        Id = dbItem.Document.Id,
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
                            Id = dbItem.Document.Company.Id,
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

                    Copy<Com.BudgetMetal.DBEntities.RfqPriceSchedule, VmQuotationPriceScheduleItem>(dbItem, resultPriceSchedule, new string[] { "Rfq_Id", "Rfq_Id", "ItemAmount", "Quotation_Id", "Id" });

                    listPriceSchdule.Add(resultPriceSchedule);
                }
            }
            resultObj.QuotationPriceSchedule = listPriceSchdule;


           



            var listRequirement = new List<VmQuotationRequirementItem>();

            if (dbResult.Requirement != null)
            {
                foreach (var dbItem in dbResult.Requirement)
                {
                    var resultRequirement = new VmQuotationRequirementItem();

                    Copy<Com.BudgetMetal.DBEntities.Requirement, VmQuotationRequirementItem>(dbItem, resultRequirement, new string[] { "Rfq_Id", "Rfq_Id", "Compliance", "SupplierDescription", "Quotation_Id", "Id" });

                    listRequirement.Add(resultRequirement);
                }
            }
            resultObj.QuotationRequirement = listRequirement;

            var listSupport = new List<VmQuotationSupportItem>();
            if (dbResult.Sla != null)
            {
                foreach (var dbItem in dbResult.Sla)
                {
                    var resultSupport = new VmQuotationSupportItem();

                    Copy<Com.BudgetMetal.DBEntities.Sla, VmQuotationSupportItem>(dbItem, resultSupport, new string[] { "Rfq_Id", "Rfq_Id", "Compliance", "SupplierDescription", "Quotation_Id", "Id" });
                    resultSupport.ServiceName = dbItem.Requirement;


                    listSupport.Add(resultSupport);
                }
            }
            resultObj.QuotationSupport = listSupport;

            var listCommercial = new List<VmQuotationCommercialItem>();
            if (dbResult.Penalty != null)
            {
                foreach (var dbItem in dbResult.Penalty)
                {
                    var resultCommercial = new VmQuotationCommercialItem();

                    Copy<Com.BudgetMetal.DBEntities.Penalty, VmQuotationCommercialItem>(dbItem, resultCommercial, new string[] { "Rfq_Id", "Rfq_Id", "Compliance", "SupplierDescription", "Quotation_Id", "Id" });
                    resultCommercial.ServiceName = dbItem.BreachOfServiceDefinition;

                    listCommercial.Add(resultCommercial);
                }
            }
            resultObj.QuotationCommercial = listCommercial;


            return resultObj;

        }

        public async Task<VmGenericServiceResult> SaveQuotation(VmQuotationItem quotation)
        {
            var result = new VmGenericServiceResult();
            try
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

                if (quotation.Document.Attachment != null)
                {
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
                }

                if (quotation.Document.DocumentUser != null)
                {
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
                }

                if (quotation.QuotationPriceSchedule != null)
                {
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
                }

                if (quotation.QuotationRequirement != null)
                {
                    if (quotation.QuotationRequirement.Count > 0)
                    {
                        foreach (var item in quotation.QuotationRequirement)
                        {
                            var dbQRequirement = new Com.BudgetMetal.DBEntities.QuotationRequirement();

                            Copy<VmQuotationRequirementItem, Com.BudgetMetal.DBEntities.QuotationRequirement>(item, dbQRequirement);
                            dbQRequirement.Quotation_Id = dbQuotation.Id;
                            dbQRequirement.CreatedBy = dbQRequirement.UpdatedBy = dbQuotation.CreatedBy;
                            repoRequirement.Add(dbQRequirement);
                        }
                        repoPriceSchedule.Commit();
                    }
                }

                if (quotation.QuotationSupport != null)
                {
                    if (quotation.QuotationSupport.Count > 0)
                    {
                        foreach (var item in quotation.QuotationSupport)
                        {
                            var dbQSupport = new Com.BudgetMetal.DBEntities.QuotationSupport();

                            Copy<VmQuotationSupportItem, Com.BudgetMetal.DBEntities.QuotationSupport>(item, dbQSupport);
                            dbQSupport.Quotation_Id = dbQuotation.Id;
                            dbQSupport.CreatedBy = dbQSupport.UpdatedBy = dbQuotation.CreatedBy;
                            repoSupport.Add(dbQSupport);
                        }
                        repoSupport.Commit();
                    }
                }

                if (quotation.QuotationCommercial != null)
                {
                    if (quotation.QuotationCommercial.Count > 0)
                    {
                        foreach (var item in quotation.QuotationCommercial)
                        {
                            var dbQCommercial = new Com.BudgetMetal.DBEntities.QuotationCommercial();

                            Copy<VmQuotationCommercialItem, Com.BudgetMetal.DBEntities.QuotationCommercial>(item, dbQCommercial);
                            dbQCommercial.Quotation_Id = dbQuotation.Id;
                            dbQCommercial.CreatedBy = dbQCommercial.UpdatedBy = dbQuotation.CreatedBy;
                            repoCommercial.Add(dbQCommercial);
                        }
                        repoSupport.Commit();
                    }
                }



                if (quotation.Document.DocumentActivityList != null)
                {
                    if (quotation.Document.DocumentActivityList.Count > 0)
                    {
                        foreach (var item in quotation.Document.DocumentActivityList)
                        {
                            var dbDocumentActivity = new Com.BudgetMetal.DBEntities.DocumentActivity();

                            Copy<VmDocumentActivityItem, Com.BudgetMetal.DBEntities.DocumentActivity>(item, dbDocumentActivity);
                            dbDocumentActivity.Document_Id = dbDocument.Id;
                            dbDocumentActivity.CreatedBy = dbDocumentActivity.UpdatedBy = quotation.CreatedBy;
                            repoDocumentActivity.Add(dbDocumentActivity);
                        }
                        repoDocumentActivity.Commit();
                    }
                }

                //start adding timeline
                var timeline = new Com.BudgetMetal.DBEntities.TimeLine()
                {
                    Company_Id = quotation.Document.Company_Id,
                    User_Id = quotation.Document.DocumentUser.FirstOrDefault().User_Id,
                    Message = "Your company interest on RFQ " + quotation.Rfq.Document.DocumentNo,
                    MessageType = Constants_CodeTable.Code_TM_Quotation,
                    IsRead = false,
                    Document_Id = dbDocument.Id,
                    CreatedBy = dbQuotation.CreatedBy,
                    UpdatedBy = dbQuotation.CreatedBy
                };
                repoTimeLine.Add(timeline);

                var timelineforbuyer = new Com.BudgetMetal.DBEntities.TimeLine()
                {
                    Company_Id = quotation.Rfq.Document.Company_Id,
                    User_Id = quotation.Document.DocumentUser.FirstOrDefault().User_Id,
                    Message = "Interest on RFQ " + quotation.Rfq.Document.DocumentNo,
                    MessageType = Constants_CodeTable.Code_TM_Rfq,
                    IsRead = false,
                    Document_Id = quotation.Rfq.Document.Id,
                    CreatedBy = dbQuotation.CreatedBy,
                    UpdatedBy = dbQuotation.CreatedBy
                };
                repoTimeLine.Add(timelineforbuyer);

                repoTimeLine.Commit();
                //end adding timeline

                //get rfq owner admin email
                var rfq = await repoRfq.GetSingleRfqById(quotation.Rfq.Id);
                var BuyerOwnerList = rfq.Document.DocumentUser.Where(e => e.Role_Id == Constants.RFQCreatorRoleId).ToList();
                foreach(var BuyerOwner in BuyerOwnerList)
                {
                    var dbUser = await repoUser.Get(BuyerOwner.User_Id);
                    var sendMail = new SendingMail();
                    
                    var SupplierCompany = await repoCompany.Get(quotation.Document.Company_Id);
                    var emailTemplate = await repoEmailTemplate.GetEmailTemplateByPurpose("RFQ_Interested");
                    string emailSubject = emailTemplate.EmailSubject.Replace("[ProjectTitle]", rfq.Document.Title).Replace("[SupplierCompanyX]", SupplierCompany.Name);
                    string emailBody = emailTemplate.EmailContent;

                    emailBody = emailBody.Replace("[BuyerOwnerX]", dbUser.ContactName);
                    emailBody = emailBody.Replace("[ProjectTitle]", rfq.Document.Title);
                    emailBody = emailBody.Replace("[SupplierCompanyX]", SupplierCompany.Name);
                    emailBody = emailBody.Replace("[RedirectLink]", rfq.Id.ToString());
                    
                    sendMail.SendMail(dbUser.EmailAddress, "", emailSubject, emailBody);
                }
                result.IsSuccess = true;
                result.MessageToUser = dbQuotation.Id.ToString();
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.MessageToUser = "Fail to create your quotation. Please contact site admin.";
                result.Error = ex;
            }
            return result;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quotation"></param>
        /// <returns></returns>
        public async Task<VmGenericServiceResult> UpdateQuotation(VmQuotationItem quotation)
        {
            var result = new VmGenericServiceResult();
            try
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

                if (quotation.Document.Attachment != null)
                {
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
                }

                repoAttachment.DeleteByDocumentId(dbDocument.Id);

                repoDocumentUser.InactiveByDocumentId(dbDocument.Id, dbDocument.UpdatedBy);
                repoDocumentUser.Commit();

                if (quotation.Document.DocumentUser != null)
                {
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
                }

                repoPriceSchedule.InactiveByQuotationId(dbQuotation.Id, dbQuotation.UpdatedBy);
                repoPriceSchedule.Commit();

                if (quotation.QuotationPriceSchedule != null)
                {
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
                }

                repoRequirement.InactiveByQuotationId(dbQuotation.Id, dbQuotation.UpdatedBy);
                repoRequirement.Commit();

                if (quotation.QuotationRequirement != null)
                {
                    if (quotation.QuotationRequirement.Count > 0)
                    {
                        foreach (var item in quotation.QuotationRequirement)
                        {
                            var dbQRequirement = new Com.BudgetMetal.DBEntities.QuotationRequirement();

                            Copy<VmQuotationRequirementItem, Com.BudgetMetal.DBEntities.QuotationRequirement>(item, dbQRequirement);
                            dbQRequirement.Quotation_Id = dbQuotation.Id;
                            dbQRequirement.CreatedBy = dbQRequirement.UpdatedBy = dbQuotation.CreatedBy;
                            repoRequirement.Add(dbQRequirement);
                        }
                        repoPriceSchedule.Commit();
                    }
                }

                repoSupport.InactiveByQuotationId(dbQuotation.Id, dbQuotation.UpdatedBy);
                repoSupport.Commit();

                if (quotation.QuotationSupport != null)
                {
                    if (quotation.QuotationSupport.Count > 0)
                    {
                        foreach (var item in quotation.QuotationSupport)
                        {
                            var dbQSupport = new Com.BudgetMetal.DBEntities.QuotationSupport();

                            Copy<VmQuotationSupportItem, Com.BudgetMetal.DBEntities.QuotationSupport>(item, dbQSupport);
                            dbQSupport.Quotation_Id = dbQuotation.Id;
                            dbQSupport.CreatedBy = dbQSupport.UpdatedBy = dbQuotation.CreatedBy;
                            repoSupport.Add(dbQSupport);
                        }
                        repoSupport.Commit();
                    }
                }

                repoCommercial.InactiveByQuotationId(dbQuotation.Id, dbQuotation.UpdatedBy);
                repoCommercial.Commit();

                if (quotation.QuotationCommercial != null)
                {
                    if (quotation.QuotationCommercial.Count > 0)
                    {
                        foreach (var item in quotation.QuotationCommercial)
                        {
                            var dbQCommercial = new Com.BudgetMetal.DBEntities.QuotationCommercial();

                            Copy<VmQuotationCommercialItem, Com.BudgetMetal.DBEntities.QuotationCommercial>(item, dbQCommercial);
                            dbQCommercial.Quotation_Id = dbQuotation.Id;
                            dbQCommercial.CreatedBy = dbQCommercial.UpdatedBy = dbQuotation.CreatedBy;
                            repoCommercial.Add(dbQCommercial);
                        }
                        repoCommercial.Commit();
                    }
                }

                if (quotation.Document.DocumentActivityList != null)
                {
                    if (quotation.Document.DocumentActivityList.Count > 0)
                    {
                        foreach (var item in quotation.Document.DocumentActivityList)
                        {
                            var dbDocumentActivity = new Com.BudgetMetal.DBEntities.DocumentActivity();

                            Copy<VmDocumentActivityItem, Com.BudgetMetal.DBEntities.DocumentActivity>(item, dbDocumentActivity);
                            dbDocumentActivity.Document_Id = quotation.Document_Id;
                            dbDocumentActivity.CreatedBy = dbDocumentActivity.UpdatedBy = quotation.CreatedBy;
                            repoDocumentActivity.Add(dbDocumentActivity);
                        }
                        repoDocumentActivity.Commit();
                    }
                }

                //start adding timeline
                var timeline = new Com.BudgetMetal.DBEntities.TimeLine()
                {
                    Company_Id = quotation.Document.Company_Id,
                    User_Id = quotation.Document.DocumentUser.FirstOrDefault().User_Id,
                    Message = "Updated on quotation " + quotation.Document.DocumentNo,
                    MessageType = Constants_CodeTable.Code_TM_Quotation,
                    IsRead = false,
                    Document_Id = dbDocument.Id,
                    CreatedBy = dbQuotation.CreatedBy,
                    UpdatedBy = dbQuotation.CreatedBy
                };
                repoTimeLine.Add(timeline);
                repoTimeLine.Commit();

                if (quotation.Document.DocumentStatus_Id == Constants_CodeTable.Code_Quotation_Submitted)
                {
                    var timelineforbuyer = new Com.BudgetMetal.DBEntities.TimeLine()
                    {
                        Company_Id = quotation.Rfq.Document.Company_Id,
                        User_Id = quotation.Document.DocumentUser.FirstOrDefault().User_Id,
                        Message = "Quotation " + quotation.Document.DocumentNo + " has been submitted.",
                    MessageType = Constants_CodeTable.Code_TM_Rfq,
                        IsRead = false,
                        Document_Id = quotation.Rfq.Document_Id,
                        CreatedBy = dbQuotation.CreatedBy,
                        UpdatedBy = dbQuotation.CreatedBy
                    };
                    repoTimeLine.Add(timelineforbuyer);

                    repoTimeLine.Commit();
                    //end adding timeline

                    //get rfq owner admin email
                    var rfq = await repoRfq.GetSingleRfqById(quotation.Rfq_Id);
                    var BuyerOwnerList = rfq.Document.DocumentUser.Where(e => e.Role_Id == Constants.RFQCreatorRoleId).ToList();
                    foreach (var BuyerOwner in BuyerOwnerList)
                    {
                        var dbUser = await repoUser.Get(BuyerOwner.User_Id);
                        var sendMail = new SendingMail();
                        var BuyerUser = await repoUser.Get(BuyerOwner.User_Id);
                        var supplierCompany = await repoCompany.Get(quotation.Document.Company_Id);
                        var emailTemplate = await repoEmailTemplate.GetEmailTemplateByPurpose("Quotation_Received");
                        string emailSubject = emailTemplate.EmailSubject.Replace("[ProjectTitle]", rfq.Document.Title).Replace("[SupplierCompanyX]", supplierCompany.Name);
                        string emailBody = emailTemplate.EmailContent;

                        emailBody = emailBody.Replace("[BuyerOwnerX]", dbUser.ContactName);
                        emailBody = emailBody.Replace("[ProjectTitle]", rfq.Document.Title);
                        emailBody = emailBody.Replace("[SupplierCompanyX]", supplierCompany.Name);
                        emailBody = emailBody.Replace("[RedirectLink]", dbQuotation.Id.ToString());

                        sendMail.SendMail(dbUser.EmailAddress, "", emailSubject, emailBody);
                    }
                }
                

                result.IsSuccess = true;
                result.MessageToUser = dbQuotation.Id.ToString();
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.MessageToUser = "Fail to create your RFQ. Please contact site admin.";
                result.Error = ex;
            }
            return result;
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

            Copy<Com.BudgetMetal.DBEntities.Quotation, VmQuotationItem>(dbResult, resultObject, new string[] { "Document", "QuotationPriceSchedule" });

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
                foreach (var item in dbResult.Document.Attachment.Where(e => e.IsActive == true).ToList())
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
                UserList = dbResult.Document.DocumentUser.Where(e => e.IsActive == true).Select(e => e.User_Id).Distinct().ToList();
                foreach (var itemUser in UserList)
                {
                    var documentUser = new VmDocumentUserDisplay();
                    documentUser.User_Id = itemUser;

                    var user = new User();
                    user = await repoUser.Get(itemUser);
                    documentUser.UserName = user.ContactName;

                    documentUser.Roles_Id = string.Join(",", dbResult.Document.DocumentUser.Where(e => e.User_Id == itemUser && e.IsActive == true).Select(e => e.Role_Id).ToArray());

                    var roles = new List<string>();
                    foreach (var roleId in dbResult.Document.DocumentUser.Where(e => e.User_Id == itemUser && e.IsActive == true).Select(e => e.Role_Id).ToList())
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
                foreach (var item in dbResult.QuotationPriceSchedule.Where(e => e.IsActive == true).ToList())
                {
                    var itemQuotationPriceSchedule = new VmQuotationPriceScheduleItem();
                    Copy<Com.BudgetMetal.DBEntities.QuotationPriceSchedule, VmQuotationPriceScheduleItem>(item, itemQuotationPriceSchedule, new string[] { "Quotation" });
                    listQuotationPriceSchedule.Add(itemQuotationPriceSchedule);
                }
            }
            resultObject.QuotationPriceSchedule = listQuotationPriceSchedule;

            var listQuotationRequirement = new List<VmQuotationRequirementItem>();
            if (dbResult.QuotationRequirement != null)
            {
                foreach (var item in dbResult.QuotationRequirement.Where(e => e.IsActive == true).ToList())
                {
                    var itemQuotationRequirement = new VmQuotationRequirementItem();
                    Copy<Com.BudgetMetal.DBEntities.QuotationRequirement, VmQuotationRequirementItem>(item, itemQuotationRequirement, new string[] { "Quotation" });
                    listQuotationRequirement.Add(itemQuotationRequirement);
                }
            }
            resultObject.QuotationRequirement = listQuotationRequirement;

            var listQuotationCommercial = new List<VmQuotationCommercialItem>();
            if (dbResult.QuotationRequirement != null)
            {
                foreach (var item in dbResult.QuotationCommercial.Where(e => e.IsActive == true).ToList())
                {
                    var itemQuotationCommercial = new VmQuotationCommercialItem();
                    Copy<Com.BudgetMetal.DBEntities.QuotationCommercial, VmQuotationCommercialItem>(item, itemQuotationCommercial, new string[] { "Quotation" });
                    listQuotationCommercial.Add(itemQuotationCommercial);
                }
            }
            resultObject.QuotationCommercial = listQuotationCommercial;


            var listQuotationSupport = new List<VmQuotationSupportItem>();
            if (dbResult.QuotationRequirement != null)
            {
                foreach (var item in dbResult.QuotationSupport.Where(e => e.IsActive == true).ToList())
                {
                    var itemQuotationSupport = new VmQuotationSupportItem();
                    Copy<Com.BudgetMetal.DBEntities.QuotationSupport, VmQuotationSupportItem>(item, itemQuotationSupport, new string[] { "Quotation" });
                    listQuotationSupport.Add(itemQuotationSupport);
                }
            }
            resultObject.QuotationSupport = listQuotationSupport;


            var dbResultRFQ = await repoRfq.GetSingleRfqById(resultObject.Rfq_Id);

            var resultRfq = new VmRfqItem();

            Copy<Com.BudgetMetal.DBEntities.Rfq, VmRfqItem>(dbResultRFQ, resultRfq, new string[] { "Document", "InvitedSupplier", "Penalty", "Requirement", "RfqPriceSchedule", "Sla" });

            var resultDocumentRFQ = new VmDocumentItem();

            Copy<Com.BudgetMetal.DBEntities.Document, VmDocumentItem>(dbResultRFQ.Document, resultDocumentRFQ, new string[] { "DocumentStatus", "DocumentType", "Attachment", "DocumentUser", "DocumentStatus", "DocumentType", "Company" });

            resultRfq.Document = resultDocumentRFQ;

            resultObject.Rfq = resultRfq;


            var listDocumentActivity = new List<VmDocumentActivityItem>();
            if (dbResult.Document.DocumentActivity != null)
            {
                foreach (var item in dbResult.Document.DocumentActivity)
                {
                    var newItem = new VmDocumentActivityItem();
                    newItem.Action = item.Action;
                    newItem.CreatedBy = item.CreatedBy;
                    newItem.CreatedDate = item.CreatedDate;
                    newItem.Document_Id = item.Document_Id;
                    listDocumentActivity.Add(newItem);
                }
            }
            resultObject.Document.DocumentActivityList = listDocumentActivity;

            resultObject.Document.ClarificationList = new List<VmClarificationItem>();
            if (dbResult.Document.Clarification != null)
            {
                foreach (var item in dbResult.Document.Clarification.Where(e => e.IsActive == true).ToList())
                {
                    var newItem = new VmClarificationItem();
                    Copy<Com.BudgetMetal.DBEntities.Clarification, VmClarificationItem>(item, newItem);

                    var user = await repoUser.Get(item.User_Id);
                    var userModel = new VmUserItem();
                    Copy<Com.BudgetMetal.DBEntities.User, VmUserItem>(user, userModel);

                    newItem.User = userModel;

                    resultObject.Document.ClarificationList.Add(newItem);
                }
            }

            return resultObject;
        }

        public async Task<VmGenericServiceResult> AddClarification(int documentId, int userId, string userName, string clarification, int commentId)
        {
            var result = new VmGenericServiceResult();
            try
            {
                var dbClarification = new Com.BudgetMetal.DBEntities.Clarification
                {
                    Document_Id = documentId,
                    User_Id = userId,
                    ClarificationQuestion = clarification,
                    Clarification_Id = commentId,
                    CreatedBy = userName,
                    UpdatedBy = userName
                };

                repoClarification.Add(dbClarification);
                repoClarification.Commit();

                var dbDocument = await repoDocument.Get(documentId);

                //Add Timeline
                var timeline = new Com.BudgetMetal.DBEntities.TimeLine()
                {
                    Company_Id = dbDocument.Company_Id,
                    User_Id = userId,
                    Message = userName + " has added clarification in Document " + dbDocument.DocumentNo + ".",
                    MessageType = Constants_CodeTable.Code_TM_Rfq,
                    IsRead = false,
                    Document_Id = dbDocument.Id,
                    CreatedBy = userName,
                    UpdatedBy = userName
                };
                repoTimeLine.Add(timeline);
                repoTimeLine.Commit();

                result.IsSuccess = true;
                result.MessageToUser = dbClarification.Id.ToString();
            }
            catch
            {
                result.IsSuccess = false;
                result.MessageToUser = "You have failed to add clarification.";
            }

            return result;
        }

        public async Task<VmGenericServiceResult> CheckPermissionForQuotation(int companyId, int C_BussinessType, int userId, int QuotationId, bool companyAdmin)
        {
            var result = new VmGenericServiceResult();

            var quotation = await repoQuotation.GetSingleQuotationById(QuotationId);

            result.IsSuccess = false;
            if (quotation != null)
            {
                if (C_BussinessType == Constants_CodeTable.Code_C_Supplier)
                {
                    if (quotation.Document.Company_Id == companyId)
                    {
                         result.IsSuccess = true;
                    }
                }
                else if (C_BussinessType == Constants_CodeTable.Code_C_Buyer)
                {
                    var buyerId = repoQuotation.GetRfqOwnerId(quotation.Document_Id);

                    if (companyId == buyerId)
                    {
                        result.IsSuccess = true;
                    }
                }
            }
            return result;
        }

    }
}
