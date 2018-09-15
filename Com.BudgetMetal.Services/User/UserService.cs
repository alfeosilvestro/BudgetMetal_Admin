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
using Com.BudgetMetal.DataRepository.Roles;
using Com.BudgetMetal.ViewModels.Role;
using Com.BudgetMetal.DataRepository.UserRoles;
using Com.BudgetMetal.ViewModels.Sys_User;

namespace Com.BudgetMetal.Services.Users
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository repo;
        private readonly ICompanyRepository cRepo;
        private readonly ICodeTableRepository CTrepo;
        private readonly IRoleRepository roleRepo;
        private readonly IUserRolesRepository userRolesRepo;

        public UserService(IUserRepository repo, ICompanyRepository _cRepo, ICodeTableRepository _ctRepo, IRoleRepository _roleRepo, IUserRolesRepository _userRoleRepo)
        {
            this.repo = repo;
            cRepo = _cRepo;
            CTrepo = _ctRepo;
            roleRepo = _roleRepo;
            userRolesRepo = _userRoleRepo;
        }

        public async Task<VmUserItem> ValidateUser(VM_Sys_User_Sign_In user)
        {
            var dbresult = await repo.GetUser(user.UserName, user.Password);
           
            if (dbresult == null)
            {
                return new VmUserItem();
            }
            else
            {
                var result = new VmUserItem();
                Copy<Com.BudgetMetal.DBEntities.User, VmUserItem>(dbresult, result);

                return result;
            }

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
            var dbPageResult = await repo.GetUserById(Id);

            if (dbPageResult == null)
            {
                return new VmUserItem();
            }
           
            var result = new VmUserItem();

            Copy<Com.BudgetMetal.DBEntities.User, VmUserItem>(dbPageResult, result);

            var dbCList = await cRepo.GetAll();

            //filter with companyId
            var dbCCList = await CTrepo.GetAll();

            var dbRoleList = await roleRepo.GetAll();

            if (dbCList == null && dbCCList == null && dbRoleList == null) { return result; }

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
            if (dbRoleList != null)
            {
                result.RoleList = new List<VmRoleItem>();

                foreach (var item in dbRoleList)
                {
                    VmRoleItem _roleItem = new VmRoleItem()
                    {
                        Id = item.Id,
                        Name = item.Name
                    };

                    result.RoleList.Add(_roleItem);
                }
            }

            if(dbPageResult.UserRoles != null)
            {
                if (dbPageResult.UserRoles.Count > 0)
                {
                    result.SelectedRoleId = new List<int>();
                    foreach (var item in dbPageResult.UserRoles)
                    {
                        int roleId = item.Role_Id;
                        result.SelectedRoleId.Add(roleId);
                    }

                }
                else
                {
                    //Add zero
                    result.SelectedRoleId = new List<int>();
                    result.SelectedRoleId.Add(0);
                }
            }

            return result;
        }

        public async Task<VmGenericServiceResult> Insert(VmUserItem vObj)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                Com.BudgetMetal.DBEntities.User dbObj = new Com.BudgetMetal.DBEntities.User();

                Copy<VmUserItem, Com.BudgetMetal.DBEntities.User>(vObj, dbObj);

                #region comments
                // r.Industry_Id = 1;//hard code
                //var user = new[]
                //{
                //    new User { Id = r.Id },
                //    //new User {Id = 2}
                //};
                ////var role = new Role { Id = 1 };
                ////var role1 = new Role { Id = 2 };

                //var role = new[]
                //{
                //    new Role {Id=1},
                //    new Role {Id=2}
                //};
                //r.UserRoles.Add(new UserRoles { User = user[0], Role = role[0] });

                //r.UserRoles.Add(new UserRoles { User = user, Role = role1 });


                //foreach (var post in UserRoles)
                //{
                //    var oldPostTag = post.PostTags.FirstOrDefault(e => e.Tag.Text == "Pineapple");
                //    if (oldPostTag != null)
                //    {
                //        post.PostTags.Remove(oldPostTag);
                //        post.PostTags.Add(new PostTag { Post = post, Tag = newTag1 });
                //    }
                //    post.PostTags.Add(new PostTag { Post = post, Tag = newTag2 });
                //}
                //UserRoles _userRole = new UserRoles();
                ////_userRole.User = user[0];
                //_userRole.User_Id = r.Id;
                //_userRole.Role_Id = 1;
                ////_userRole.Role = role[0];
                //userRolesRepo.Add(_userRole);
                //userRolesRepo.Commit();
                #endregion

                if (vObj.RoleList != null && vObj.RoleList.Count > 0)
                {
                    foreach (var vUsrRole in vObj.RoleList)
                    {
                        var dbRoleObj = await roleRepo.Get(vUsrRole.Id);

                        if (dbRoleObj != null)
                        {
                            UserRoles ur = new UserRoles();
                            ur.Role = dbRoleObj;
                            ur.PrepareNewRecord("System");
                            dbObj.UserRoles.Add(ur);
                        }
                    }

                }
                dbObj.PrepareNewRecord("System");

                repo.Add(dbObj);

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

        public async Task<VmGenericServiceResult> Update(VmUserItem vObj)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                Com.BudgetMetal.DBEntities.User dbObj = await repo.Get(vObj.Id);

                Copy<VmUserItem, Com.BudgetMetal.DBEntities.User>(vObj, dbObj);

                if (vObj.RoleList != null && vObj.RoleList.Count > 0)
                {
                    foreach (var vUsrRole in vObj.RoleList)
                    {
                        var dbRoleObj = await roleRepo.Get(vUsrRole.Id);

                        if (dbRoleObj != null)
                        {
                            UserRoles ur = new UserRoles();
                            ur.Role = dbRoleObj;
                            ur.PrepareNewRecord("System");
                            dbObj.UserRoles.Add(ur);
                        }
                    }

                }
                dbObj.PrepareNewRecord("System");
                repo.Update(dbObj);

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

            var dbRoleList = await roleRepo.GetAll();

            if (dbCList == null && dbCCList == null && dbRoleList == null) { return result; }

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
            if (dbRoleList != null)
            {
                result.RoleList = new List<VmRoleItem>();

                foreach (var item in dbRoleList)
                {
                    VmRoleItem _roleItem = new VmRoleItem()
                    {
                        Id = item.Id,
                        Name = item.Name
                    };

                    result.RoleList.Add(_roleItem);
                }
            }

            return result;
        }

        public async Task<List<VmUserItem>> GetUserByCompany(int Id)
        {
            var dbResult = await repo.GetUserByCompany(Id);
            var resultList = new List<VmUserItem>();

            foreach (var item in dbResult)
            {
                var result = new VmUserItem();

                Copy<Com.BudgetMetal.DBEntities.User, VmUserItem>(item, result);

                resultList.Add(result);
            }


            return resultList;
        }
    }
}
