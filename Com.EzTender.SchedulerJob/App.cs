using Com.BudgetMetal.DataRepository.Company;
using Com.BudgetMetal.Common;
using System;
using Com.BudgetMetal.DataRepository.RFQ;
using System.Threading.Tasks;
using System.Linq;
using Com.BudgetMetal.DataRepository.Users;

namespace Com.EzTender.SchedulerJob
{
    public class App
    {
        private readonly ICompanyRepository repoCompany;
        private readonly IRfqRepository repoRFQ;
        private readonly IUserRepository repoUser;

        public App(ICompanyRepository _repoCompany, IRfqRepository _repoRFQ, IUserRepository _repoUser)
        {
            repoCompany = _repoCompany;
            repoRFQ = _repoRFQ;
            repoUser = _repoUser;
        }

        public void Run()
        {
            AlertOpenRFQForExpiring();
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