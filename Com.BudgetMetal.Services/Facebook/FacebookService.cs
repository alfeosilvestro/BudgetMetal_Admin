using Com.BudgetMetal.ViewModels.Facebook;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Facebook;
using Com.BudgetMetal.DataRepository.Code_Table;
using Com.BudgetMetal.Common;
using Com.BudgetMetal.ViewModels;
using System.Dynamic;

namespace Com.BudgetMetal.Services.Facebook
{
    public class FacebookService : IFacebookService
    {
        private readonly ICodeTableRepository repoCodeTable;
        private string _appId { get; set; }
        private string _secret { get; set; }


        public FacebookService(ICodeTableRepository repoCodeTable)
        {
            this.repoCodeTable = repoCodeTable;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var appIdTask = repoCodeTable.Get(Constants_CodeTable.Code_SiteOption_Fb_App_Id);
            var secretTask = repoCodeTable.Get(Constants_CodeTable.Code_SiteOption_Fb_App_Secret);
            var accessTokenTask = repoCodeTable.Get(Constants_CodeTable.Code_SiteOption_Fb_AccessToken);

            Task.WaitAll(new[] { appIdTask, secretTask, accessTokenTask });

            string app_id = appIdTask.Result.Value;
            string app_secret = secretTask.Result.Value;
            string accessToken = accessTokenTask.Result.Value;
           
            this._appId = app_id;
            this._secret = app_secret;
            return accessToken; // no need to create 
            //var fb = new FacebookClient();
            //dynamic result = fb.Get("oauth/access_token", new
            //{
            //    client_id = app_id,
            //    client_secret = app_secret,
            //    grant_type = "fb_exchange_token",
            //    fb_exchange_token = accessToken
            //});
            //fb.AccessToken = result.access_token;

            //return fb.AccessToken;
        }

        
        public async Task<VmGenericServiceResult> PostMessage(string message)
        {
            var result = new VmGenericServiceResult();
            try
            {
                //var appIdTask = repoCodeTable.Get(Constants_CodeTable.Code_SiteOption_Fb_App_Id);
                //var secretTask = repoCodeTable.Get(Constants_CodeTable.Code_SiteOption_Fb_App_Secret);
                var PageIdTask = repoCodeTable.Get(Constants_CodeTable.Code_SiteOption_Fb_PageId);

                //Task.WaitAll(new[] { appIdTask, secretTask, PageIdTask });

                Task.WaitAll(new[] { PageIdTask });

                //string app_id = appIdTask.Result.Value;
                //string app_secret = secretTask.Result.Value;
               

                string accessToken = await GetAccessTokenAsync();
                string page_id = PageIdTask.Result.Value;
                string app_id = _appId;
                string app_secret = _secret;
                var client = new FacebookClient(accessToken);
                client.AppId = app_id;
                client.AppSecret = app_secret;

                dynamic parameters = new ExpandoObject();
                parameters.message = message;

                client.Post("/"+ page_id + "/feed", parameters);
                result.IsSuccess = true;
                result.MessageToUser = "Successful";
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.MessageToUser = "Error while posting on facebook. " + ex.Message;
                result.Error = ex;
            }

            return result;
        }


    }
}
