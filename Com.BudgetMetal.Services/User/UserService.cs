using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Users;
using Com.BudgetMetal.DataRepository.Single_Sign_On;
using Com.BudgetMetal.DB.Entities;
using Com.BudgetMetal.Services.Base;
using Com.BudgetMetal.ViewModels.User;
using System.Collections.Generic;
using System;

namespace Com.BudgetMetal.Services.Users
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository repo;


        public UserService(IUserRepository repo)
        {
            this.repo = repo;
        }

        public VmUserPage GetUserByPage(string keyword, int page, int totalRecords)
        {
            var dbPageResult = repo.GetUsersByPage(keyword,
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

                resultObj.Result.Records.Add(resultItem);
            }

            return resultObj;
        }

        public VmUserItem GetUserById(int Id)
        {
            var dbPageResult = repo.GetUserById(Id);

            if (dbPageResult == null)
            {
                return new VmUserItem();
            }

            var resultObj = new VmUserItem();
            resultObj.Id = dbPageResult.Id;
            //resultObj = dbPageResult.UserName;
            //resultObj.Password = dbPageResult.Password;
            //resultObj.Title = dbPageResult.Title;
            //resultObj.SiteAdmin = dbPageResult.SiteAdmin;
            //resultObj.Status = dbPageResult.Status;
            //resultObj.Confirmed = dbPageResult.Confirmed;
            resultObj.IsActive = dbPageResult.IsActive;
            resultObj.Version = dbPageResult.Version;
            resultObj.CreatedBy = dbPageResult.CreatedBy;
            resultObj.CreatedDate = dbPageResult.CreatedDate;
            return resultObj;
        }

        public void Insert(VmUserItem vmRoleItem)
        {
            //user r = new user();
            //r.IsActive = true;
            //r.UserName = vmRoleItem.UserName;
            //r.Password = vmRoleItem.Password;
            //r.Email = vmRoleItem.Email;
            //r.Title = vmRoleItem.Title;
            //r.Status = vmRoleItem.Status;
            //r.SiteAdmin = vmRoleItem.SiteAdmin;
            //r.Confirmed = vmRoleItem.Confirmed;
            //r.RoleId = vmRoleItem.RoleId;
            //r.Version = "001";
            //r.CreatedBy = "System";
            //r.CreatedDate = DateTime.Now;
            //r.UpdatedBy = "System";
            //r.UpdatedDate = DateTime.Now;
            //repo.Add(r);
        }

        public void Update(VmUserItem vmRoleItem)
        {
            //user r = new user();
            //r.Id = vmRoleItem.Id;
            //r.IsActive = vmRoleItem.IsActive;
            //r.Version = vmRoleItem.Version;
            //r.UpdatedBy = vmRoleItem.UpdatedBy;
            //r.UpdatedDate = DateTime.Now;
            //r.UserName = vmRoleItem.UserName;
            //r.Email = vmRoleItem.Email;
            //r.Password = vmRoleItem.Password;
            //r.Title = vmRoleItem.Title;
            //r.Confirmed = vmRoleItem.Confirmed;
            //r.SiteAdmin = vmRoleItem.SiteAdmin;
            //r.Status = vmRoleItem.Status;
            //r.RoleId = vmRoleItem.RoleId;
            //repo.Update(r);
        }

        public void Delete(int Id)
        {
            //user r= repo.GetUserById(Id);
            //r.IsActive = false;
            //repo.Update(r);
        }
    }
}
