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
using Com.BudgetMetal.ViewModels.EzyTender;
using Com.BudgetMetal.ViewModels.Rfq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public RFQService(IDocumentRepository repoDocument, IRfqRepository repoRfq, IAttachmentRepository repoAttachment, IRequirementRepository repoRequirement, ISlaRepository repoSla, IRfqPriceScheduleRepository repoRfqPriceSchedule, IPenaltyRepository repoPenalty, IInvitedSupplierRepository repoInvitedSupplier)
        {
            this.repoDocument = repoDocument;
            this.repoRfq = repoRfq;
            this.repoAttachment = repoAttachment;
            this.repoRequirement = repoRequirement;
            this.repoSla = repoSla;
            this.repoRfqPriceSchedule = repoRfqPriceSchedule;
            this.repoPenalty = repoPenalty;
            this.repoInvitedSupplier = repoInvitedSupplier;
        }

        public string SaveRFQ(VmRfq rfq)
        {
            var dbDocument = new Com.BudgetMetal.DBEntities.Document();
            string documentNo = "RFQ_" + rfq.Document.Company_Id.ToString().PadLeft(4, '0') + "_" + DateTime.Now.ToString("yyyyMMddhhmmss");
            rfq.Document.DocumentNo = documentNo;
            Copy<VmDocument, Com.BudgetMetal.DBEntities.Document>(rfq.Document, dbDocument);
            repoDocument.Add(dbDocument);
            repoDocument.Commit();

            rfq.Document_Id = dbDocument.Id;

            var dbRFQ = new Com.BudgetMetal.DBEntities.Rfq();
            Copy<VmRfq, Com.BudgetMetal.DBEntities.Rfq>(rfq, dbRFQ);
            repoRfq.Add(dbRFQ);
            repoRfq.Commit();


            if (rfq.Document.Attachment.Count > 0)
            {
                foreach (var item in rfq.Document.Attachment)
                {
                    var dbAttachment = new Com.BudgetMetal.DBEntities.Attachment();

                    Copy<VmAttachment, Com.BudgetMetal.DBEntities.Attachment>(item, dbAttachment);
                    dbAttachment.Document_Id = dbDocument.Id;
                    dbAttachment.CreatedBy = dbAttachment.UpdatedBy = dbRFQ.CreatedBy;
                    repoAttachment.Add(dbAttachment);
                }
                repoAttachment.Commit();
            }

            if (rfq.Requirement.Count > 0)
            {
                foreach (var item in rfq.Requirement)
                {
                    if (item.ServiceName != null && item.Quantity != null && item.Description != null)
                    {
                        var dbRequirement = new Com.BudgetMetal.DBEntities.Requirement();

                        Copy<VmRequirement, Com.BudgetMetal.DBEntities.Requirement>(item, dbRequirement);
                        dbRequirement.Rfq_Id = dbRFQ.Id;
                        dbRequirement.CreatedBy = dbRequirement.UpdatedBy = dbRFQ.CreatedBy;
                        repoRequirement.Add(dbRequirement);
                    }

                }
                repoRequirement.Commit();
            }

            if (rfq.Sla.Count > 0)
            {
                foreach (var item in rfq.Sla)
                {
                    if (item.Requirement != null && item.Description != null)
                    {
                        var dbSla = new Com.BudgetMetal.DBEntities.Sla();

                        Copy<VmSla, Com.BudgetMetal.DBEntities.Sla>(item, dbSla);
                        dbSla.Rfq_Id = dbRFQ.Id;
                        dbSla.CreatedBy = dbSla.UpdatedBy = dbRFQ.CreatedBy;
                        repoSla.Add(dbSla);
                    }

                }
                repoSla.Commit();
            }


            if (rfq.Penalty.Count > 0)
            {
                foreach (var item in rfq.Penalty)
                {
                    if (item.BreachOfServiceDefinition != null && item.PenaltyAmount != null && item.Description != null)
                    {
                        var dbPenalty = new Com.BudgetMetal.DBEntities.Penalty();

                        Copy<VmPenalty, Com.BudgetMetal.DBEntities.Penalty>(item, dbPenalty);
                        dbPenalty.Rfq_Id = dbRFQ.Id;
                        dbPenalty.CreatedBy = dbPenalty.UpdatedBy = dbRFQ.CreatedBy;
                        repoPenalty.Add(dbPenalty);
                    }

                }
                repoPenalty.Commit();
            }

            if (rfq.RfqPriceSchedule.Count > 0)
            {
                foreach (var item in rfq.RfqPriceSchedule)
                {
                    if (item.ItemName != null && item.ItemDescription != null && item.InternalRefrenceCode != null && item.QuantityRequired != null)
                    {
                        var dbRfqPriceSchedule = new Com.BudgetMetal.DBEntities.RfqPriceSchedule();

                        Copy<VmRfqPriceSchedule, Com.BudgetMetal.DBEntities.RfqPriceSchedule>(item, dbRfqPriceSchedule);
                        dbRfqPriceSchedule.Rfq_Id = dbRFQ.Id;
                        dbRfqPriceSchedule.CreatedBy = dbRfqPriceSchedule.UpdatedBy = dbRFQ.CreatedBy;
                        repoRfqPriceSchedule.Add(dbRfqPriceSchedule);
                    }

                }
                repoRfqPriceSchedule.Commit();
            }

            if (rfq.InvitedSupplier.Count > 0)
            {
                foreach (var item in rfq.InvitedSupplier)
                {
                    var dbInvitedSupplier = new Com.BudgetMetal.DBEntities.InvitedSupplier();

                    Copy<VmInvitedSupplier, Com.BudgetMetal.DBEntities.InvitedSupplier>(item, dbInvitedSupplier);
                    dbInvitedSupplier.Rfq_Id = dbRFQ.Id;
                    dbInvitedSupplier.CreatedBy = dbInvitedSupplier.UpdatedBy = dbRFQ.CreatedBy;
                    repoInvitedSupplier.Add(dbInvitedSupplier);
                }
                repoInvitedSupplier.Commit();
            }


            return documentNo;
        }

        public async Task<VmRfqPage> GetRfqByPage(string keyword, int page, int totalRecords)
        {
            var dbPageResult = await repoRfq.GetPage(keyword,
                (page == 0 ? Constants.app_firstPage : page),
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords));

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
                        Title = dbItem.Document.Title
                    };
                }

                resultObj.Result.Records.Add(resultItem);
            }

            return resultObj;
        }

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
    }
}
