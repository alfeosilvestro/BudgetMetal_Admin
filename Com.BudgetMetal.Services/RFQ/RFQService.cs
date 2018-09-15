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
        
        public RFQService(IDocumentRepository repoDocument, IRfqRepository repoRfq, IAttachmentRepository repoAttachment, IRequirementRepository repoRequirement, ISlaRepository repoSla, IRfqPriceScheduleRepository repoRfqPriceSchedule, IPenaltyRepository repoPenalty, IInvitedSupplierRepository repoInvitedSupplier, IDocumentUserRepository repoDocumentUser, IUserRepository repoUser, IRoleRepository repoRole, ICompanyRepository repoCompany, IDocumentActivityRepository repoDocumentActivity, IQuotationRepository repoQuotation)
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
            this.repoCompany =  repoCompany;
            this.repoDocumentActivity = repoDocumentActivity;
            this.repoQuotation = repoQuotation;
        }

        public async Task<VmRfqPage> GetRfqByPage(int documentOwner, int page,int totalRecords, int statusId = 0, string keyword = "")
        {
            var dbPageResult = await repoRfq.GetRfqByPage(documentOwner,
                (page == 0 ? Constants.app_firstPage : page),
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords),statusId, keyword);

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

            int RFQLimitPerWeek = (dbResult.Result.MaxRFQPerWeek == null)? 0: Convert.ToInt32( dbResult.Result.MaxRFQPerWeek);

            bool RFQLimit = true;

            if(documentCount >= RFQLimitPerWeek)
            {
                RFQLimit = false;
            }

            return RFQLimit;
        }


        public string SaveRFQ(VmRfqItem rfq)
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
                    }
                    repoInvitedSupplier.Commit();
                }
            }

            if(rfq.Document.DocumentActivityList != null)
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

            return documentNo;
        }

        public string UpdateRFQ(VmRfqItem rfq)
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
                    }
                    repoInvitedSupplier.Commit();
                }
            }

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

            return rfq.Document.DocumentNo;
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
            if(dbResult.Document.Attachment != null)
            {
                foreach(var item in dbResult.Document.Attachment.Where(e => e.IsActive == true).ToList())
                {
                    var itemAttachment = new VmAttachmentItem();
                    Copy<Com.BudgetMetal.DBEntities.Attachment, VmAttachmentItem>(item, itemAttachment, new string[] { "Document" , "FileBinary" });
                    listAttachment.Add(itemAttachment);
                }
            }
            resultDocument.Attachment = listAttachment;

            var listDocumentUser = new List<VmDocumentUserDisplay>();
            if (dbResult.Document.DocumentUser != null)
            {
                List<int> UserList = new List<int>();
                UserList = dbResult.Document.DocumentUser.Where(e=>e.IsActive ==  true).Select(e => e.User_Id).Distinct().ToList();
                foreach(var itemUser in UserList)
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


            //var documentActivityEntity = repoDocumentActivity.GetDocumentActivityWithDocumentId(dbResult.Document_Id, true);
            //var listDocumentActivity = new List<VmDocumentActivityItem>();
            //if (documentActivityEntity != null)
            //{
            //    foreach (var item in documentActivityEntity.Result.Records)
            //    {
            //        var newItem = new VmDocumentActivityItem();
            //        newItem.Action = item.Action;
            //        newItem.CreatedBy = item.CreatedBy;
            //        newItem.CreatedDate = item.CreatedDate;
            //        newItem.Document_Id = item.Document_Id;
            //        listDocumentActivity.Add(newItem);
            //    }
            //}
            //resultObject.DocumentActivityList = listDocumentActivity;

            //if (documentActivityEntity != null)
            //{
            //    var listDocumentActivity = new List<VmDocumentActivityItem>();
            //    foreach (var item in documentActivityEntity.Result.Records)
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

            //var listInvitedSupplier = new List<VmInvitedSupplierItem>();
            //if (dbResult.InvitedSupplier != null)
            //{
            //    foreach (var item in dbResult.InvitedSupplier)
            //    {
            //        var itemInvitedSupplier = new VmInvitedSupplierItem();
            //        Copy<Com.BudgetMetal.DBEntities.InvitedSupplier, VmInvitedSupplierItem>(item, itemInvitedSupplier, new string[] { "Rfq" });
            //        listInvitedSupplier.Add(itemInvitedSupplier);
            //    }
            //}

            var dbResultQuotationList = repoQuotation.GetQuotationByRfqId(documentId);
            
            List<List<string>> requirementComparisonList = new List<List<string>>();
            List<List<string>> priceComparisonList = new List<List<string>>();
            foreach (var item in dbResultQuotationList.Result)
            {
                List<string> requirementComparison = new List<string>();
                requirementComparison.Add(item.Document.Company.Name); 
                foreach(var requirementItem in item.QuotationRequirement.Where(e => e.IsActive == true).ToList())
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
    }
}
