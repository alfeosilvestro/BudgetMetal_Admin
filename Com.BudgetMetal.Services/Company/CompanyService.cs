using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Code_Table;
using Com.BudgetMetal.DataRepository.Company;
using Com.BudgetMetal.DataRepository.Rating;
using Com.BudgetMetal.DataRepository.Users;
using Com.BudgetMetal.Services.Base;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.Company;
using Com.BudgetMetal.ViewModels.Rating;
using Com.BudgetMetal.ViewModels.Role;
using Com.BudgetMetal.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.Company
{
    public class CompanyService : BaseService, ICompanyService
    {
        private readonly ICompanyRepository repo;
        private readonly IUserRepository repoUser;
        private readonly ICodeTableRepository companyRepo;
        private readonly IRatingRepository ratingRepo;

        public CompanyService(ICompanyRepository repo, ICodeTableRepository companyRepo, IUserRepository repoUser, IRatingRepository ratingRepo)
        {
            this.repo = repo;
            this.companyRepo = companyRepo;
            this.repoUser = repoUser;
            this.ratingRepo = ratingRepo;
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
                        VmRoleItem rItem = new VmRoleItem()
                        {
                            Name = dbRoleItem.Role.Name,
                            Code = dbRoleItem.Role.Code
                        };
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
                var codeTableRepo = companyRepo.Get(10100001);

                int maxQuotationPerWeek = Convert.ToInt32(codeTableRepo.Result.Value);

                //Max Default Quote Per Week
                codeTableRepo = companyRepo.Get(10100002);

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

        public async Task<VmCompanyPage> GetSupplierByServiceTagsId(string serviceTagsId, int page, string searchKeyword)
        {
            var dbPageResult = repo.GetSupplierByServiceTagsId(serviceTagsId,
                (page == 0 ? Constants.app_firstPage : page),
                Constants.app_totalRecords, searchKeyword);

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
    }
}
