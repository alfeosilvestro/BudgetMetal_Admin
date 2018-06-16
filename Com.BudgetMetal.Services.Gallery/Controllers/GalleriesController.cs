using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Com.BudgetMetal.Services.Gallery.DB;
using Com.BudgetMetal.Services.Gallery.Models;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;

namespace Com.BudgetMetal.Services.Gallery.Controllers
{
    public class GalleriesController : Controller
    {
        private readonly AppDbContext _context;
        public GalleriesController(AppDbContext context)
        {
            _context = context;
        }
        // GET api/values
        [HttpGet]
        public async Task<JsonResult> Get(string keyword = "", int page = 1)
        {
            var result = await GetGalleriesByPage(keyword, page);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        private async Task<object> GetGalleriesByPage(string keyword, int page, int totalRecords = 10)
        {
            //return _context.bm_gallery.ToListAsync();

            if (string.IsNullOrEmpty(keyword.Trim()))
            {
                //return await base.GetPage(keyword, page, totalRecords);
            }

            var records = _context.bm_gallery.Where(e =>
                e.Name.Contains(keyword) ||
                e.Description.Contains(keyword)
            );

            var recordList = records.OrderBy(e => e.Name)
            .OrderBy(e => e.CreatedDate)
            .Skip((totalRecords * page) - totalRecords)
            .Take(totalRecords)
            .ToList();

            var count = records.Count();

            var result = new PageResult()
            {
                Records = recordList,
                TotalPage = (count + totalRecords - 1) / totalRecords,
                CurrentPage = page,
                TotalRecords = count
            };

            var resultObject = new ResponseBase();
            resultObject.ResultObject = result;
            
            return resultObject;
        }

        public async Task<FileResult> Download(int fileid)
        {
            var bm_gallery = await _context.bm_gallery
                .SingleOrDefaultAsync(m => m.Id == fileid);
            if (bm_gallery == null)
            {
                return null;
            }

            var fileByeArray = bm_gallery.DownloadableImage;
            string fileName = (bm_gallery.Name).Replace(" ", "_").Trim() + ".zip";
            var readStream = new MemoryStream(Convert.FromBase64String(fileByeArray));
            var mimeType = "application/zip";
            return File(readStream, mimeType, fileName);
        }
    }
}