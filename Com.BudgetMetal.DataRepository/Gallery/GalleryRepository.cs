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

        public PageResult<bm_gallery> GetGalleriesByPage(string keyword, int page, int totalRecords, bool getDetailImage)
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
                    DetailImage = (getDetailImage ? r.DetailImage : null),
                    Description = r.Description,
                    CreatedDate = r.CreatedDate
                })
            .OrderBy(e => e.Name)
            .OrderBy(e => e.CreatedDate)
            .Skip((totalRecords * page) - totalRecords)
            .Take(totalRecords)
            .ToList();

            var count = records.Count();

            var result = new PageResult<bm_gallery>()
            {
                Records = recordList,
                TotalPage = (count + totalRecords - 1) / totalRecords,
                CurrentPage = page,
                TotalRecords = count
            };

            return result;
        }
    }
}
