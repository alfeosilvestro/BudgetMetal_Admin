using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.DataRepository.RfqInvites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Com.EzTender.PublicPortal.Controllers
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

        // GET: RfqAccessByEmail/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RfqAccessByEmail/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RfqAccessByEmail/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RfqAccessByEmail/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RfqAccessByEmail/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RfqAccessByEmail/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RfqAccessByEmail/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}