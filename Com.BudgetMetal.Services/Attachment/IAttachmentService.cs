using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.Attachment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.Attachment
{
    public interface IAttachmentService
    {

        Task<VmAttachmentItem> GetAttachmentById(int Id);

    }
}
