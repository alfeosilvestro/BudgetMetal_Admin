using Com.BudgetMetal.DataRepository.Company;
using Com.BudgetMetal.Common;
using System;
using Com.BudgetMetal.DataRepository.Forex;
using Com.BudgetMetal.DataRepository.RFQ;
using System.Threading.Tasks;
using System.Linq;
using Com.BudgetMetal.DataRepository.Users;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using Com.BudgetMetal.Services.Facebook;

namespace Com.EzTender.SchedulerJob
{
    public class App
    {
        private readonly ICompanyRepository repoCompany;
        private readonly IRfqRepository repoRFQ;
        private readonly IUserRepository repoUser;
        private readonly IForexRepository repoForex;
        private readonly IFacebookService svsFacebook;

        public App(ICompanyRepository _repoCompany, IRfqRepository _repoRFQ, IUserRepository _repoUser, IForexRepository _repoForex, IFacebookService _svsFacebook)
        {
            repoCompany = _repoCompany;
            repoRFQ = _repoRFQ;
            repoUser = _repoUser;
            repoForex = _repoForex;
            svsFacebook = _svsFacebook;
        }

        public void Run()
        {
            //AlertOpenRFQForExpiring();
            PostCurrencyRateToFacebook();
        }

        private async Task PostCurrencyRateToFacebook()
        {
            Console.WriteLine("Post Exchange Rate  - Start");
            string result =await repoForex.GetForexDataFromBankApi();
            JObject json = JObject.Parse(result);
            //string[] arrExchanges = { "USD", "GBP", "SGD" };

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Exchange Rates On " + DateTime.Now.ToString("dd.MMM.yyyy") + Environment.NewLine);
            foreach (var exch in Constants.arrExchanges)
            {
                sb.AppendFormat("1 {0} = {1} MMK{2}", exch,json["rates"][exch],Environment.NewLine);
            }

            var resultPost =  await svsFacebook.PostMessage(sb.ToString());
            if (resultPost.IsSuccess)
            {
                Console.WriteLine("Post Exchange Rate  - Success");
            }
            else
            {
                Console.WriteLine("Post Exchange Rate  - Failed:" + resultPost.MessageToUser);
            }
            Console.WriteLine("Post Exchange Rate  - End");
        }

        private async void AlertOpenRFQForExpiring()
        {
            var rfqList = await repoRFQ.GetAllOpenRFQ();

            if (rfqList != null)
            {
                foreach (var item in rfqList)
                {
                    if (Convert.ToDateTime(item.ValidRfqdate).Date < DateTime.Now.Date)
                    {
                        //send email
                        var documentUser = item.Document.DocumentUser.Where(e => e.IsActive == true).OrderBy(e => e.Id).First();
                        var ownerId = documentUser.User_Id;
                        var user = await repoUser.Get(ownerId);

                        SendingMail SM = new SendingMail();
                        SM.SendMail(user.EmailAddress, "", "Alert Test", "Testing");
                    }
                }
            }
        }
    }
}