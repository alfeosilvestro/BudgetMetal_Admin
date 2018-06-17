using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BudgetMetal_Admin.DB;
using BudgetMetal_Admin.Models;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;

namespace BudgetMetal_Admin.Controllers
{
    public class GalleryController : Controller
    {
        private readonly AppDbContext _context;

        public GalleryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Gallery
        public async Task<IActionResult> Index()
        {
            return View(await _context.bm_gallery.ToListAsync());
        }

        // GET: Gallery/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bm_gallery = await _context.bm_gallery
                .SingleOrDefaultAsync(m => m.Id == id);
            if (bm_gallery == null)
            {
                return NotFound();
            }

            return View(bm_gallery);
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

        // GET: Gallery/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gallery/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] bm_gallery bm_gallery, IFormFile ThumbnailImage, IFormFile DetailImage, IFormFile DownloadableImage)
        {

            Byte[] bytes = null;
            if (ThumbnailImage == null || DetailImage == null || DownloadableImage == null)
            {

                return RedirectToAction(nameof(Index));
            }
            else
            {

                //Byte[] bytes = System.IO.File.ReadAllBytes(files_ThumbnailImage);
                bytes = ConvertFiletoBytes(ThumbnailImage);
                String ThumbnailImage_64 = Convert.ToBase64String(bytes);

                //bytes = System.IO.File.ReadAllBytes(files_DetailImage);
                bytes = ConvertFiletoBytes(DetailImage);
                String DetailImage_64 = Convert.ToBase64String(bytes);

                //bytes = System.IO.File.ReadAllBytes(files_DownloadableImage);
                bytes = ConvertFiletoBytes(DownloadableImage);
                String DownloadableImage_64 = Convert.ToBase64String(bytes);


                bm_gallery.ThumbnailImage = ThumbnailImage_64;
                bm_gallery.DetailImage = DetailImage_64;
                bm_gallery.DownloadableImage = DownloadableImage_64;
                bm_gallery.IsActive = true;
                bm_gallery.CreatedBy = "system";
                bm_gallery.UpdatedBy = "system";
                bm_gallery.CreatedDate = DateTime.Now;
                bm_gallery.UpdatedDate = DateTime.Now;
                bm_gallery.Version = "1";
                _context.Add(bm_gallery);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }



        }

        private byte[] ConvertFiletoBytes(IFormFile file)
        {
            byte[] p1 = null;
            if (file != null)
            {
                if (file.Length > 0)
                //Convert Image to byte and save to database
                {
                    using (var fs1 = file.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                }
            }
            return p1;
        }

        // GET: Gallery/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bm_gallery = await _context.bm_gallery.SingleOrDefaultAsync(m => m.Id == id);
            if (bm_gallery == null)
            {
                return NotFound();
            }
            return View(bm_gallery);
        }

        // POST: Gallery/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ThumbnailImage,DetailImage,DownloadableImage,CreatedDate,CreatedBy,IsActive,Version")] bm_gallery bm_gallery, IFormFile ThumbnailImage_file, IFormFile DetailImage_file, IFormFile DownloadableImage_file)
        {
            if (id != bm_gallery.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ThumbnailImage_file == null)
                    {

                        //return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        Byte[] bytes = ConvertFiletoBytes(ThumbnailImage_file);
                        String ThumbnailImage_64 = Convert.ToBase64String(bytes);

                        bm_gallery.ThumbnailImage = ThumbnailImage_64;

                    }
                    if (DetailImage_file == null)
                    {

                        //return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        Byte[] bytes = ConvertFiletoBytes(DetailImage_file);
                        String DetailImage_64 = Convert.ToBase64String(bytes);


                        bm_gallery.DetailImage = DetailImage_64;

                    }
                    if (DownloadableImage_file == null)
                    {

                        //return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        Byte[] bytes = ConvertFiletoBytes(DownloadableImage_file);
                        String DownloadableImage_64 = Convert.ToBase64String(bytes);

                        bm_gallery.DownloadableImage = DownloadableImage_64;
                    }
                    bm_gallery.UpdatedBy = "system";
                    bm_gallery.UpdatedDate = DateTime.Now;
                    _context.Update(bm_gallery);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!bm_galleryExists(bm_gallery.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bm_gallery);
        }

        // GET: Gallery/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bm_gallery = await _context.bm_gallery
                .SingleOrDefaultAsync(m => m.Id == id);
            if (bm_gallery == null)
            {
                return NotFound();
            }

            return View(bm_gallery);
        }

        // POST: Gallery/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bm_gallery = await _context.bm_gallery.SingleOrDefaultAsync(m => m.Id == id);
            _context.bm_gallery.Remove(bm_gallery);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool bm_galleryExists(int id)
        {
            return _context.bm_gallery.Any(e => e.Id == id);
        }
    }
}
