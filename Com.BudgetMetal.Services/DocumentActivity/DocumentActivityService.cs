using Com.BudgetMetal.DataRepository.DocumentActivity;
using Com.BudgetMetal.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.Services.DocumentActivity
{
    public class DocumentActivityService : BaseService, IDocumentActivityService
    {
        private readonly IDocumentActivityRepository repo;

        public DocumentActivityService(IDocumentActivityRepository repo)
        {
            this.repo = repo;
        }
    }
}
