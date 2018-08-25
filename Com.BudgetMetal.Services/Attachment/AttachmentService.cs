using Com.BudgetMetal.Common;
using Com.BudgetMetal.DataRepository.Code_Category;
using Com.BudgetMetal.DataRepository.Code_Table;
using Com.BudgetMetal.DBEntities;
using Com.BudgetMetal.Services.Base;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.CodeTable;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Com.BudgetMetal.DataRepository.Attachment;
using Com.BudgetMetal.ViewModels.Attachment;

namespace Com.BudgetMetal.Services.Attachment
{
    public class AttachmentService : BaseService, IAttachmentService
    {
        private readonly IAttachmentRepository repo;

        public AttachmentService(IAttachmentRepository repo)
        {
            this.repo = repo;
        }

      
        public async Task<VmAttachmentItem> GetAttachmentById(int Id)
        {
            var dbPageResult = await repo.Get(Id);

            if (dbPageResult == null)
            {
                return new VmAttachmentItem();
            }

            var resultObj = new VmAttachmentItem();

            Copy<Com.BudgetMetal.DBEntities.Attachment, VmAttachmentItem>(dbPageResult, resultObj);
            

            return resultObj;
        }

        

    }
}
