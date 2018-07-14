using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Com.BudgetMetal.Common;
using Com.BudgetMetal.Services.GalleryAPI;
using Com.BudgetMetal.Services.GalleryAPI.Configurations;
using Microsoft.Extensions.Options;
using Com.BudgetMetal.Services.Gallery;

namespace Com.BudgetMetal.Services.GalleryAPI.Controllers
{
    public class GalleriesController : Controller
    {
        private readonly IGalleryService svs;
        private readonly AppSettings _appSettings;


        public GalleriesController(IGalleryService svs, IOptions<AppSettings> appSettings)
        {
            this.svs = svs;
            this._appSettings = appSettings.Value;
        }

        // GET api/values
        [HttpGet]
        public async Task<JsonResult> Get(string keyword, int page, int totalRecords)
        {
            var result = svs.GetGalleriesByPage(keyword, page, _appSettings.TotalRecordPerPage, _appSettings.App_Identity.Identity);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> GetItem(int Id)
        {
            var result = svs.GetGalleryById(Id, _appSettings.App_Identity.Identity);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        public async Task<bool> CheckAuthentication(string token)
        {
            var result = svs.CheckAuthentication(token);

            return result;
        }

        public async Task<FileResult> Download(int fileid, string token)
        {

            var result = svs.GetGalleryFileById(fileid, _appSettings.App_Identity.Identity, token);
            if (result == null)
            {
                return null;
            }

            var fileByeArray = result.Result.DownloadableImage;
            string fileName = (result.Result.Name).Replace(" ", "_").Trim() + ".zip";
            var readStream = new MemoryStream(Convert.FromBase64String(fileByeArray));
            var mimeType = "application/zip";
            return File(readStream, mimeType, fileName);

        }
    }
}