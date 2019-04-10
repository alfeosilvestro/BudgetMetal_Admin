using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Rating;
using Com.BudgetMetal.DataRepository.Single_Sign_On;
using Com.BudgetMetal.DBEntities;
using Com.BudgetMetal.Services.Base;
using Com.BudgetMetal.ViewModels.Rating;
using System.Collections.Generic;
using System;
using Com.BudgetMetal.ViewModels;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.Rating
{
    public class RatingService : BaseService, IRatingService
    {
        private readonly IRatingRepository repo;

        public RatingService(IRatingRepository repo)
        {
            this.repo = repo;
        }

        public async Task<VmGenericServiceResult> Insert(VmRatingItem vmRatingItem)
        {
            VmGenericServiceResult result = new VmGenericServiceResult();

            try
            {
                BudgetMetal.DBEntities.Rating r = new BudgetMetal.DBEntities.Rating();

                Copy<VmRatingItem, BudgetMetal.DBEntities.Rating>(vmRatingItem, r);

                if (r.CreatedBy.IsNullOrEmpty())
                {
                    r.CreatedBy = r.UpdatedBy = "System";
                }

                repo.Add(r);

                repo.Commit();

                result.IsSuccess = true;
                result.MessageToUser = "Rating successfully submited.";
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Error = e;
            }

            return result;
        }

        public async Task<VmRatingPage> GetRatingData(int page, int totalRecords, int companyId, int statusId = 0, string keyword = "")
        {
            var dbPageResult = await repo.GetRatingData((page == 0 ? Constants.app_firstPage : page), companyId,
                (totalRecords == 0 ? Constants.app_totalRecords : totalRecords), keyword);


            if (dbPageResult == null)
            {
                return new VmRatingPage();
            }

            var resultObj = new VmRatingPage();
            resultObj.RequestId = DateTime.Now.ToString("yyyyMMddHHmmss");
            resultObj.RequestDate = DateTime.Now;
            resultObj.Result = new PageResult<VmRatingItem>();
            resultObj.Result.Records = new List<VmRatingItem>();
                        
            Copy<PageResult<Com.BudgetMetal.DBEntities.Rating>, PageResult<VmRatingItem>>(dbPageResult, resultObj.Result, new string[] { "Records" });

            foreach (var dbItem in dbPageResult.Records)
            {
                var resultItem = new VmRatingItem();

                Copy<Com.BudgetMetal.DBEntities.Rating, VmRatingItem>(dbItem, resultItem);
                resultItem.UserName = dbItem.User.UserName;
                resultObj.Result.Records.Add(resultItem);
            }

            return resultObj;
        }

    }
}
