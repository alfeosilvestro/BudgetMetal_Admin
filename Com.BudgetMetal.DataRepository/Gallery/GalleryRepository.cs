using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Com.BudgetMetal.DB.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Gallery
{
    public class GalleryRepository : GenericRepository<bm_gallery>, IGalleryRepository
    {
        public GalleryRepository(DataContext context, ILoggerFactory loggerFactory) :
       base(context, loggerFactory, "GalleryRepository")
        {

        }

        public PageResult<bm_gallery> GetGalleriesByPage(string keyword, int page, int totalRecords)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                keyword = string.Empty;
                //return await base.GetPage(keyword, page, totalRecords);
            }

            var records = this.DbContext.bm_gallery.Where(e =>
                (keyword == string.Empty ||
                e.Name.Contains(keyword) ||
                e.Description.Contains(keyword))
            );

            var recordList = records
            .Select(r =>
                new bm_gallery()
                {
                    Id = r.Id,
                    Name = r.Name,
                    ThumbnailImage = r.ThumbnailImage,
                    Description = r.Description,
                    CreatedDate = r.CreatedDate
                })
            .OrderBy(e => e.Name)
            .OrderBy(e => e.CreatedDate)
            .Skip((totalRecords * page) - totalRecords)
            .Take(totalRecords)
            .ToList();
            //DetailImage = (getDetailImage ? r.DetailImage : null),

            var count = records.Count();

            var nextPage = 0;
            var prePage = 0;
            if (page > 1)
            {
                prePage = page - 1;
            }

            var totalPage = (count + totalRecords - 1) / totalRecords;
            if (page < totalPage)
            {
                nextPage = page + 1;
            }

            var result = new PageResult<bm_gallery>()
            {
                Records = recordList,
                TotalPage = totalPage,
                CurrentPage = page,
                PreviousPage = prePage,
                NextPage = nextPage,
                TotalRecords = count
            };

            return result;
        }



        public bm_gallery GetGalleryById(int Id)
        {
            var records = this.DbContext.bm_gallery.Select(r =>
                new bm_gallery()
                {
                    Id = r.Id,
                    Name = r.Name,
                    DetailImage = r.DetailImage,
                    Description = r.Description,
                    CreatedDate = r.CreatedDate,
                    CreatedBy = r.CreatedBy
                })
                .Single(e =>
                e.Id == Id);

            return records;
        }

        public bm_gallery GetGalleryFileById(int Id)
        {
            var records = this.DbContext.bm_gallery.Select(r =>
                new bm_gallery()
                {
                    Id = r.Id,
                    Name = r.Name,
                    DownloadableImage = r.DownloadableImage,
                    CreatedDate = r.CreatedDate
                })
                .Single(e =>
                e.Id == Id);

            return records;
        }

    }
}
