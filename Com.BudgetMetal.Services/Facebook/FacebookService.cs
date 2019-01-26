using Com.BudgetMetal.ViewModels.Facebook;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.Facebook
{
    public class FacebookService : IFacebookService
    {
        private string _accessToken;
        private string _pageId;

        public FacebookService(string accessToken, string pageId)
        {
            this._accessToken = accessToken;
            this._pageId = pageId;
        }

        public string Publish(string textToPublish, string imageUrl)
        {
            IFacebookClient ifc = new FacebookClient(this._accessToken, this._pageId);

            var facebookTask = ifc.PublishToFacebook(textToPublish, imageUrl);

            return facebookTask;
        }

        public async Task SimplePost(string textToPublish)
        {
            IFacebookClient ifc = new FacebookClient(this._accessToken, this._pageId);

            var facebookTask = ifc.PublishSimplePost(textToPublish);
            Task.WaitAll(facebookTask);

            Console.WriteLine( facebookTask.Result );
        }

    }
}
