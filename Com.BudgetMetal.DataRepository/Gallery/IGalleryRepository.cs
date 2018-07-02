using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Gallery
{
    public interface IGalleryRepository : IGenericRepository<bm_gallery>
    {
        PageResult<bm_gallery> GetGalleriesByPage(string keyword, int page, int totalRecords, bool getDetailImage);

        bm_gallery GetGalleryById(int Id);
    }
}
