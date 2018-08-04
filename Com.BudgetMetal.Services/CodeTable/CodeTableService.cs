using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Code_Category;
using Com.BudgetMetal.DataRepository.Code_Table;
using Com.BudgetMetal.DBEntities;
using Com.BudgetMetal.Services.Base;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.CodeTable;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Com.BudgetMetal.ViewModels.CodeCategory;

namespace Com.BudgetMetal.Services.Code_Table
{
    public class CodeTableService : BaseService, ICodeTableService
    {
        private readonly ICodeTableRepository repo;
        private readonly ICodeCategoryRepository repoCate;

        public CodeTableService(ICodeTableRepository repo, ICodeCategoryRepository repoC)
        {
            this.repo = repo;
            this.repoCate = repoC;
        }

        public async Task<VmCodeTablePage> GetCodeTableByPage(string keyword, int page, int totalRecords)
        {
            var dbPageResult = await repo.GetPage(keyword,
                (page == 0 ? Constants.app_firstPage : page),
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords));

            //var dbPageResult = repo.GetCodeTableByPage(keyword,
            //    (page == 0 ? Constants.app_firstPage : page),
            //    (totalRecords == 0 ? Constants.app_totalRecords : totalRecords));

            if (dbPageResult == null)
            {
                return new VmCodeTablePage();
            }

            var resultObj = new VmCodeTablePage();
            resultObj.RequestId = DateTime.Now.ToString("yyyyMMddHHmmss");
            resultObj.RequestDate = DateTime.Now;
            resultObj.Result = new PageResult<VmCodeTableItem>();
            resultObj.Result.Records = new List<VmCodeTableItem>();

            Copy<PageResult<CodeTable>, PageResult<VmCodeTableItem>>(dbPageResult, resultObj.Result, new string[] { "Records" });

            foreach (var dbItem in dbPageResult.Records)
            {
                var resultItem = new VmCodeTableItem();

                Copy<CodeTable, VmCodeTableItem>(dbItem, resultItem);

                if (dbItem.CodeCategory != null)
                {
                    resultItem.CodeCategory = new ViewModels.CodeCategory.VmCodeCategoryItem()
                    {
                        Name = dbItem.CodeCategory.Name
                    };
                }

                resultObj.Result.Records.Add(resultItem);
            }

            return resultObj;
        }

        //public async Task<List<VmCodeCategoryItem>> GetCodeCategory()
        //{   
        //    List<VmCodeCategoryItem> lstCodeCategory = new List<VmCodeCategoryItem>();

        //    var result = await repoCate.GetAll();

        //    foreach (var item in result)
        //    {
        //        var resultItem = new VmCodeCategoryItem();

        //        Copy<CodeCategory, VmCodeCategoryItem>(item, resultItem);

        //        lstCodeCategory.Add(resultItem);
        //    }
            
        //    return lstCodeCategory;
        //}

        public async Task<VmCodeTableItem> GetCodeTableById(int Id)
        {
            var dbPageResult = await repo.Get(Id);

            if (dbPageResult == null)
            {
                return new VmCodeTableItem();
            }

            var resultObj = new VmCodeTableItem();

            Copy<CodeTable, VmCodeTableItem>(dbPageResult, resultObj);

            return resultObj;
        }

        public VmGenericServiceResult Insert(VmCodeTableItem vmCodeTableItem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                CodeTable r = new CodeTable();

                Copy<VmCodeTableItem, CodeTable>(vmCodeTableItem, r);

                r.Id = 1;// repo.GetLastId();

                if (r.CreatedBy.IsNullOrEmpty())
                {
                    r.CreatedBy = r.UpdatedBy = "System";
                }
                repo.Add(r);

                repo.Commit();

                result.IsSuccess = true;
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Error = e;
            }

            return result;
        }

        public async Task<VmGenericServiceResult> Update(VmCodeTableItem vmCodeTableItem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                CodeTable r = await repo.Get(vmCodeTableItem.Id);

                Copy<VmCodeTableItem, CodeTable>(vmCodeTableItem, r);

                if (r.UpdatedBy.IsNullOrEmpty())
                {
                    r.UpdatedBy = "System";
                }

                repo.Update(r);

                repo.Commit();

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
            CodeTable r = await repo.Get(Id);
            r.IsActive = false;
            repo.Update(r);
            repo.Commit();
        }

        public async Task<VmCodeTableItem> GetFormObject()
        {
            VmCodeTableItem result = new VmCodeTableItem();
            
            var dbCatList = await repoCate.GetAll();

            if (dbCatList == null) return result;

            result.CodeCategoryList = new List<VmCodeCategoryItem>();

            foreach (var dbcat in dbCatList)
            {
                VmCodeCategoryItem cat = new VmCodeCategoryItem() {
                    Id = dbcat.Id,
                    Name = dbcat.Name
                };

                result.CodeCategoryList.Add(cat);
            }

            return result;
        }
    }
}
