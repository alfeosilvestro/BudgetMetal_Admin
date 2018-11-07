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
using Com.BudgetMetal.DataRepository.Company;
using Com.BudgetMetal.DataRepository.DocumentActivity;
using Com.BudgetMetal.ViewModels.DocumentActivity;
using Com.BudgetMetal.DataRepository.Quotation;
using Com.BudgetMetal.DataRepository.TimeLine;
using Com.BudgetMetal.ViewModels.Company;
using Com.BudgetMetal.DataRepository.CompanySupplier;
using Com.BudgetMetal.DataRepository.Clarification;
using Com.BudgetMetal.ViewModels.Clarification;
using Com.BudgetMetal.ViewModels.Industries;
using Com.BudgetMetal.DataRepository.Industries;

namespace Com.BudgetMetal.Services.RFQ
{
    public class RFQService : BaseService, IRFQService
    {
        private readonly IDocumentRepository repoDocument;
        private readonly IRfqRepository repoRfq;
        private readonly IAttachmentRepository repoAttachment;
        private readonly IRequirementRepository repoRequirement;
        private readonly ISlaRepository repoSla;
        private readonly IRfqPriceScheduleRepository repoRfqPriceSchedule;
        private readonly IPenaltyRepository repoPenalty;
        private readonly IInvitedSupplierRepository repoInvitedSupplier;
        private readonly IDocumentUserRepository repoDocumentUser;
        private readonly IUserRepository repoUser;
        private readonly IRoleRepository repoRole;
        private readonly ICompanyRepository repoCompany;
        private readonly IDocumentActivityRepository repoDocumentActivity;
        private readonly IQuotationRepository repoQuotation;
        private readonly ITimeLineRepository repoTimeLine;
        private readonly ICompanySupplierRepository repoCompanySupplier;
        private readonly IClarificationRepository repoClarification;
        private readonly IIndustryRepository industryRepository;

        public RFQService(IDocumentRepository repoDocument, IRfqRepository repoRfq, IAttachmentRepository repoAttachment, IRequirementRepository repoRequirement, ISlaRepository repoSla, IRfqPriceScheduleRepository repoRfqPriceSchedule, IPenaltyRepository repoPenalty, IInvitedSupplierRepository repoInvitedSupplier, IDocumentUserRepository repoDocumentUser, IUserRepository repoUser, IRoleRepository repoRole, ICompanyRepository repoCompany, IDocumentActivityRepository repoDocumentActivity, IQuotationRepository repoQuotation, ITimeLineRepository repoTimeLine, ICompanySupplierRepository repoCompanySupplier, IClarificationRepository repoClarification, IIndustryRepository industryRepository)
        {
            this.repoDocument = repoDocument;
            this.repoRfq = repoRfq;
            this.repoAttachment = repoAttachment;
            this.repoRequirement = repoRequirement;
            this.repoSla = repoSla;
            this.repoRfqPriceSchedule = repoRfqPriceSchedule;
            this.repoPenalty = repoPenalty;
            this.repoInvitedSupplier = repoInvitedSupplier;
            this.repoDocumentUser = repoDocumentUser;
            this.repoRole = repoRole;
            this.repoUser = repoUser;
            this.repoCompany = repoCompany;
            this.repoDocumentActivity = repoDocumentActivity;
            this.repoQuotation = repoQuotation;
            this.repoCompanySupplier = repoCompanySupplier;
            this.repoTimeLine = repoTimeLine;
            this.repoClarification = repoClarification;
            this.industryRepository = industryRepository;
        }

        public async Task<VmRfqPage> GetRfqByPage(int userId, int documentOwner, int page, int totalRecords, bool isCompanyAdmin, int statusId = 0, string keyword = "")
        {
            var dbPageResult = await repoRfq.GetRfqByPage(userId, documentOwner,
                (page == 0 ? Constants.app_firstPage : page),
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords), isCompanyAdmin, statusId, keyword);

            //var dbPageResult = repo.GetCodeTableByPage(keyword,
            //    (page == 0 ? Constants.app_firstPage : page),
            //    (totalRecords == 0 ? Constants.app_totalRecords : totalRecords));

            if (dbPageResult == null)
            {
                return new VmRfqPage();
            }

            var resultObj = new VmRfqPage();
            resultObj.RequestId = DateTime.Now.ToString("yyyyMMddHHmmss");
            resultObj.RequestDate = DateTime.Now;
            resultObj.Result = new PageResult<VmRfqItem>();
            resultObj.Result.Records = new List<VmRfqItem>();

            Copy<PageResult<Rfq>, PageResult<VmRfqItem>>(dbPageResult, resultObj.Result, new string[] { "Records" });

            var industryList = industryRepository.GetAll();

