﻿using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Gallery
{
    public interface IRoleRepository : IGenericRepository<bm_gallery>
    {
        PageResult<bm_gallery> GetGalleriesByPage(string keyword, int page, int totalRecords);

        bm_gallery GetGalleryById(int Id );

        bm_gallery GetGalleryFileById(int Id);
    }
}
