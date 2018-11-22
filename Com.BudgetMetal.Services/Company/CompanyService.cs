using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Code_Table;
using Com.BudgetMetal.DataRepository.Company;
using Com.BudgetMetal.DataRepository.CompanySupplier;
using Com.BudgetMetal.DataRepository.Rating;
using Com.BudgetMetal.DataRepository.Roles;
using Com.BudgetMetal.DataRepository.Users;
using Com.BudgetMetal.DataRepository.UserRoles;
using Com.BudgetMetal.Services.Base;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.Company;
using Com.BudgetMetal.ViewModels.Rating;
using Com.BudgetMetal.ViewModels.Role;
using Com.BudgetMetal.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.BudgetMetal.DataRepository.SupplierServiceTags;
using Com.BudgetMetal.ViewModels.SupplierServiceTag;

namespace Com.BudgetMetal.Services.Company
{
    public class CompanyService : BaseService, ICompanyService
    {
        private readonly ICompanyRepository repo;
        private readonly IUserRepository repoUser;
        private readonly ICodeTableRepository codeTableRepo;
        private readonly IRatingRepository ratingRepo;
        private readonly ICompanySupplierRepository companySupplierRepo;
        private readonly IRoleRepository roleRepository;
        private readonly IUserRolesRepository userRoleRepository;
        private readonly ISupplierServiceTagsRepository supplierServiceTagRepo;

        public CompanyService(ICompanyRepository repo, ICodeTableRepository companyRepo, IUserRepository repoUser, 
            IRatingRepository ratingRepo, ICompanySupplierRepository companySupplierRepo, IRoleRepository roleRepository, 
            IUserRolesRepository userRoleRepository,
            ISupplierServiceTagsRepository supplierSvsTagRepo)
        {
            this.repo = repo;
            this.codeTableRepo = companyRepo;
            this.repoUser = repoUser;
            this.ratingRepo = ratingRepo;
            this.companySupplierRepo = companySupplierRepo;
            this.roleRepository = roleRepository;
            this.userRoleRepository = userRoleRepository;
            this.supplierServiceTagRepo = supplierSvsTagRepo;
        }

        public async Task<VmCompanyPage> GetCompanyByPage(string keyword, int page, int totalRecords)
        {
            var dbPageResult = await repo.GetPage(keyword,
                (page == 0 ? Constants.app_firstPage : page),
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords));

            if (dbPageResult == null)
            {
                return new VmCompanyPage();
            }

            var resultObj = new VmCompanyPage();
            resultObj.RequestId = DateTime.Now.ToString("yyyyMMddHHmmss");
            resultObj.RequestDate = DateTime.Now;
            resultObj.Result = new PageResult<VmCompanyItem>();
            resultObj.Result.Records = new List<VmCompanyItem>();

            Copy<PageResult<Com.BudgetMetal.DBEntities.Company>, PageResult<VmCompanyItem>>(dbPageResult, resultObj.Result, new string[] { "Records" });

            foreach (var dbItem in dbPageResult.Records)
            {
                var resultItem = new VmCompanyItem();

                Copy<Com.BudgetMetal.DBEntities.Company, VmCompanyItem>(dbItem, resultItem);
                resultItem.IsVerified = (dbItem.IsVerified == null) ? false : (bool)dbItem.IsVerified;
                resultObj.Result.Records.Add(resultItem);
            }

            return resultObj;
        }

        public async Task<VmCompanyPage> GetCompanySupplierList(string keyword, int page, int totalRecords)
        {
            var dbPageResult = await repo.GetSupplierList(keyword,
                (page == 0 ? Constants.app_firstPage : page),
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords));

            if (dbPageResult == null)
            {
                return new VmCompanyPage();
            }

            var resultObj = new VmCompanyPage();
            resultObj.RequestId = DateTime.Now.ToString("yyyyMMddHHmmss");
            resultObj.RequestDate = DateTime.Now;
            resultObj.Result = new PageResult<VmCompanyItem>();
            resultObj.Result.Records = new List<VmCompanyItem>();

