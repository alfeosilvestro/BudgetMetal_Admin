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

namespace Com.BudgetMetal.Services.GalleryAPI.Controllers
{
    public class GalleriesController : Controller
    {
        private readonly IGalleryService svs;

        public GalleriesController(IGalleryService svs)
        {
            this.svs = svs;
        }
        
        // GET api/values
        [HttpGet]
        public async Task<JsonResult> Get(string keyword, int page, int totalRecords, bool getDetailImage = false)
        {
            var result = svs.GetGalleriesByPage(keyword, page, totalRecords, getDetailImage);
                
            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
        
        //public async Task<FileResult> Download(int fileid)
        //{
        //    var bm_gallery = await _context.bm_gallery
        //        .SingleOrDefaultAsync(m => m.Id == fileid);
        //    if (bm_gallery == null)
        //    {
        //        return null;
        //    }

        //    var fileByeArray = bm_gallery.DownloadableImage;
        //    string fileName = (bm_gallery.Name).Replace(" ", "_").Trim() + ".zip";
        //    var readStream = new MemoryStream(Convert.FromBase64String(fileByeArray));
        //    var mimeType = "application/zip";
        //    return File(readStream, mimeType, fileName);
        //}
    }
}