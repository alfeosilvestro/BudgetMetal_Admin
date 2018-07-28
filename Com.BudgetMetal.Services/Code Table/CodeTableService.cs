using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Code_Table;
using Com.BudgetMetal.DBEntities;
using Com.BudgetMetal.Services.Base;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.CodeTable;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.Services.Code_Table
{
    public class CodeTableService : BaseService, ICodeTableService
    {
        private readonly ICodeTableRepository repo;

        public CodeTableService(ICodeTableRepository repo)
        {
            this.repo = repo;
        }

        public VmCodeTablePage GetCodeTableByPage(string keyword, int page, int totalRecords)
        {
            
            var dbPageResult = repo.GetCodeTableByPage(keyword,
                (page == 0 ? Constants.app_firstPage : page),
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords));

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

                resultObj.Result.Records.Add(resultItem);
            }

            return resultObj;
        }

        public VmCodeTableItem GetCodeTableById(int Id)
        {
            var dbPageResult = repo.GetCodeTableById(Id);

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

        public VmGenericServiceResult Update(VmCodeTableItem vmCodeTableItem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                CodeTable r = repo.GetCodeTableById(vmCodeTableItem.Id);

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

        public void Delete(int Id)
        {
            CodeTable r = repo.GetCodeTableById(Id);
            r.IsActive = false;
            repo.Update(r);
            repo.Commit();
        }
    }
}