            foreach (var dbItem in dbPageResult.Records)
            {
                var resultItem = new VmRfqItem();

                Copy<Rfq, VmRfqItem>(dbItem, resultItem);

                if (dbItem.Document != null)
                {
                    resultItem.Document = new ViewModels.Document.VmDocumentItem()
                    {
                        Id = dbItem.Document.Id,
                        DocumentNo = dbItem.Document.DocumentNo,
                        Title = dbItem.Document.Title,
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

                resultItem.IndustryOfRfq = "";

                if (!string.IsNullOrEmpty(dbItem.IndustryOfRfq))
                {
                    var industry = industryList.Result.Where(x => x.Id == int.Parse(dbItem.IndustryOfRfq)).FirstOrDefault();
                    if (industry != null)
                    {
                        resultItem.IndustryOfRfq = industry.Name;
                    }
                }

                resultObj.Result.Records.Add(resultItem);
            }

            return resultObj;
        }

        public async Task<VmRfqPage> GetRfqForSupplierByPage(int supplierId, int page, int totalRecords, int statusId = 0, string keyword = "")
        {
            var dbPageResult = await repoRfq.GetRfqForSupplierByPage(supplierId,
                (page == 0 ? Constants.app_firstPage : page),
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords), statusId, keyword);

            //var dbPageResult = repo.GetCodeTableByPage(keyword,
            //    (page == 0 ? Constants.app_firstPage : page),
            //    (totalRecords == 0 ? Constants.app_totalRecords : totalRecords));

            if (dbPageResult == null)
            {
                return new VmRfqPage();
            }

            var resultObj = new VmRfqPage();
            resultObj.RequestId = DateTime.Now.ToString("yyyyMMddHHmmss");
            resultObj.RequestDate = DateTime.Now;
            resultObj.Result = new PageResult<VmRfqItem>();
            resultObj.Result.Records = new List<VmRfqItem>();

            Copy<PageResult<Rfq>, PageResult<VmRfqItem>>(dbPageResult, resultObj.Result, new string[] { "Records" });

            var industryList = industryRepository.GetAll();

            foreach (var dbItem in dbPageResult.Records)
            {
                var resultItem = new VmRfqItem();

                Copy<Rfq, VmRfqItem>(dbItem, resultItem);

                if (dbItem.Document != null)
                {
                    resultItem.Document = new ViewModels.Document.VmDocumentItem()
                    {
                        Id = dbItem.Document.Id,
                        DocumentNo = dbItem.Document.DocumentNo,
                        Title = dbItem.Document.Title,
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

                resultItem.IndustryOfRfq = "";

                if (!string.IsNullOrEmpty(dbItem.IndustryOfRfq))
                {
                    var industry = industryList.Result.Where(x => x.Id == int.Parse(dbItem.IndustryOfRfq)).FirstOrDefault();
                    if (industry != null)
                    {
                        resultItem.IndustryOfRfq = industry.Name;
                    }
                }

                resultObj.Result.Records.Add(resultItem);
            }

            return resultObj;
        }

        public async Task<VmRfqPage> GetPublicRfqByPage(int page, int totalRecords, int statusId = 0, string keyword = "")
        {
            var dbPageResult = await repoRfq.GetPublicRfqByPage((page == 0 ? Constants.app_firstPage : page),
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords), statusId, keyword);

            //var dbPageResult = repo.GetCodeTableByPage(keyword,
            //    (page == 0 ? Constants.app_firstPage : page),
            //    (totalRecords == 0 ? Constants.app_totalRecords : totalRecords));

            if (dbPageResult == null)
            {
                return new VmRfqPage();
            }

            var resultObj = new VmRfqPage();
            resultObj.RequestId = DateTime.Now.ToString("yyyyMMddHHmmss");
            resultObj.RequestDate = DateTime.Now;
            resultObj.Result = new PageResult<VmRfqItem>();
            resultObj.Result.Records = new List<VmRfqItem>();

            Copy<PageResult<Rfq>, PageResult<VmRfqItem>>(dbPageResult, resultObj.Result, new string[] { "Records" });

            foreach (var dbItem in dbPageResult.Records)
            {
                var resultItem = new VmRfqItem();

                Copy<Rfq, VmRfqItem>(dbItem, resultItem);

                if (dbItem.Document != null)
                {
                    resultItem.Document = new ViewModels.Document.VmDocumentItem()
                    {
                        DocumentNo = dbItem.Document.DocumentNo,
                        Title = dbItem.Document.Title,
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

        public bool CheckRFQLimit(int companyId)
        {
            string currentWeek = GetCurrentWeek();
            int documentCount = repoDocument.GetRfqCountByCompanyAndWorkingPeriod(companyId, currentWeek);


            var dbResult = repoCompany.Get(companyId);

            int RFQLimitPerWeek = (dbResult.Result.MaxRFQPerWeek == null) ? 0 : Convert.ToInt32(dbResult.Result.MaxRFQPerWeek);

            bool RFQLimit = true;

            if (documentCount >= RFQLimitPerWeek)
            {
                RFQLimit = false;
            }

            return RFQLimit;
        }


        public VmGenericServiceResult SaveRFQ(VmRfqItem rfq)
        {
            var result = new VmGenericServiceResult();
            try
            {
                var dbDocument = new Com.BudgetMetal.DBEntities.Document();
                string documentNo = "";
                documentNo = GenerateDocumentNo(rfq.Document.Company_Id);
                rfq.Document.DocumentNo = documentNo;
                rfq.Document.WorkingPeriod = GetCurrentWeek();
                Copy<VmDocumentItem, Com.BudgetMetal.DBEntities.Document>(rfq.Document, dbDocument);
                repoDocument.Add(dbDocument);
                repoDocument.Commit();

                rfq.Document_Id = dbDocument.Id;

                var dbRFQ = new Com.BudgetMetal.DBEntities.Rfq();
                Copy<VmRfqItem, Com.BudgetMetal.DBEntities.Rfq>(rfq, dbRFQ);
                repoRfq.Add(dbRFQ);
                repoRfq.Commit();

                if (rfq.Document.Attachment != null)
                {
                    if (rfq.Document.Attachment.Count > 0)
                    {
                        foreach (var item in rfq.Document.Attachment)
                        {
                            var dbAttachment = new Com.BudgetMetal.DBEntities.Attachment();

                            Copy<VmAttachmentItem, Com.BudgetMetal.DBEntities.Attachment>(item, dbAttachment);
                            dbAttachment.Document_Id = dbDocument.Id;
                            dbAttachment.CreatedBy = dbAttachment.UpdatedBy = dbRFQ.CreatedBy;
                            repoAttachment.Add(dbAttachment);
                        }
                        repoAttachment.Commit();
                    }
                }

                if (rfq.Document.DocumentUser != null)
                {
                    if (rfq.Document.DocumentUser.Count > 0)
                    {
                        foreach (var item in rfq.Document.DocumentUser)
                        {
                            var dbDocumentUser = new Com.BudgetMetal.DBEntities.DocumentUser();

                            Copy<VmDocumentUserItem, Com.BudgetMetal.DBEntities.DocumentUser>(item, dbDocumentUser);
                            dbDocumentUser.Document_Id = dbDocument.Id;
                            dbDocumentUser.CreatedBy = dbDocumentUser.UpdatedBy = dbRFQ.CreatedBy;
                            repoDocumentUser.Add(dbDocumentUser);
                        }
                        repoDocumentUser.Commit();
                    }
                }

                if (rfq.Requirement != null)
                {
                    if (rfq.Requirement.Count > 0)
                    {
                        foreach (var item in rfq.Requirement)
                        {
                            if (item.ServiceName != null && item.Description != null)
                            {
                                var dbRequirement = new Com.BudgetMetal.DBEntities.Requirement();

                                Copy<VmRequirementItem, Com.BudgetMetal.DBEntities.Requirement>(item, dbRequirement);
                                dbRequirement.Rfq_Id = dbRFQ.Id;
                                dbRequirement.CreatedBy = dbRequirement.UpdatedBy = dbRFQ.CreatedBy;
                                repoRequirement.Add(dbRequirement);
                            }
                        }
                        repoRequirement.Commit();
                    }
                }

                if (rfq.Sla != null)
                {
                    if (rfq.Sla.Count > 0)
                    {
                        foreach (var item in rfq.Sla)
                        {
                            if (item.Requirement != null && item.Description != null)
                            {
                                var dbSla = new Com.BudgetMetal.DBEntities.Sla();

                                Copy<VmSlaItem, Com.BudgetMetal.DBEntities.Sla>(item, dbSla);
                                dbSla.Rfq_Id = dbRFQ.Id;
                                dbSla.CreatedBy = dbSla.UpdatedBy = dbRFQ.CreatedBy;
                                repoSla.Add(dbSla);
                            }

                        }
                        repoSla.Commit();
                    }
                }

                if (rfq.Penalty != null)
                {
                    if (rfq.Penalty.Count > 0)
                    {
                        foreach (var item in rfq.Penalty)
                        {
                            if (item.BreachOfServiceDefinition != null && item.PenaltyAmount != null && item.Description != null)
                            {
                                var dbPenalty = new Com.BudgetMetal.DBEntities.Penalty();

                                Copy<VmPenaltyItem, Com.BudgetMetal.DBEntities.Penalty>(item, dbPenalty);
                                dbPenalty.Rfq_Id = dbRFQ.Id;
                                dbPenalty.CreatedBy = dbPenalty.UpdatedBy = dbRFQ.CreatedBy;
                                repoPenalty.Add(dbPenalty);
                            }
                        }
                        repoPenalty.Commit();
                    }
                }

                if (rfq.RfqPriceSchedule != null)
                {
                    if (rfq.RfqPriceSchedule.Count > 0)
                    {
                        foreach (var item in rfq.RfqPriceSchedule)
                        {
                            if (item.ItemName != null && item.ItemDescription != null && item.InternalRefrenceCode != null && item.QuantityRequired != null)
                            {
                                var dbRfqPriceSchedule = new Com.BudgetMetal.DBEntities.RfqPriceSchedule();

                                Copy<VmRfqPriceScheduleItem, Com.BudgetMetal.DBEntities.RfqPriceSchedule>(item, dbRfqPriceSchedule);
                                dbRfqPriceSchedule.Rfq_Id = dbRFQ.Id;
                                dbRfqPriceSchedule.CreatedBy = dbRfqPriceSchedule.UpdatedBy = dbRFQ.CreatedBy;
                                repoRfqPriceSchedule.Add(dbRfqPriceSchedule);
                            }
                        }
                        repoRfqPriceSchedule.Commit();
                    }
                }

                if (rfq.InvitedSupplier != null)
                {
                    if (rfq.InvitedSupplier.Count > 0)
                    {
                        foreach (var item in rfq.InvitedSupplier)
                        {
                            var dbInvitedSupplier = new Com.BudgetMetal.DBEntities.InvitedSupplier();

                            Copy<VmInvitedSupplierItem, Com.BudgetMetal.DBEntities.InvitedSupplier>(item, dbInvitedSupplier);
                            dbInvitedSupplier.Rfq_Id = dbRFQ.Id;
                            dbInvitedSupplier.CreatedBy = dbInvitedSupplier.UpdatedBy = dbRFQ.CreatedBy;
                            repoInvitedSupplier.Add(dbInvitedSupplier);

                            //For Prefer supplier List
                            if (!repoCompanySupplier.IsExistedSupplier(rfq.Document.Company_Id, item.Company_Id))
                            {
                                Com.BudgetMetal.DBEntities.CompanySupplier companySupplierEntity = new Com.BudgetMetal.DBEntities.CompanySupplier();
                                companySupplierEntity.Company_Id = rfq.Document.Company_Id;
                                companySupplierEntity.Supplier_Id = item.Company_Id;
                                companySupplierEntity.CreatedBy = companySupplierEntity.UpdatedBy = dbRFQ.CreatedBy;
                                repoCompanySupplier.Add(companySupplierEntity);
                            }
                        }
                        repoInvitedSupplier.Commit();
                    }
                }
                //            Copy<VmInvitedSupplierItem, Com.BudgetMetal.DBEntities.InvitedSupplier>(item, dbInvitedSupplier);
                //            dbInvitedSupplier.Rfq_Id = dbRFQ.Id;
                //            dbInvitedSupplier.CreatedBy = dbInvitedSupplier.UpdatedBy = dbRFQ.CreatedBy;
                //            repoInvitedSupplier.Add(dbInvitedSupplier);
                //        }
                //        repoInvitedSupplier.Commit();
                //    }
                //}

                if (rfq.Document.DocumentActivityList != null)
                {
                    if (rfq.Document.DocumentActivityList.Count > 0)
                    {
                        foreach (var item in rfq.Document.DocumentActivityList)
                        {
                            var dbDocumentActivity = new Com.BudgetMetal.DBEntities.DocumentActivity();

                            Copy<VmDocumentActivityItem, Com.BudgetMetal.DBEntities.DocumentActivity>(item, dbDocumentActivity);
                            dbDocumentActivity.Document_Id = dbDocument.Id;
                            dbDocumentActivity.CreatedBy = dbDocumentActivity.UpdatedBy = dbRFQ.CreatedBy;
                            repoDocumentActivity.Add(dbDocumentActivity);
                        }
                        repoDocumentActivity.Commit();
                    }
                }

                //start adding timeline
                var timeline = new Com.BudgetMetal.DBEntities.TimeLine()
                {
                    Company_Id = rfq.Document.Company_Id,
                    User_Id = rfq.Document.DocumentUser.FirstOrDefault().User_Id,
                    Message = "RFQ is successfully created.",
                    MessageType = Constants_CodeTable.Code_TM_Rfq,
                    IsRead = false,
                    Document_Id = dbDocument.Id,
                    CreatedBy = dbRFQ.CreatedBy,
                    UpdatedBy = dbRFQ.CreatedBy
                };
                repoTimeLine.Add(timeline);
                repoTimeLine.Commit();
                //end adding timeline
                if (rfq.Document.DocumentStatus_Id == Constants_CodeTable.Code_RFQ_Submitted)
                {
                    //get invited supplier email

                    var resultSupplierAdmin = repoUser.GetSupplierAdmin(rfq.InvitedSupplier.Select(e => e.Company_Id).Distinct().ToList());
                    var sendMail = new SendingMail();
                    if (resultSupplierAdmin != null)
                    {
                        string emailSubject = "You are intvited for RFQ " + documentNo + ".";
                        string emailBody = "Email Template need to provide.";
                        foreach (var item in resultSupplierAdmin)
                        {
                            sendMail.SendMail(item, "", emailSubject, emailBody);
                        }
                    }
                    //start adding timeline
                    foreach (var item in rfq.InvitedSupplier)
                    {
                        var timelineForInvitedSupplier = new Com.BudgetMetal.DBEntities.TimeLine()
                        {
                            Company_Id = item.Company_Id,
                            User_Id = rfq.Document.DocumentUser.FirstOrDefault().User_Id,
                            Message = "RFQ is successfully created.",
                            MessageType = Constants_CodeTable.Code_TM_Rfq,
                            Document_Id = dbDocument.Id,
                            IsRead = false,
                            CreatedBy = dbRFQ.CreatedBy,
                            UpdatedBy = dbRFQ.CreatedBy
                        };
                        repoTimeLine.Add(timelineForInvitedSupplier);
                        repoTimeLine.Commit();
                    }
                    //end adding timeline
                }

                result.IsSuccess = true;
                result.MessageToUser = "You have succesfully created RFQ as Document No." + documentNo;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.MessageToUser = "Fail to create your RFQ. Please contact site admin.";
                result.Error = ex;
            }


            return result;
        }

        public VmGenericServiceResult UpdateRFQ(VmRfqItem rfq)
        {
            var result = new VmGenericServiceResult();
            try
            {
                var dbDocument = new Com.BudgetMetal.DBEntities.Document();
                Copy<VmDocumentItem, Com.BudgetMetal.DBEntities.Document>(rfq.Document, dbDocument);
                repoDocument.Update(dbDocument);
                repoDocument.Commit();

                rfq.Document_Id = dbDocument.Id;

                var dbRFQ = new Com.BudgetMetal.DBEntities.Rfq();
                Copy<VmRfqItem, Com.BudgetMetal.DBEntities.Rfq>(rfq, dbRFQ);
                repoRfq.Update(dbRFQ);
                repoRfq.Commit();

                repoAttachment.InactiveByDocumentId(dbDocument.Id, dbDocument.UpdatedBy);
                repoAttachment.Commit();
                if (rfq.Document.Attachment != null)
                {
                    if (rfq.Document.Attachment.Count > 0)
                    {
                        foreach (var item in rfq.Document.Attachment)
                        {
                            var dbAttachment = new Com.BudgetMetal.DBEntities.Attachment();

                            Copy<VmAttachmentItem, Com.BudgetMetal.DBEntities.Attachment>(item, dbAttachment);
                            dbAttachment.Document_Id = dbDocument.Id;
                            dbAttachment.CreatedBy = dbAttachment.UpdatedBy = dbRFQ.UpdatedBy;
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

                if (rfq.Document != null)
                {
                    if (rfq.Document.DocumentUser.Count > 0)
                    {
                        foreach (var item in rfq.Document.DocumentUser)
                        {
                            var dbDocumentUser = new Com.BudgetMetal.DBEntities.DocumentUser();

                            Copy<VmDocumentUserItem, Com.BudgetMetal.DBEntities.DocumentUser>(item, dbDocumentUser);
                            dbDocumentUser.Document_Id = dbDocument.Id;
                            dbDocumentUser.CreatedBy = dbDocumentUser.UpdatedBy = dbRFQ.UpdatedBy;
                            repoDocumentUser.Add(dbDocumentUser);
                        }
                        repoDocumentUser.Commit();
                    }
                }

                repoRequirement.InactiveByRFQId(dbRFQ.Id, dbRFQ.UpdatedBy);
                repoRequirement.Commit();
                if (rfq.Requirement != null)
                {
                    if (rfq.Requirement.Count > 0)
                    {
                        foreach (var item in rfq.Requirement)
                        {
                            if (item.ServiceName != null && item.Description != null)
                            {
                                var dbRequirement = new Com.BudgetMetal.DBEntities.Requirement();

                                Copy<VmRequirementItem, Com.BudgetMetal.DBEntities.Requirement>(item, dbRequirement);
                                dbRequirement.Rfq_Id = dbRFQ.Id;
                                dbRequirement.CreatedBy = dbRequirement.UpdatedBy = dbRFQ.UpdatedBy;
                                repoRequirement.Add(dbRequirement);
                            }
                        }
                        repoRequirement.Commit();
                    }
                }

                repoSla.InactiveByRFQId(dbRFQ.Id, dbRFQ.UpdatedBy);
                repoSla.Commit();

                if (rfq.Sla != null)
                {
                    if (rfq.Sla.Count > 0)
                    {
                        foreach (var item in rfq.Sla)
                        {
                            if (item.Requirement != null && item.Description != null)
                            {
                                var dbSla = new Com.BudgetMetal.DBEntities.Sla();

                                Copy<VmSlaItem, Com.BudgetMetal.DBEntities.Sla>(item, dbSla);
                                dbSla.Rfq_Id = dbRFQ.Id;
                                dbSla.CreatedBy = dbSla.UpdatedBy = dbRFQ.UpdatedBy;
                                repoSla.Add(dbSla);
                            }
                        }
                        repoSla.Commit();
                    }
                }

                repoPenalty.InactiveByRFQId(dbRFQ.Id, dbRFQ.UpdatedBy);
                repoPenalty.Commit();

                if (rfq.Penalty != null)
                {
                    if (rfq.Penalty.Count > 0)
                    {
                        foreach (var item in rfq.Penalty)
                        {
                            if (item.BreachOfServiceDefinition != null && item.PenaltyAmount != null && item.Description != null)
                            {
                                var dbPenalty = new Com.BudgetMetal.DBEntities.Penalty();

                                Copy<VmPenaltyItem, Com.BudgetMetal.DBEntities.Penalty>(item, dbPenalty);
                                dbPenalty.Rfq_Id = dbRFQ.Id;
                                dbPenalty.CreatedBy = dbPenalty.UpdatedBy = dbRFQ.UpdatedBy;
                                repoPenalty.Add(dbPenalty);
                            }

                        }
                        repoPenalty.Commit();
                    }
                }

                repoRfqPriceSchedule.InactiveByRFQId(dbRFQ.Id, dbRFQ.UpdatedBy);
                repoRfqPriceSchedule.Commit();

                if (rfq.RfqPriceSchedule != null)
                {
                    if (rfq.RfqPriceSchedule.Count > 0)
                    {
                        foreach (var item in rfq.RfqPriceSchedule)
                        {
                            if (item.ItemName != null && item.ItemDescription != null && item.InternalRefrenceCode != null && item.QuantityRequired != null)
                            {
                                var dbRfqPriceSchedule = new Com.BudgetMetal.DBEntities.RfqPriceSchedule();

                                Copy<VmRfqPriceScheduleItem, Com.BudgetMetal.DBEntities.RfqPriceSchedule>(item, dbRfqPriceSchedule);
                                dbRfqPriceSchedule.Rfq_Id = dbRFQ.Id;
                                dbRfqPriceSchedule.CreatedBy = dbRfqPriceSchedule.UpdatedBy = dbRFQ.UpdatedBy;
                                repoRfqPriceSchedule.Add(dbRfqPriceSchedule);
                            }

                        }
                        repoRfqPriceSchedule.Commit();
                    }
                }

                repoInvitedSupplier.InactiveByRFQId(dbRFQ.Id, dbRFQ.UpdatedBy);
                repoInvitedSupplier.Commit();

                if (rfq.InvitedSupplier != null)
                {
                    if (rfq.InvitedSupplier.Count > 0)
                    {
                        foreach (var item in rfq.InvitedSupplier)
                        {
                            var dbInvitedSupplier = new Com.BudgetMetal.DBEntities.InvitedSupplier();

                            Copy<VmInvitedSupplierItem, Com.BudgetMetal.DBEntities.InvitedSupplier>(item, dbInvitedSupplier);
                            dbInvitedSupplier.Rfq_Id = dbRFQ.Id;
                            dbInvitedSupplier.CreatedBy = dbInvitedSupplier.UpdatedBy = dbRFQ.UpdatedBy;
                            repoInvitedSupplier.Add(dbInvitedSupplier);

                            //For Prefer supplier List
                            if (!repoCompanySupplier.IsExistedSupplier(rfq.Document.Company_Id, item.Company_Id))
                            {
                                Com.BudgetMetal.DBEntities.CompanySupplier companySupplierEntity = new Com.BudgetMetal.DBEntities.CompanySupplier();
                                companySupplierEntity.Company_Id = rfq.Document.Company_Id;
                                companySupplierEntity.Supplier_Id = item.Company_Id;
                                companySupplierEntity.CreatedBy = companySupplierEntity.UpdatedBy = dbRFQ.CreatedBy;
                                repoCompanySupplier.Add(companySupplierEntity);
                            }

                        }
                        repoInvitedSupplier.Commit();
                    }
                }
                //            Copy<VmInvitedSupplierItem, Com.BudgetMetal.DBEntities.InvitedSupplier>(item, dbInvitedSupplier);
                //            dbInvitedSupplier.Rfq_Id = dbRFQ.Id;
                //            dbInvitedSupplier.CreatedBy = dbInvitedSupplier.UpdatedBy = dbRFQ.UpdatedBy;
                //            repoInvitedSupplier.Add(dbInvitedSupplier);
                //        }
                //                    repoInvitedSupplier.Commit();
                //    }
                //}

                //if (dbDocument != null)
                //{
                //    var dbDocumentActivity = new Com.BudgetMetal.DBEntities.DocumentActivity();
                //    dbDocumentActivity.Document_Id = dbDocument.Id;
                //    dbDocumentActivity.User_Id = 1;
                //    dbDocumentActivity.IsRfq = true;
                //    dbDocumentActivity.Action = "Edit";
                //    dbDocumentActivity.CreatedBy = dbDocumentActivity.UpdatedBy = dbRFQ.UpdatedBy;
                //    repoDocumentActivity.Add(dbDocumentActivity);
                //    repoDocumentActivity.Commit();
                //}
                if (rfq.Document.DocumentActivityList != null)
                {
                    if (rfq.Document.DocumentActivityList.Count > 0)
                    {
                        foreach (var item in rfq.Document.DocumentActivityList)
                        {
                            var dbDocumentActivity = new Com.BudgetMetal.DBEntities.DocumentActivity();

                            Copy<VmDocumentActivityItem, Com.BudgetMetal.DBEntities.DocumentActivity>(item, dbDocumentActivity);
                            dbDocumentActivity.Document_Id = rfq.Document_Id;
                            dbDocumentActivity.CreatedBy = dbDocumentActivity.UpdatedBy = dbRFQ.CreatedBy;
                            repoDocumentActivity.Add(dbDocumentActivity);
                        }
                        repoDocumentActivity.Commit();
                    }
                }

                //start adding timeline
                var timeline = new Com.BudgetMetal.DBEntities.TimeLine()
                {
                    Company_Id = rfq.Document.Company_Id,
                    User_Id = rfq.Document.DocumentUser.FirstOrDefault().User_Id,
                    Message = "RFQ is successfully updated.",
                    MessageType = Constants_CodeTable.Code_TM_Rfq,
                    IsRead = false,
                    Document_Id = dbDocument.Id,
                    CreatedBy = dbRFQ.CreatedBy,
                    UpdatedBy = dbRFQ.CreatedBy
                };
                repoTimeLine.Add(timeline);
                repoTimeLine.Commit();
                //end adding timeline
                if (rfq.Document.DocumentStatus_Id == Constants_CodeTable.Code_RFQ_Submitted)
                {
                    //get invited supplier email

                    var resultSupplierAdmin = repoUser.GetSupplierAdmin(rfq.InvitedSupplier.Select(e => e.Company_Id).Distinct().ToList());
                    var sendMail = new SendingMail();
                    if (resultSupplierAdmin != null)
                    {
                        string emailSubject = "You are intvited for Document " + rfq.Document.DocumentNo + ".";
                        string emailBody = "Email Template need to provide.";
                        foreach (var item in resultSupplierAdmin)
                        {
                            sendMail.SendMail(item, "", emailSubject, emailBody);
                        }
                    }
                    //start adding timeline
                    foreach (var item in rfq.InvitedSupplier)
                    {
                        var timelineForInvitedSupplier = new Com.BudgetMetal.DBEntities.TimeLine()
                        {
                            Company_Id = item.Company_Id,
                            User_Id = rfq.Document.DocumentUser.FirstOrDefault().User_Id,
                            Message = "RFQ is successfully updated.",
                            MessageType = Constants_CodeTable.Code_TM_Rfq,
                            Document_Id = dbDocument.Id,
                            IsRead = false,
                            CreatedBy = dbRFQ.CreatedBy,
                            UpdatedBy = dbRFQ.CreatedBy
                        };
                        repoTimeLine.Add(timelineForInvitedSupplier);
                        repoTimeLine.Commit();
                    }
                    //end adding timeline
                }

                result.IsSuccess = true;
                result.MessageToUser = "You have succesfully updated RFQ as Document No." + rfq.Document.DocumentNo;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.MessageToUser = "Fail to update your RFQ. Please contact site admin.";
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        ///  Action for withdrawn RFQ
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns>VmGenericServiceResult</returns>
        public async Task<VmGenericServiceResult> WithdrawnRfq(int documentId, int userId, string userName)
        {
            var result = new VmGenericServiceResult();
            try
            {
                //Change Status
                var dbDocument = await repoDocument.Get(documentId);
                dbDocument.DocumentStatus_Id = Constants_CodeTable.Code_RFQ_Withdrawn;
                dbDocument.UpdatedBy = userName;
                repoDocument.Update(dbDocument);

                //Add Document Activity
                var dbDocumentActivity = new Com.BudgetMetal.DBEntities.DocumentActivity()
                {
                    Action = "Withdrawn",
                    IsRfq = true,
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
                    Message = "Document " + dbDocument.DocumentNo + " is successfully withdrawn.",
                    MessageType = Constants_CodeTable.Code_TM_Rfq,
                    IsRead = false,
                    Document_Id = dbDocument.Id,
                    CreatedBy = userName,
                    UpdatedBy = userName
                };
                repoTimeLine.Add(timeline);
                repoTimeLine.Commit();

                var dbInvitedSupplierList = await repoInvitedSupplier.GetByDocumentId(documentId);
                if (dbInvitedSupplierList != null)
                {
                    var resultSupplierAdmin = repoUser.GetSupplierAdmin(dbInvitedSupplierList.Select(e => e.Company_Id).Distinct().ToList());
                    var sendMail = new SendingMail();
                    if (resultSupplierAdmin != null)
                    {
                        string emailSubject = "Buyer withdrawn Document " + dbDocument.DocumentNo + ".";
                        string emailBody = "Email Template need to provide.";
                        foreach (var item in resultSupplierAdmin)
                        {
                            sendMail.SendMail(item, "", emailSubject, emailBody);
                        }
                    }

                    foreach (var item in dbInvitedSupplierList)
                    {
                        var timelineForInvitedSupplier = new Com.BudgetMetal.DBEntities.TimeLine()
                        {
                            Company_Id = item.Company_Id,
                            User_Id = userId,
                            Message = "Document " + dbDocument.DocumentNo + " is withdrawn.",
                            MessageType = Constants_CodeTable.Code_TM_Rfq,
                            Document_Id = documentId,
                            IsRead = false,
                            CreatedBy = userName,
                            UpdatedBy = userName
                        };
                        repoTimeLine.Add(timelineForInvitedSupplier);
                        repoTimeLine.Commit();
                    }
                }

                result.IsSuccess = true;
                result.MessageToUser = "Your Rfq is successfully updated.";
            }
            catch
            {
                result.IsSuccess = false;
                result.MessageToUser = "Your Rfq is failed to update.";
            }

            return result;

        }

        public async Task<VmGenericServiceResult> DeleteRfq(int documentId, int userId, string userName)
        {
            var result = new VmGenericServiceResult();
            try
            {
                //Change Status
                var dbDocument = await repoDocument.Get(documentId);
                dbDocument.DocumentStatus_Id = Constants_CodeTable.Code_RFQ_Delete;
                dbDocument.UpdatedBy = userName;
                repoDocument.Update(dbDocument);

                //Add Document Activity
                var dbDocumentActivity = new Com.BudgetMetal.DBEntities.DocumentActivity()
                {
                    Action = "Withdrawn",
                    User_Id = userId,
                    IsRfq = true,
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
                    Message = "Document " + dbDocument.DocumentNo + " is successfully deleted.",
                    MessageType = Constants_CodeTable.Code_TM_Rfq,
                    IsRead = false,
                    Document_Id = dbDocument.Id,
                    CreatedBy = userName,
                    UpdatedBy = userName
                };
                repoTimeLine.Add(timeline);
                repoTimeLine.Commit();



                result.IsSuccess = true;
                result.MessageToUser = "Your Rfq is successfully updated.";
            }
            catch
            {
                result.IsSuccess = false;
                result.MessageToUser = "Your Rfq is failed to update.";
            }

            return result;

        }

        private string GenerateDocumentNo(int companyId)
        {
            int countRfq = repoDocument.GetRfqCountByCompany(companyId);
            countRfq = countRfq + 1;
            string documentNo = "RFQ_" + companyId.ToString().PadLeft(4, '0') + "_" + countRfq.ToString().PadLeft(4, '0');
            return documentNo;
        }

        ////public async Task<VmRfqPage> GetRfqByPage(string keyword, int page, int totalRecords)
        ////{
        ////    var dbPageResult = await repoRfq.GetPage(keyword,
        ////        (page == 0 ? Constants.app_firstPage : page),
        ////        (totalRecords == 0 ? Constants.app_totalRecords : totalRecords));

        ////    if (dbPageResult == null)
        ////    {
        ////        return new VmRfqPage();
        ////    }

        ////    var resultObj = new VmRfqPage();
        ////    resultObj.RequestId = DateTime.Now.ToString("yyyyMMddHHmmss");
        ////    resultObj.RequestDate = DateTime.Now;
        ////    resultObj.Result = new PageResult<VmRfqItem>();
        ////    resultObj.Result.Records = new List<VmRfqItem>();

        ////    Copy<PageResult<Rfq>, PageResult<VmRfqItem>>(dbPageResult, resultObj.Result, new string[] { "Records" });

        ////    foreach (var dbItem in dbPageResult.Records)
        ////    {
        ////        var resultItem = new VmRfqItem();

        ////        Copy<Rfq, VmRfqItem>(dbItem, resultItem);

        ////        if (dbItem.Document != null)
        ////        {
        ////            resultItem.Document = new ViewModels.Document.VmDocumentItem()
        ////            {
        ////                Title = dbItem.Document.Title
        ////            };
        ////        }

        ////        resultObj.Result.Records.Add(resultItem);
        ////    }

        ////    return resultObj;
        ////}

        public async Task<VmRfqItem> GetRfqtById(int Id)
        {
            var dbPageResult = await repoRfq.Get(Id);

            if (dbPageResult == null)
            {
                return new VmRfqItem();
            }

            var resultObj = new VmRfqItem();

            Copy<Rfq, VmRfqItem>(dbPageResult, resultObj);

            var dbDocumentEntity = await repoDocument.GetAll();

            if (dbDocumentEntity == null) return resultObj;

            resultObj.DocumentList = new List<VmDocumentItem>();

            foreach (var dbcat in dbDocumentEntity)
            {
                VmDocumentItem docuentItem = new VmDocumentItem()
                {
                    Id = dbcat.Id,
                    Title = dbcat.Title
                };

                resultObj.DocumentList.Add(docuentItem);
            }

            return resultObj;
        }

        public VmGenericServiceResult Insert(VmRfqItem vmItem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                Rfq entity = new Rfq();

                Copy<VmRfqItem, Rfq>(vmItem, entity);

                entity.Document_Id = (int)vmItem.Document_Id;
                if (entity.CreatedBy.IsNullOrEmpty())
                {
                    entity.CreatedBy = entity.UpdatedBy = "System";
                }
                repoRfq.Add(entity);

                repoRfq.Commit();

                result.IsSuccess = true;
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Error = e;
            }

            return result;
        }

        public async Task<VmGenericServiceResult> Update(VmRfqItem vmItem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                Rfq entity = await repoRfq.Get(vmItem.Id);

                Copy<VmRfqItem, Rfq>(vmItem, entity);

                if (entity.UpdatedBy.IsNullOrEmpty())
                {
                    entity.UpdatedBy = "System";
                }

                repoRfq.Update(entity);

                repoRfq.Commit();

                result.IsSuccess = true;
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Error = e;
            }

            return result;
        }

        public async Task Delete(int Id)
        {
            var entity = await repoRfq.Get(Id);
            entity.IsActive = false;
            repoRfq.Update(entity);
            repoRfq.Commit();
        }

        public async Task<VmRfqItem> GetFormObject()
        {
            VmRfqItem resultObj = new VmRfqItem();

            var dbDocumentList = await repoDocument.GetAll();

            if (dbDocumentList == null) return resultObj;

            resultObj.DocumentList = new List<VmDocumentItem>();

            foreach (var dbcat in dbDocumentList)
            {
                VmDocumentItem document = new VmDocumentItem()
                {
                    Id = dbcat.Id,
                    Title = dbcat.Title,
                    ContactPersonName = dbcat.ContactPersonName,
                    DocumentNo = dbcat.DocumentNo
                };

                resultObj.DocumentList.Add(document);
            }

            return resultObj;
        }

        public async Task<VmRfqItem> GetSingleRfqById(int documentId)
        {
            var dbResult = await repoRfq.GetSingleRfqById(documentId);

            var resultObject = new VmRfqItem();

            Copy<Com.BudgetMetal.DBEntities.Rfq, VmRfqItem>(dbResult, resultObject, new string[] { "Document", "InvitedSupplier", "Penalty", "Requirement", "RfqPriceSchedule", "Sla" });

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

            var listRequirement = new List<VmRequirementItem>();
            if (dbResult.Requirement != null)
            {
                foreach (var item in dbResult.Requirement.Where(e => e.IsActive == true).ToList())
                {
                    var itemRequirement = new VmRequirementItem();
                    Copy<Com.BudgetMetal.DBEntities.Requirement, VmRequirementItem>(item, itemRequirement, new string[] { "Rfq" });
                    listRequirement.Add(itemRequirement);
                }
            }
            resultObject.Requirement = listRequirement;

            var listSla = new List<VmSlaItem>();
            if (dbResult.Sla != null)
            {
                foreach (var item in dbResult.Sla.Where(e => e.IsActive == true).ToList())
                {
                    var itemSla = new VmSlaItem();
                    Copy<Com.BudgetMetal.DBEntities.Sla, VmSlaItem>(item, itemSla, new string[] { "Rfq" });
                    listSla.Add(itemSla);
                }
            }
            resultObject.Sla = listSla;

            var listPenalty = new List<VmPenaltyItem>();
            if (dbResult.Penalty != null)
            {
                foreach (var item in dbResult.Penalty.Where(e => e.IsActive == true).ToList())
                {
                    var itemPenalty = new VmPenaltyItem();
                    Copy<Com.BudgetMetal.DBEntities.Penalty, VmPenaltyItem>(item, itemPenalty, new string[] { "Rfq" });
                    listPenalty.Add(itemPenalty);
                }
            }
            resultObject.Penalty = listPenalty;

            var listRfqPriceSchedule = new List<VmRfqPriceScheduleItem>();
            if (dbResult.RfqPriceSchedule != null)
            {
                foreach (var item in dbResult.RfqPriceSchedule.Where(e => e.IsActive == true).ToList())
                {
                    var itemRfqPriceSchedule = new VmRfqPriceScheduleItem();
                    Copy<Com.BudgetMetal.DBEntities.RfqPriceSchedule, VmRfqPriceScheduleItem>(item, itemRfqPriceSchedule, new string[] { "Rfq" });
                    listRfqPriceSchedule.Add(itemRfqPriceSchedule);
                }
            }
            resultObject.RfqPriceSchedule = listRfqPriceSchedule;

            var listInvitedSupplier = new List<VmInvitedSupplierItem>();
            if (dbResult.InvitedSupplier != null)
            {
                foreach (var item in dbResult.InvitedSupplier.Where(e => e.IsActive == true).ToList())
                {
                    var itemInvitedSupplier = new VmInvitedSupplierItem();
                    Copy<Com.BudgetMetal.DBEntities.InvitedSupplier, VmInvitedSupplierItem>(item, itemInvitedSupplier, new string[] { "Rfq" });
                    listInvitedSupplier.Add(itemInvitedSupplier);
                }
            }
            resultObject.InvitedSupplier = listInvitedSupplier;




            if (dbResult.Document.DocumentActivity != null)
            {
                var listDocumentActivity = new List<VmDocumentActivityItem>();
                foreach (var item in dbResult.Document.DocumentActivity)
                {
                    var newItem = new VmDocumentActivityItem();
                    newItem.Action = item.Action;
                    newItem.CreatedBy = item.CreatedBy;
                    newItem.CreatedDate = item.CreatedDate;
                    newItem.Document_Id = item.Document_Id;
                    listDocumentActivity.Add(newItem);
                }
                resultObject.Document.DocumentActivityList = listDocumentActivity;
            }

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

            var dbResultQuotationList = repoQuotation.GetQuotationByRfqId(documentId);

            List<List<string>> requirementComparisonList = new List<List<string>>();
            List<List<string>> priceComparisonList = new List<List<string>>();
            foreach (var item in dbResultQuotationList.Result)
            {
                List<string> requirementComparison = new List<string>();
                requirementComparison.Add(item.Document.Company.Name);
                foreach (var requirementItem in item.QuotationRequirement.Where(e => e.IsActive == true).ToList())
                {
                    requirementComparison.Add(requirementItem.Compliance);
                }
                requirementComparisonList.Add(requirementComparison);

                List<string> priceComparison = new List<string>();
                priceComparison.Add(item.Document.Company.Name);
                foreach (var priceItem in item.QuotationPriceSchedule.Where(e => e.IsActive == true).ToList())
                {
                    priceComparison.Add(priceItem.ItemAmount.ToString());
                }
                priceComparisonList.Add(priceComparison);
            }

            resultObject.RequirementComparison = requirementComparisonList;
            resultObject.PriceComparison = priceComparisonList;

            return resultObject;
        }

        public async Task<VmRfqItem> GetPublicPortalSingleRfqById(int documentId)
        {
            var dbResult = await repoRfq.GetSingleRfqById(documentId);

            var resultObject = new VmRfqItem();

            Copy<Com.BudgetMetal.DBEntities.Rfq, VmRfqItem>(dbResult, resultObject, new string[] { "Document", "InvitedSupplier", "Penalty", "Requirement", "RfqPriceSchedule", "Sla" });

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

            //var listDocumentUser = new List<VmDocumentUserDisplay>();
            //if (dbResult.Document.DocumentUser != null)
            //{
            //    List<int> UserList = new List<int>();
            //    UserList = dbResult.Document.DocumentUser.Where(e => e.IsActive == true).Select(e => e.User_Id).Distinct().ToList();
            //    foreach (var itemUser in UserList)
            //    {
            //        var documentUser = new VmDocumentUserDisplay();
            //        documentUser.User_Id = itemUser;

            //        var user = new User();
            //        user = await repoUser.Get(itemUser);
            //        documentUser.UserName = user.ContactName;

            //        documentUser.Roles_Id = string.Join(",", dbResult.Document.DocumentUser.Where(e => e.User_Id == itemUser && e.IsActive == true).Select(e => e.Role_Id).ToArray());

            //        var roles = new List<string>();
            //        foreach (var roleId in dbResult.Document.DocumentUser.Where(e => e.User_Id == itemUser && e.IsActive == true).Select(e => e.Role_Id).ToList())
            //        {
            //            var role = new Role();
            //            role = await repoRole.Get(roleId);
            //            roles.Add(role.Name);
            //        }
            //        documentUser.Roles = roles;
            //        listDocumentUser.Add(documentUser);
            //    }
            //}
            //resultDocument.DocumentUserDisplay = listDocumentUser;

            resultObject.Document = resultDocument;

            var listRequirement = new List<VmRequirementItem>();
            if (dbResult.Requirement != null)
            {
                foreach (var item in dbResult.Requirement.Where(e => e.IsActive == true).ToList())
                {
                    var itemRequirement = new VmRequirementItem();
                    Copy<Com.BudgetMetal.DBEntities.Requirement, VmRequirementItem>(item, itemRequirement, new string[] { "Rfq" });
                    listRequirement.Add(itemRequirement);
                }
            }
            resultObject.Requirement = listRequirement;

            var listSla = new List<VmSlaItem>();
            if (dbResult.Sla != null)
            {
                foreach (var item in dbResult.Sla.Where(e => e.IsActive == true).ToList())
                {
                    var itemSla = new VmSlaItem();
                    Copy<Com.BudgetMetal.DBEntities.Sla, VmSlaItem>(item, itemSla, new string[] { "Rfq" });
                    listSla.Add(itemSla);
                }
            }
            resultObject.Sla = listSla;

            var listPenalty = new List<VmPenaltyItem>();
            if (dbResult.Penalty != null)
            {
                foreach (var item in dbResult.Penalty.Where(e => e.IsActive == true).ToList())
                {
                    var itemPenalty = new VmPenaltyItem();
                    Copy<Com.BudgetMetal.DBEntities.Penalty, VmPenaltyItem>(item, itemPenalty, new string[] { "Rfq" });
                    listPenalty.Add(itemPenalty);
                }
            }
            resultObject.Penalty = listPenalty;

            var listRfqPriceSchedule = new List<VmRfqPriceScheduleItem>();
            if (dbResult.RfqPriceSchedule != null)
            {
                foreach (var item in dbResult.RfqPriceSchedule.Where(e => e.IsActive == true).ToList())
                {
                    var itemRfqPriceSchedule = new VmRfqPriceScheduleItem();
                    Copy<Com.BudgetMetal.DBEntities.RfqPriceSchedule, VmRfqPriceScheduleItem>(item, itemRfqPriceSchedule, new string[] { "Rfq" });
                    listRfqPriceSchedule.Add(itemRfqPriceSchedule);
                }
            }
            resultObject.RfqPriceSchedule = listRfqPriceSchedule;

            //if (dbResult.Document.DocumentActivity != null)
            //{
            //    var listDocumentActivity = new List<VmDocumentActivityItem>();
            //    foreach (var item in dbResult.Document.DocumentActivity)
            //    {
            //        var newItem = new VmDocumentActivityItem();
            //        newItem.Action = item.Action;
            //        newItem.CreatedBy = item.CreatedBy;
            //        newItem.CreatedDate = item.CreatedDate;
            //        newItem.Document_Id = item.Document_Id;
            //        listDocumentActivity.Add(newItem);
            //    }
            //    resultObject.Document.DocumentActivityList = listDocumentActivity;
            //}

            var dbResultQuotationList = repoQuotation.GetQuotationByRfqId(documentId);

            List<List<string>> requirementComparisonList = new List<List<string>>();
            List<List<string>> priceComparisonList = new List<List<string>>();
            foreach (var item in dbResultQuotationList.Result)
            {
                List<string> requirementComparison = new List<string>();
                requirementComparison.Add(item.Document.Company.Name);
                foreach (var requirementItem in item.QuotationRequirement.Where(e => e.IsActive == true).ToList())
                {
                    requirementComparison.Add(requirementItem.Compliance);
                }
                requirementComparisonList.Add(requirementComparison);

                List<string> priceComparison = new List<string>();
                priceComparison.Add(item.Document.Company.Name);
                foreach (var priceItem in item.QuotationPriceSchedule.Where(e => e.IsActive == true).ToList())
                {
                    priceComparison.Add(priceItem.ItemAmount.ToString());
                }
                priceComparisonList.Add(priceComparison);
            }

            resultObject.RequirementComparison = requirementComparisonList;
            resultObject.PriceComparison = priceComparisonList;

            return resultObject;
        }

        public async Task<VmRfqPage> GetPublicRfqByCompany(int page, int companyId, int totalRecords, int statusId = 0, string keyword = "")
        {
            var dbPageResult = await repoRfq.GetPublicRfqByCompany((page == 0 ? Constants.app_firstPage : page), companyId,
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords), statusId, keyword);

            if (dbPageResult == null)
            {
                return new VmRfqPage();
            }

            var resultObj = new VmRfqPage();
            resultObj.RequestId = DateTime.Now.ToString("yyyyMMddHHmmss");
            resultObj.RequestDate = DateTime.Now;
            resultObj.Result = new PageResult<VmRfqItem>();
            resultObj.Result.Records = new List<VmRfqItem>();

            Copy<PageResult<Rfq>, PageResult<VmRfqItem>>(dbPageResult, resultObj.Result, new string[] { "Records" });

            foreach (var dbItem in dbPageResult.Records)
            {
                var resultItem = new VmRfqItem();

                Copy<Rfq, VmRfqItem>(dbItem, resultItem);

                if (dbItem.Document != null)
                {
                    resultItem.Document = new ViewModels.Document.VmDocumentItem()
                    {
                        DocumentNo = dbItem.Document.DocumentNo,
                        Title = dbItem.Document.Title,
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


        public async Task<List<VmCompanyItem>> LoadSelectedSupplier(int rfqId)
        {
            var listCompany = new List<VmCompanyItem>();

            var dbResult = await repoRfq.GetSelectedSupplier(rfqId);
            if (dbResult != null)
            {
                foreach (var item in dbResult)
                {
                    var resultItem = new VmCompanyItem();
                    Copy<Com.BudgetMetal.DBEntities.Company, VmCompanyItem>(item, resultItem);
                    listCompany.Add(resultItem);
                }
            }

            return listCompany;
        }

        public async Task<VmGenericServiceResult> CheckQuotationByRfqId(int rfqId, int companyId)
        {
            var result = new VmGenericServiceResult();

            var dbresult = await repoQuotation.GetQuotationBy_RfqId_CompanyId(rfqId, companyId);

            if (dbresult == null)
            {
                result.IsSuccess = false;
                result.MessageToUser = "";
            }
            else
            {
                result.IsSuccess = true;
                result.MessageToUser = dbresult.Id.ToString();
            }

            return result;
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

        public async Task<VmGenericServiceResult> CheckPermissionForRFQ(int companyId, int C_BussinessType, int userId, int RfqId, bool companyAdmin)
        {
            var result = new VmGenericServiceResult();

            var rfq = await repoRfq.GetSingleRfqById(RfqId);

            result.IsSuccess = false;
            if (rfq != null)
            {
                if (rfq.IsPublic)
                {
                    result.IsSuccess = true;
                }
                else { 
                if (C_BussinessType == Constants_CodeTable.Code_C_Buyer)
                {
                    if (rfq.Document.Company_Id == companyId)
                    {
                        if (companyAdmin)
                        {
                            result.IsSuccess = true;
                        }
                        else
                        {
                            if (rfq.Document.DocumentUser != null)
                            {
                                if (rfq.Document.DocumentUser.Where(e => e.IsActive == true && e.User_Id == userId).ToList().Count > 0)
                                {
                                    result.IsSuccess = true;
                                }
                            }
                        }
                    }
                }
                else if (C_BussinessType == Constants_CodeTable.Code_C_Supplier)
                {
                    if(rfq.InvitedSupplier != null)
                    {
                        if (rfq.InvitedSupplier.Where(e => e.IsActive == true && e.Company_Id == companyId).ToList().Count > 0)
                        {
                            result.IsSuccess = true;
                        }
                    }
                }
                }
            }
            return result;
        }
    }
}
