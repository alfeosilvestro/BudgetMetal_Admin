﻿using Com.BudgetMetal.Common;
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
using Com.BudgetMetal.DataRepository.ServiceTags;
using Com.BudgetMetal.DataRepository.SupplierServiceTags;
using Com.BudgetMetal.ViewModels.UserRoles;

namespace Com.BudgetMetal.Services.Users
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository repo;
        private readonly ICompanyRepository cRepo;
        private readonly ICodeTableRepository CTrepo;
        private readonly IRoleRepository roleRepo;
        private readonly IUserRolesRepository userRolesRepo;
        private readonly ISupplierServiceTagsRepository supplierServiceTagsRepo;

        public UserService(IUserRepository repo, ICompanyRepository _cRepo, ICodeTableRepository _ctRepo, IRoleRepository _roleRepo, IUserRolesRepository _userRoleRepo, ISupplierServiceTagsRepository _supplierServiceTagsRepo)
        {
            this.repo = repo;
            cRepo = _cRepo;
            CTrepo = _ctRepo;
            roleRepo = _roleRepo;
            userRolesRepo = _userRoleRepo;
            supplierServiceTagsRepo = _supplierServiceTagsRepo;
        }


        public async Task<bool> CheckEmail(string email)
        {
            var dbresult = await repo.GetUserByEmail(email);

            if (dbresult == null)
            {
                return false;
            }
            else
            {
                var result = new VmUserItem();
                Copy<Com.BudgetMetal.DBEntities.User, VmUserItem>(dbresult, result);

                return true;
            }
        }

        public async Task<bool> CheckUserName(string UserName)
        {
            var dbresult = await repo.GetUserByUserName(UserName);

            if (dbresult == null)
            {
                return false;
            }
            else
            {
                var result = new VmUserItem();
                Copy<Com.BudgetMetal.DBEntities.User, VmUserItem>(dbresult, result);

                return true;
            }
        }

        public async Task<bool> CheckCurrentPassword(int id, string currentPassword)
        {
            var dbresult = await repo.GetUserById(id);

            if (dbresult == null)
            {
                return false;
            }
            else
            {
                if (dbresult.Password == Md5.Encrypt(currentPassword))
                {
                    var result = new VmUserItem();
                    Copy<Com.BudgetMetal.DBEntities.User, VmUserItem>(dbresult, result);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<VmUserItem> ValidateUser(VM_Sys_User_Sign_In user)
        {
            var dbresult = await repo.GetUser(user.UserName, Md5.Encrypt(user.Password));
           
            if (dbresult == null)
            {
                return new VmUserItem();
            }
            else
            {
                var result = new VmUserItem();
                Copy<Com.BudgetMetal.DBEntities.User, VmUserItem>(dbresult, result);

                if(dbresult.UserRoles != null)
                {
                    var SelectedRoles = new List<VmRoleItem>();
                    foreach(var dbItem in dbresult.UserRoles)
                    {
                        var resultUserRoles = new VmRoleItem();
                        var dbRole = await roleRepo.Get(dbItem.Role_Id);
                        
                        Copy<Com.BudgetMetal.DBEntities.Role, VmRoleItem>(dbRole, resultUserRoles);

                        SelectedRoles.Add(resultUserRoles);
                    }
                    result.SelectedRoles = SelectedRoles;
                }

                var resultCompany = new VmCompanyItem();
                Copy<Com.BudgetMetal.DBEntities.Company, VmCompanyItem>(dbresult.Company, resultCompany);
                result.Company = resultCompany;

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

        public async Task<VmGenericServiceResult> Register(VmUserItem user, string[] serviceTags)
        {
            var result = new VmGenericServiceResult();

            try
            {
                int companyId = 0;
                if (user.Company.Id > 0)
                {
                    companyId = user.Company.Id;
                }
                else
                {
                    var dbCompany = new Com.BudgetMetal.DBEntities.Company();
                    Copy<VmCompanyItem, Com.BudgetMetal.DBEntities.Company>(user.Company, dbCompany);

                    var dbMaxDefaultRFQ = await CTrepo.Get(Constants_CodeTable.Code_MaxDefaultRFQPerWeek);
                    dbCompany.MaxRFQPerWeek = Convert.ToInt32(dbMaxDefaultRFQ.Value);

                    var dbMaxDefaultQuote = await CTrepo.Get(Constants_CodeTable.Code_MaxDefaultQuotePerWeek);
                    dbCompany.MaxQuotationPerWeek = Convert.ToInt32(dbMaxDefaultQuote.Value);

                    dbCompany.IsVerified = false;
                    dbCompany.SupplierAvgRating = dbCompany.BuyerAvgRating = dbCompany.AwardedQuotation = dbCompany.SubmittedQuotation = 0;
                    dbCompany.C_BusinessType = Constants_CodeTable.Code_C_Supplier;
                    dbCompany.CreatedBy = dbCompany.UpdatedBy = user.UserName;
                    var dbResultCompany = cRepo.Add(dbCompany);
                    cRepo.Commit();
                    companyId = dbResultCompany.Id;

                    if (serviceTags != null)
                    {
                        foreach (var item in serviceTags)
                        {
                            var dbSupplierServicTags = new SupplierServiceTags();
                            dbSupplierServicTags.Company_Id = companyId;
                            dbSupplierServicTags.ServiceTags_Id = Convert.ToInt32(item);
                            dbSupplierServicTags.CreatedBy = dbSupplierServicTags.UpdatedBy = user.UserName;

                            supplierServiceTagsRepo.Add(dbSupplierServicTags);

                        }
                        supplierServiceTagsRepo.Commit();
                    }
                }

                var dbUser = new User();
                Copy<VmUserItem, Com.BudgetMetal.DBEntities.User>(user, dbUser, new string[] { "Company" });
                dbUser.Company_Id = companyId;
                dbUser.Password = Common.Md5.Encrypt(dbUser.Password);
                dbUser.CreatedBy = dbUser.UpdatedBy = user.UserName;
                dbUser.IsConfirmed = false;
                dbUser.UserType = Constants_CodeTable.Code_Supplier;

                var dbResultUser = repo.Add(dbUser);
                repo.Commit();

                result.IsSuccess = true;
                result.MessageToUser = user.EmailAddress;
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.MessageToUser = ex.Message;
                result.Error = ex;
            }
            
            return result;
        }

        public async Task<VmGenericServiceResult> ConfirmUserName(string Username)
        {
            var result = new VmGenericServiceResult();

            var dbresult = await repo.GetUserByUserName(Username);

            if (dbresult == null)
            {
                result.IsSuccess = false;
                result.MessageToUser = "This user is not registered.";
            }
            else
            {
                if (dbresult.IsConfirmed)
                {
                    result.IsSuccess = false;
                    result.MessageToUser = "This user is already confirmed.";
                }
                else
                {
                    dbresult.IsConfirmed = true;
                    repo.Update(dbresult);
                    repo.Commit();
                    result.IsSuccess = true;
                    result.MessageToUser = "Successful";
                }
                
            }

            return result;
        }

        public async Task<VmGenericServiceResult> ResetPassword(string username)
        {
            var result = new VmGenericServiceResult();
            
            var dbresult = await repo.GetUserByUserNameOrEmail(username);

            


            if (dbresult == null)
            {
                result.IsSuccess = false;
                result.MessageToUser = "This user is not registered.";
            }
            else
            {   
                //dbresult.Password = Md5.Encrypt(newPassword);
                //repo.Update(dbresult);
                //repo.Commit();
                result.IsSuccess = true;
                //generate reset password link
                string link = "";
                string encryptedUserName =System.Net.WebUtility.UrlEncode(Md5.Encrypt(dbresult.UserName));
                string encryptedValidDate = System.Net.WebUtility.UrlEncode(Md5.Encrypt(DateTime.Now.ToString("yyyy-mm-dd")));
                link = "u=" + encryptedUserName + "&d=" + encryptedValidDate;

                result.MessageToUser = dbresult.EmailAddress + ",#," + link;
                //result.MessageToUser = "The system sent the reset password link to email (" + dbresult.EmailAddress+").";
            }

            return result;
        }

        public async Task<VmGenericServiceResult> ResetPass(string username, string password)
        {
            var result = new VmGenericServiceResult();
            string newPassword = Md5.Encrypt(password);
            username = Md5.Decrypt(username);
            var dbresult = await repo.GetUserByUserName(username);
            
            if (dbresult == null)
            {
                result.IsSuccess = false;
                result.MessageToUser = "This user is not registered.";
            }
            else
            {
                dbresult.Password = newPassword;
                repo.Update(dbresult);
                repo.Commit();
                result.IsSuccess = true;
                result.MessageToUser = "Your password is successfully reset. Please sign in with new password. Thanks.";
            }

            return result;
        }

        public async Task<VmGenericServiceResult> ChangePassword(int id, string password)
        {
            var result = new VmGenericServiceResult();

            var dbresult = await repo.GetUserById(id);

            if (dbresult == null)
            {
                result.IsSuccess = false;
                result.MessageToUser = "This email is not registered.";
            }
            else
            {
                dbresult.Password = Md5.Encrypt(password);
                repo.Update(dbresult);
                repo.Commit();
                result.IsSuccess = true;
                result.MessageToUser = "Successful";
            }

            return result;
        }

    }
}