            Copy<PageResult<Com.BudgetMetal.DBEntities.Company>, PageResult<VmCompanyItem>>(dbPageResult, resultObj.Result, new string[] { "Records" });

            foreach (var dbItem in dbPageResult.Records)
            {
                var resultItem = new VmCompanyItem();

                Copy<Com.BudgetMetal.DBEntities.Company, VmCompanyItem>(dbItem, resultItem);
                resultItem.IsVerified = (dbItem.IsVerified == null) ? false : (bool)dbItem.IsVerified;
                resultObj.Result.Records.Add(resultItem);
            }

            return resultObj;
        }

        public async Task<VmCompanyItem> GetCompanyById(int Id)
        {
            var dbPageResult = await repo.Get(Id);

            if (dbPageResult == null)
            {
                return new VmCompanyItem();
            }

            var resultObj = new VmCompanyItem();

            Copy<Com.BudgetMetal.DBEntities.Company, VmCompanyItem>(dbPageResult, resultObj);

            resultObj.IsVerified = (dbPageResult.IsVerified == null) ? false : (bool)dbPageResult.IsVerified;

            var dbUserList = await repoUser.GetUserByCompany(Id);

            if (dbUserList == null) return resultObj;

            resultObj.UserList = new List<VmUserItem>();

            foreach (var dbUser in dbUserList)
            {
                List<VmRoleItem> rListItem = new List<VmRoleItem>();
                if (dbUser.UserRoles != null)
                {
                    foreach (var dbRoleItem in dbUser.UserRoles)
                    {
                        VmRoleItem rItem = new VmRoleItem();
                        rItem.Id = dbRoleItem.Role.Id;
                        rItem.Name = dbRoleItem.Role.Name;
                        rItem.Code = dbRoleItem.Role.Code;
                       
                        rListItem.Add(rItem);
                    }
                }
                VmUserItem user = new VmUserItem()
                {
                    Id = dbUser.Id,
                    UserName = dbUser.UserName,
                    EmailAddress = dbUser.EmailAddress,
                    JobTitle = dbUser.JobTitle,
                    UserType = dbUser.UserType,
                    RoleList = rListItem
                };

                resultObj.UserList.Add(user);
            }

            var dbRepoList = await ratingRepo.GetRagintByCompany(Id);
            if (dbRepoList == null) return resultObj;

            resultObj.RatingList = new List<VmRatingItem>();
            foreach (var dbRating in dbRepoList)
            {
                VmRatingItem rating = new VmRatingItem()
                {
                    SpeedOfQuotation = dbRating.SpeedOfQuotation,
                    SpeedofDelivery = dbRating.SpeedofDelivery,
                    ServiceQuality = dbRating.ServiceQuality,
                    Price = dbRating.Price,
                    SpeedofProcessing = dbRating.SpeedofProcessing,
                    Payment = dbRating.Payment,
                    Title = dbRating.Title,
                    Description = dbRating.Description,
                    Ratingcol = dbRating.Ratingcol,
                    UserName = dbRating.User.ContactName
                };
                //var rating = new VmRatingItem();
                //Copy<Com.BudgetMetal.DBEntities.Rating, VmRatingItem>(dbRating, rating);
                resultObj.RatingList.Add(rating);
            }

            return resultObj;
        }

        public VmGenericServiceResult Insert(VmCompanyItem vmItem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                Com.BudgetMetal.DBEntities.Company r = new Com.BudgetMetal.DBEntities.Company();

                Copy<VmCompanyItem, Com.BudgetMetal.DBEntities.Company>(vmItem, r);

                r.IsVerified = false;
                r.AwardedQuotation = 0;
                r.SubmittedQuotation = 0;
                r.BuyerAvgRating = 0;
                r.SupplierAvgRating = 0;

                //Max Default RFQ Per Week
                var codeTableRepo = this.codeTableRepo.Get(10100001);

                int maxQuotationPerWeek = Convert.ToInt32(codeTableRepo.Result.Value);

                //Max Default Quote Per Week
                codeTableRepo = this.codeTableRepo.Get(10100002);

                int maxRFQPerWeek = Convert.ToInt32(codeTableRepo.Result.Value);

                r.MaxQuotationPerWeek = maxQuotationPerWeek;
                r.MaxRFQPerWeek = maxRFQPerWeek;

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

        public async Task<VmGenericServiceResult> Update(VmCompanyItem vmCompanyItem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                Com.BudgetMetal.DBEntities.Company r = await repo.Get(vmCompanyItem.Id);

                Copy<VmCompanyItem, Com.BudgetMetal.DBEntities.Company>(vmCompanyItem, r);

                if (r.UpdatedBy.IsNullOrEmpty())
                {
                    r.UpdatedBy = "System";
                }

                r.IsVerified = vmCompanyItem.IsVerified;

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
            Com.BudgetMetal.DBEntities.Company r = await repo.Get(Id);
            r.IsActive = false;
            repo.Update(r);
            repo.Commit();
        }

        public async Task<VmCompanyPage> GetSupplierByServiceTagsId( int companyId, string serviceTagsId,int page, string searchKeyword)
        {
            var dbPageResult = repo.GetSupplierByServiceTagsId(serviceTagsId,
                (page == 0 ? Constants.app_firstPage : page),
                Constants.app_totalRecords, searchKeyword);

            var PreferredSupplierList = await companySupplierRepo.GetPreferredSupplierByCompanyId(companyId);
            

            if (dbPageResult == null)
            {
                return new VmCompanyPage();
            }

            var resultObj = new VmCompanyPage();
            resultObj.RequestId = DateTime.Now.ToString("yyyyMMddHHmmss");
            resultObj.RequestDate = DateTime.Now;
            resultObj.Result = new PageResult<VmCompanyItem>();
             var companyList = new List<VmCompanyItem>();

            Copy<PageResult<Com.BudgetMetal.DBEntities.Company>, PageResult<VmCompanyItem>>(dbPageResult, resultObj.Result, new string[] { "Records" });

            foreach (var dbItem in dbPageResult.Records)
            {
                var resultItem = new VmCompanyItem();

                Copy<Com.BudgetMetal.DBEntities.Company, VmCompanyItem>(dbItem, resultItem);
                if (PreferredSupplierList.Contains(resultItem.Id))
                {
                    resultItem.OrderToShow = 1;
                }
               companyList.Add(resultItem);
            }

            resultObj.Result.Records =companyList.OrderByDescending(e=>e.OrderToShow).ToList();
            return resultObj;
        }

        public async Task<List<VmCompanyItem>> GetActiveCompanies()
        {
            var dbResult = await repo.GetAll();

            var resultList = new List<VmCompanyItem>();
            foreach (var dbItem in dbResult)
            {
                var resultItem = new VmCompanyItem();
                Copy<Com.BudgetMetal.DBEntities.Company, VmCompanyItem>(dbItem, resultItem);
                resultList.Add(resultItem);
            }
            return resultList;

        }

        public async Task<VmCompanyItem> GetCompanyByUEN(string RegNo)
        {
            var dbPageResult = await repo.GetCompanyByUEN(RegNo);

            if (dbPageResult == null)
            {
                return new VmCompanyItem();
            }

            var resultObj = new VmCompanyItem();

            Copy<Com.BudgetMetal.DBEntities.Company, VmCompanyItem>(dbPageResult, resultObj);

           
            return resultObj;
        }

        public async Task<List<VmSupplierServiceTag>> GetCompanyServiceTagById(int companyId)
        {
            var dbResult = await supplierServiceTagRepo.GetServiceTagByCompanyID(companyId);

            if (dbResult == null)
            {
                return null;
            }

            List<VmSupplierServiceTag> result = new List<VmSupplierServiceTag>();

            foreach (var tag in dbResult)
            {
                result.Add(new VmSupplierServiceTag() {
                    Id = tag.Id,
                    ServiceTagId = tag.ServiceTags_Id,
                    Name = ""
                });
            }

            return result;

        }

        public async Task<VmCompanyPage> GetSupplierByCompany(int companyId, int page, string keyword)
        {
            var dbPageResult = await repo.GetSupplierByCompanyId(companyId,
                (page == 0 ? Constants.app_firstPage : page),
                Constants.app_totalRecords, keyword);

            if (dbPageResult == null)
            {
                return new VmCompanyPage();
            }

            var resultObj = new VmCompanyPage();
            resultObj.RequestId = DateTime.Now.ToString("yyyyMMddHHmmss");
            resultObj.RequestDate = DateTime.Now;
            resultObj.Result = new PageResult<VmCompanyItem>();
            resultObj.Result.Records = new List<VmCompanyItem>();

            Copy<PageResult<Com.BudgetMetal.DBEntities.Company>, PageResult<VmCompanyItem>>(dbPageResult, resultObj.Result, new string[] { "Records" });

            foreach (var dbItem in dbPageResult.Records)
            {
                var resultItem = new VmCompanyItem();

                Copy<Com.BudgetMetal.DBEntities.Company, VmCompanyItem>(dbItem, resultItem);

                resultObj.Result.Records.Add(resultItem);
            }

            return resultObj;
        }
        
        public async Task<VmGenericServiceResult> EditCompanyAbout(int companyId, string about, string updatedBy)
        {            
            var result = new VmGenericServiceResult();

            var dbresult = await repo.Get(companyId);

            if (dbresult == null)
            {
                result.IsSuccess = false;
                result.MessageToUser = "This email is not registered.";
            }
            else
            {   
                dbresult.About = about;
                dbresult.UpdatedBy = updatedBy;
                repo.Update(dbresult);
                repo.Commit();
                result.IsSuccess = true;
                result.MessageToUser = "Successful";
            }
            return result;
        }

        public async Task<VmGenericServiceResult> EditCompanyAddress(int companyId, string address, string updatedBy)
        {
            var result = new VmGenericServiceResult();

            var dbresult = await repo.Get(companyId);

            if (dbresult == null)
            {
                result.IsSuccess = false;
                result.MessageToUser = "This email is not registered.";
            }
            else
            {
                dbresult.Address = address;
                dbresult.UpdatedBy = updatedBy;
                repo.Update(dbresult);
                repo.Commit();
                result.IsSuccess = true;
                result.MessageToUser = "Successful";
            }
            return result;
        }

        public async Task<VmGenericServiceResult> EditCompanyUser(int companyId, int userId, bool isActiveStatus, string updatedBy)
        {
            var result = new VmGenericServiceResult();

            var dbresult = await repoUser.GetUserCompanyIdandUserId(companyId, userId);

            if (dbresult == null)
            {
                result.IsSuccess = false;
                result.MessageToUser = "This email is not registered.";
            }
            else
            {
                dbresult.IsConfirmed = isActiveStatus;
                dbresult.UpdatedBy = updatedBy;
                repoUser.Update(dbresult);
                repoUser.Commit();
                result.IsSuccess = true;
                result.MessageToUser = "Successful";
            }
            return result;
        }

        public async Task<VmCompanyItem> GetCompanyProfileById(int Id)
        {
            var dbPageResult = await repo.Get(Id);

            if (dbPageResult == null)
            {
                return new VmCompanyItem();
            }

            var resultObj = new VmCompanyItem();

            Copy<Com.BudgetMetal.DBEntities.Company, VmCompanyItem>(dbPageResult, resultObj);

            resultObj.IsVerified = (dbPageResult.IsVerified == null) ? false : (bool)dbPageResult.IsVerified;

            var dbUserList = await repoUser.GetUserByCompanyNotFilterWithConfirm(Id);

            if (dbUserList == null) return resultObj;

            resultObj.UserList = new List<VmUserItem>();

            foreach (var dbUser in dbUserList)
            {
                List<VmRoleItem> rListItem = new List<VmRoleItem>();
                if (dbUser.UserRoles != null)
                {
                    foreach (var dbRoleItem in dbUser.UserRoles)
                    {
                        var dbRole = roleRepository.Get(dbRoleItem.Role_Id);
                        if (dbRole != null)
                        {
                            VmRoleItem rItem = new VmRoleItem();
                            rItem.Id = dbRole.Result.Id;
                            rItem.Name = dbRole.Result.Name;
                            rItem.Code = dbRole.Result.Code;
                            rListItem.Add(rItem);
                        }
                    }
                }
                VmUserItem user = new VmUserItem()
                {
                    Id = dbUser.Id,
                    UserName = dbUser.UserName,
                    EmailAddress = dbUser.EmailAddress,
                    JobTitle = dbUser.JobTitle,
                    UserType = dbUser.UserType,
                    IsConfirmed = dbUser.IsConfirmed,
                    IsActive = dbUser.IsActive,
                    RoleList = rListItem
                };

                resultObj.UserList.Add(user);
            }

            var dbRepoList = await ratingRepo.GetRagintByCompany(Id);
            if (dbRepoList == null) return resultObj;

            resultObj.RatingList = new List<VmRatingItem>();
            foreach (var dbRating in dbRepoList)
            {
                VmRatingItem rating = new VmRatingItem()
                {
                    SpeedOfQuotation = dbRating.SpeedOfQuotation,
                    SpeedofDelivery = dbRating.SpeedofDelivery,
                    ServiceQuality = dbRating.ServiceQuality,
                    Price = dbRating.Price,
                    SpeedofProcessing = dbRating.SpeedofProcessing,
                    Payment = dbRating.Payment,
                    Title = dbRating.Title,
                    Description = dbRating.Description,
                    Ratingcol = dbRating.Ratingcol,
                    UserName = dbRating.User.ContactName
                };
                //var rating = new VmRatingItem();
                //Copy<Com.BudgetMetal.DBEntities.Rating, VmRatingItem>(dbRating, rating);
                resultObj.RatingList.Add(rating);
            }

            return resultObj;
        }

        public async Task<VmGenericServiceResult>EditCompanyUserRole(int companyId, int userId, string[] userRole, string updatedBy)
        {
            var result = new VmGenericServiceResult();
            string roleName = "";
            var dbRoleUser = userRoleRepository.GetUserRolesByUserId(userId);
            if (dbRoleUser != null)
            {
                foreach (var item in dbRoleUser.Result)
                {
                    DBEntities.UserRoles userRoles = await userRoleRepository.Get(item.Id);
                    userRoleRepository.Delete(userRoles);
                }
                userRoleRepository.Commit();
            }

            foreach (var role in userRole)
            {
                if (role != null)
                {
                    int roleId = int.Parse(role);
                    var dbRoleUserResult = await userRoleRepository.GetUserRolesByUserIdRoleId(userId, roleId);
                    if (dbRoleUserResult == null)
                    {
                        DBEntities.UserRoles userRoles = new DBEntities.UserRoles();
                        userRoles.User_Id = userId;
                        userRoles.IsActive = true;
                        userRoles.UpdatedBy = updatedBy;
                        userRoles.Role_Id = roleId;
                        userRoleRepository.Add(userRoles);
                    }
                    else
                    {
                        dbRoleUserResult.IsActive = true;
                        dbRoleUserResult.UpdatedBy = updatedBy;
                        dbRoleUserResult.Role_Id = roleId;
                        userRoleRepository.Update(dbRoleUserResult);
                    }
                    var roleEntity = roleRepository.Get(roleId);
                    if (roleEntity != null)
                    {
                        roleName += roleEntity.Result.Name + "|";
                    }
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = true;
                }
            }            
            result.MessageToUser = roleName.TrimEnd('|');
            userRoleRepository.Commit();
            return result;
        }
    }
}
