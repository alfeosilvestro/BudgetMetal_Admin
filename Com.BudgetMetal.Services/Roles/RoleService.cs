using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Roles;
using Com.BudgetMetal.DataRepository.Single_Sign_On;
using Com.BudgetMetal.DBEntities;
using Com.BudgetMetal.Services.Base;
using Com.BudgetMetal.ViewModels.Role;
using System.Collections.Generic;
using System;
using Com.BudgetMetal.ViewModels;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.Roles
{
    public class RoleService : BaseService, IRoleService
    {
        private readonly IRoleRepository repo;

        public RoleService(IRoleRepository repo)
        {
            this.repo = repo;
        }

        public async Task<VmRolePage> GetRolesByPage(string keyword, int page, int totalRecords)
        {
            var dbPageResult = await repo.GetPage(keyword,
                (page == 0 ? Constants.app_firstPage : page),
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords));

            if (dbPageResult == null)
            {
                return new VmRolePage();
            }

            var resultObj = new VmRolePage();
            //resultObj.ApplicationToken = applicationToken;
            resultObj.RequestId = DateTime.Now.ToString("yyyyMMddHHmmss");
            resultObj.RequestDate = DateTime.Now;
            resultObj.Result = new PageResult<VmRoleItem>();
            resultObj.Result.Records = new List<VmRoleItem>();

            Copy<PageResult<Role>, PageResult<VmRoleItem>>(dbPageResult, resultObj.Result, new string[] { "Records" });

            foreach (var dbItem in dbPageResult.Records)
            {
                var resultItem = new VmRoleItem();

                Copy<Role, VmRoleItem>(dbItem, resultItem);

                resultObj.Result.Records.Add(resultItem);
            }

            return resultObj;
        }

        public async Task<VmRoleItem> GetRoleById(int Id)
        {
            var dbPageResult = await repo.Get(Id);

            if (dbPageResult == null)
            {
                return new VmRoleItem();
            }

            var resultObj = new VmRoleItem();

            Copy<Role, VmRoleItem>(dbPageResult, resultObj);

            return resultObj;
        }

        public VmGenericServiceResult Insert(VmRoleItem vmRoleItem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                Role r = new Role();

                Copy<VmRoleItem, Role>(vmRoleItem, r);

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

        public async Task<VmGenericServiceResult> Update(VmRoleItem vmRoleItem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                Role r = await repo.Get(vmRoleItem.Id);

                Copy<VmRoleItem, Role>(vmRoleItem, r);

                if (r.UpdatedBy.IsNullOrEmpty())
                {
                    r.UpdatedBy = "System";
                }

                repo.Update(r);

                repo.Commit();

                result.IsSuccess = true;
            }
            catch (Exception e) {
                result.IsSuccess = false;
                result.Error = e;
            }

            return result;
        }

        public async Task Delete(int Id)
        {
            Role r = await repo.Get(Id);
            r.IsActive = false;
            repo.Update(r);
            repo.Commit();
        }

        public async Task<List<VmRoleItem>> GetActiveRoles(string roleType)
        {
            var dbResult = await repo.GetAll() ;

            var resultList = new List<VmRoleItem>();
            
            
           
            foreach (var dbItem in dbResult)
            {
                var resultItem = new VmRoleItem();

               
                if (roleType == "rfq")
                {
                    if (dbItem.Code.StartsWith("R_"))
                    {
                        Copy<Role, VmRoleItem>(dbItem, resultItem);
                        resultList.Add(resultItem);
                    }
                   
                }
                else if (roleType == "quotation")
                {
                    if (dbItem.Code.StartsWith("Q_"))
                    {
                        Copy<Role, VmRoleItem>(dbItem, resultItem);
                        resultList.Add(resultItem);
                    }
                }
                else
                {
                    if ((!dbItem.Code.StartsWith("Q_")) ||(!dbItem.Code.StartsWith("R_")))
                    {
                        Copy<Role, VmRoleItem>(dbItem, resultItem);
                        resultList.Add(resultItem);
                    }
                }
                
            }
            return resultList;

        }
    }
}
