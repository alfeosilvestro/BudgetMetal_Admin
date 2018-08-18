using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.Document;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.Document
{
    public interface IDocumentService
    {
        Task<VmDocumentPage> GetDocumentByPage(string keyword, int page, int totalRecords);

        Task<VmDocumentItem> GetDocumentById(int Id);

        VmGenericServiceResult Insert(VmDocumentItem vmItem);

        Task<VmGenericServiceResult> Update(VmDocumentItem vmItem);

        Task Delete(int Id);

        Task<VmDocumentItem> GetFormObject();
    }
}
