using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.DataRepository.Attachment
{
    public interface IAttachmentRepository : IGenericRepository<Com.BudgetMetal.DBEntities.Attachment>
    {
        void InactiveByDocumentId(int documentId, string UpdatedBy);
        void UpdateDescription(DBEntities.Attachment dbAttachment);
        void DeleteByDocumentId(int documentId);
    }

     
}
