using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.DataRepository.RfqInvites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Com.EzTender.WebApp.Controllers
{
    public class RfqAccessByEmailController : Controller
    {
        private readonly IRfqInvitesRepository repoRfqInvites;

        public RfqAccessByEmailController(IRfqInvitesRepository repoRfqInvites)
        {
            this.repoRfqInvites = repoRfqInvites;
        }

        // GET: RfqAccessByEmail
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> CheckRfqEmailandAccessCode(string Email, string AccessCode)
        {
            var result = await repoRfqInvites.GetRfqInvitesWithEmailandAccessCode(Email, AccessCode);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        
    }
}