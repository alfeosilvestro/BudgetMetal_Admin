using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Code_Table;
using Com.BudgetMetal.DataRepository.Company;
using Com.BudgetMetal.DataRepository.Document;
using Com.BudgetMetal.Services.Base;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.CodeTable;
using Com.BudgetMetal.ViewModels.Company;
using Com.BudgetMetal.ViewModels.Document;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.Document
{
    public class DocumentService : BaseService, IDocumentService
    {
        private readonly IDocumentRepository documentRepo;
        private readonly ICodeTableRepository codeTableRepo;
        private readonly ICompanyRepository companyRepo;

        public DocumentService(IDocumentRepository documentRepo, ICodeTableRepository codeTableRepo, ICompanyRepository companyRepo)
        {
            this.documentRepo = documentRepo;
            this.codeTableRepo = codeTableRepo;
            this.companyRepo = companyRepo;
        }

        public async Task<VmDocumentPage> GetDocumentByPage(string keyword, int page, int totalRecords)
        {
            var dbPageResult = await documentRepo.GetPage(keyword,
                (page == 0 ? Constants.app_firstPage : page),
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords));

            if (dbPageResult == null)
            {
                return new VmDocumentPage();
            }

            var resultObj = new VmDocumentPage();
            resultObj.RequestId = DateTime.Now.ToString("yyyyMMddHHmmss");
            resultObj.RequestDate = DateTime.Now;
            resultObj.Result = new PageResult<VmDocumentItem>();
            resultObj.Result.Records = new List<VmDocumentItem>();

            Copy<PageResult<Com.BudgetMetal.DBEntities.Document>, PageResult<VmDocumentItem>>(dbPageResult, resultObj.Result, new string[] { "Records" });

            foreach (var dbItem in dbPageResult.Records)
            {
                var resultItem = new VmDocumentItem();

                Copy<Com.BudgetMetal.DBEntities.Document, VmDocumentItem>(dbItem, resultItem);

                if (dbItem.DocumentStatus != null)
                {
                    resultItem.DocumentStatus = new ViewModels.CodeTable.VmCodeTableItem()
                    {
                        Name = dbItem.DocumentStatus.Name
                    };
                }

                if (dbItem.DocumentType != null)
                {
                    resultItem.DocumentType = new ViewModels.CodeTable.VmCodeTableItem()
                    {
                        Name = dbItem.DocumentType.Name
                    };
                }

                if (dbItem.Company != null)
                {
                    resultItem.Company = new ViewModels.Company.VmCompanyItem()
                    {
                        Name = dbItem.Company.Name
                    };
                }

                resultObj.Result.Records.Add(resultItem);
            }

            return resultObj;
        }

        public async Task<VmDocumentItem> GetDocumentById(int Id)
        {
            var dbPageResult = await documentRepo.Get(Id);

            if (dbPageResult == null)
            {
                return new VmDocumentItem();
            }

            var resultObj = new VmDocumentItem();

            Copy<Com.BudgetMetal.DBEntities.Document, VmDocumentItem>(dbPageResult, resultObj);

            var dbCodeTableList = await codeTableRepo.GetAll();

            if (dbCodeTableList == null) return resultObj;

            resultObj.DocumentStatusList = new List<VmCodeTableItem>();
            resultObj.DocumentTypeList = new List<VmCodeTableItem>();

            foreach (var dbcat in dbCodeTableList)
            {
                VmCodeTableItem documentStatusandType = new VmCodeTableItem()
                {
                    Id = dbcat.Id,
                    Name = dbcat.Name
                };

                resultObj.DocumentStatusList.Add(documentStatusandType);
                resultObj.DocumentTypeList.Add(documentStatusandType);
            }

            var dbCompanyList = await companyRepo.GetAll();

            if (dbCompanyList == null) return resultObj;

            resultObj.CompanyList = new List<VmCompanyItem>();

            foreach (var dbcat in dbCompanyList)
            {
                VmCompanyItem company = new VmCompanyItem()
                {
                    Id = dbcat.Id,
                    Name = dbcat.Name
                };

                resultObj.CompanyList.Add(company);
            }

            return resultObj;
        }

        public VmGenericServiceResult Insert(VmDocumentItem vmItem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                Com.BudgetMetal.DBEntities.Document entity = new Com.BudgetMetal.DBEntities.Document();

                Copy<VmDocumentItem, Com.BudgetMetal.DBEntities.Document>(vmItem, entity);

                entity.Company_Id = (int)vmItem.Company_Id;
                entity.DocumentStatus_Id = (int)vmItem.DocumentStatus_Id;
                entity.DocumentType_Id = (int)vmItem.DocumentType_Id;
                if (entity.CreatedBy.IsNullOrEmpty())
                {
                    entity.CreatedBy = entity.UpdatedBy = "System";
                }
                documentRepo.Add(entity);

                documentRepo.Commit();

                result.IsSuccess = true;
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Error = e;
            }

            return result;
        }

        public async Task<VmGenericServiceResult> Update(VmDocumentItem vmItem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                Com.BudgetMetal.DBEntities.Document entity = await documentRepo.Get(vmItem.Id);

                Copy<VmDocumentItem, Com.BudgetMetal.DBEntities.Document>(vmItem, entity);

                if (entity.UpdatedBy.IsNullOrEmpty())
                {
                    entity.UpdatedBy = "System";
                }

                documentRepo.Update(entity);

                documentRepo.Commit();

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
            var entity = await documentRepo.Get(Id);
            entity.IsActive = false;
            documentRepo.Update(entity);
            documentRepo.Commit();
        }

        public async Task<VmDocumentItem> GetFormObject()
        {
            VmDocumentItem resultObj = new VmDocumentItem();

            var dbCodeTableList = await codeTableRepo.GetAll();

            if (dbCodeTableList == null) return resultObj;

            resultObj.DocumentStatusList = new List<VmCodeTableItem>();
            resultObj.DocumentTypeList = new List<VmCodeTableItem>();

            foreach (var dbcat in dbCodeTableList)
            {
                VmCodeTableItem documentStatusandType = new VmCodeTableItem()
                {
                    Id = dbcat.Id,
                    Name = dbcat.Name
                };

                resultObj.DocumentStatusList.Add(documentStatusandType);
                resultObj.DocumentTypeList.Add(documentStatusandType);
            }

            var dbCompanyList = await companyRepo.GetAll();

            if (dbCompanyList == null) return resultObj;

            resultObj.CompanyList = new List<VmCompanyItem>();

            foreach (var dbcat in dbCompanyList)
            {
                VmCompanyItem company = new VmCompanyItem()
                {
                    Id = dbcat.Id,
                    Name = dbcat.Name
                };

                resultObj.CompanyList.Add(company);
            }

            return resultObj;
        }
    }
}
