using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Users;
using Com.BudgetMetal.DataRepository.Single_Sign_On;
using Com.BudgetMetal.DBEntities;
using Com.BudgetMetal.Services.Base;
using Com.BudgetMetal.ViewModels.User;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.DataRepository.Company;
using Com.BudgetMetal.DataRepository.Code_Table;
using Com.BudgetMetal.ViewModels.Company;
using Com.BudgetMetal.ViewModels.CodeTable;

namespace Com.BudgetMetal.Services.Users
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository repo;
        private readonly ICompanyRepository cRepo;
        private readonly ICodeTableRepository CTrepo;

        public UserService(IUserRepository repo, ICompanyRepository _cRepo, ICodeTableRepository _ctRepo)
        {
            this.repo = repo;
            cRepo = _cRepo;
            CTrepo = _ctRepo;
        }

        public async Task<VmUserPage> GetUserByPage(string keyword, int page, int totalRecords)
        {
            var dbPageResult = await repo.GetPage(keyword,
                (page == 0 ? Constants.app_firstPage : page),
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords));

            if (dbPageResult == null)
            {
                return new VmUserPage();
            }

            var resultObj = new VmUserPage();
            //resultObj.ApplicationToken = applicationToken;
            resultObj.RequestId = DateTime.Now.ToString("yyyyMMddHHmmss");
            resultObj.RequestDate = DateTime.Now;
            resultObj.Result = new PageResult<VmUserItem>();
            resultObj.Result.Records = new List<VmUserItem>();

            Copy<PageResult<User>, PageResult<VmUserItem>>(dbPageResult, resultObj.Result, new string[] { "Records" });

            foreach (var dbItem in dbPageResult.Records)
            {
                var resultItem = new VmUserItem();

                Copy<User, VmUserItem>(dbItem, resultItem);

                if (dbItem.Company != null)
                {
                    resultItem.Company = new ViewModels.Company.VmCompanyItem()
                    {
                        Name = dbItem.Company.Name
                    };
                }
                if (dbItem.CodeTable != null)
                {
                    resultItem.CodeTable = new ViewModels.CodeTable.VmCodeTableItem()
                    {
                        Name = dbItem.CodeTable.Name
                    };
                }

                resultObj.Result.Records.Add(resultItem);
            }

            return resultObj;
        }

        public async Task<VmUserItem> GetUserById(int Id)
        {
            var dbPageResult = await repo.Get(Id);

            if (dbPageResult == null)
            {
                return new VmUserItem();
            }

            var result = new VmUserItem();

            Copy<Com.BudgetMetal.DBEntities.User, VmUserItem>(dbPageResult, result);

            var dbCList = await cRepo.GetAll();

            //filter with companyId
            var dbCCList = await CTrepo.GetAll();

            if (dbCList == null && dbCCList == null) { return result; }

            if (dbCList != null)
            {
                result.CompanyList = new List<VmCompanyItem>();

                foreach (var dbcat in dbCList)
                {
                    VmCompanyItem _company = new VmCompanyItem()
                    {
                        Id = dbcat.Id,
                        Name = dbcat.Name
                    };

                    result.CompanyList.Add(_company);
                }
            }
            if (dbCCList != null)
            {
                result.CodeTableList = new List<VmCodeTableItem>();

                foreach (var dbcat in dbCCList)
                {
                    VmCodeTableItem _codeTable = new VmCodeTableItem()
                    {
                        Id = dbcat.Id,
                        Name = dbcat.Name
                    };

                    result.CodeTableList.Add(_codeTable);
                }
            }

            return result;
        }

        public VmGenericServiceResult Insert(VmUserItem vmtem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                Com.BudgetMetal.DBEntities.User r = new Com.BudgetMetal.DBEntities.User();

                Copy<VmUserItem, Com.BudgetMetal.DBEntities.User>(vmtem, r);

                if (r.CreatedBy.IsNullOrEmpty())
                {
                    r.CreatedBy = r.UpdatedBy = "System";
                }
                // r.Industry_Id = 1;//hard code
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

        public async Task<VmGenericServiceResult> Update(VmUserItem vmtem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                Com.BudgetMetal.DBEntities.User r = await repo.Get(vmtem.Id);

                Copy<VmUserItem, Com.BudgetMetal.DBEntities.User>(vmtem, r);

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
            Com.BudgetMetal.DBEntities.User r = await repo.Get(Id);
            r.IsActive = false;
            repo.Update(r);
            repo.Commit();
        }

        public async Task<VmUserItem> GetFormObject()
        {
            VmUserItem result = new VmUserItem();

            var dbCList = await cRepo.GetAll();

            //filter with companyId
            var dbCCList = await CTrepo.GetAll();

            if (dbCList == null && dbCCList == null) { return result; }

            if (dbCList != null)
            {
                result.CompanyList = new List<VmCompanyItem>();

                foreach (var dbcat in dbCList)
                {
                    VmCompanyItem _company = new VmCompanyItem()
                    {
                        Id = dbcat.Id,
                        Name = dbcat.Name
                    };

                    result.CompanyList.Add(_company);
                }
            }
            if (dbCCList != null)
            {
                result.CodeTableList = new List<VmCodeTableItem>();

                foreach (var dbcat in dbCCList)
                {
                    VmCodeTableItem _codeTable = new VmCodeTableItem()
                    {
                        Id = dbcat.Id,
                        Name = dbcat.Name
                    };

                    result.CodeTableList.Add(_codeTable);
                }
            }

            return result;
        }

        public async Task<List<VmUserItem>> GetUserByCompany(int Id)
        {
            var dbResult = await repo.GetUserByCompany(Id);
            var resultList = new List<VmUserItem>();

            foreach(var item in dbResult)
            {
                var result = new VmUserItem();

                Copy<Com.BudgetMetal.DBEntities.User, VmUserItem>(item, result);

                resultList.Add(result);
            }


            return resultList;
        }
    }
}
