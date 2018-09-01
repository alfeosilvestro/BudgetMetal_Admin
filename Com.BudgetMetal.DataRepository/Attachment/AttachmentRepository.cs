using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.BudgetMetal.DataRepository.Attachment
{
   
    public class AttachmentRepository : GenericRepository<Com.BudgetMetal.DBEntities.Attachment>, IAttachmentRepository
    {
        public AttachmentRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "AttachmentRepository")
        {

        }
        public void InactiveByDocumentId(int documentId, string UpdatedBy)
        {
            var dbResult = this.entities.Where(e => e.IsActive == true && e.Document_Id == documentId).ToList();
            dbResult.ForEach(e =>
            {
                e.IsActive = false;
                e.UpdatedDate = DateTime.Now;
                e.UpdatedBy = UpdatedBy;
            }
            );
        }

        public void UpdateDescription(DBEntities.Attachment dbAttachment)
        {
            var dbResult = this.entities.Where(e => e.Id == dbAttachment.Id).ToList();
            dbResult.ForEach(e =>
            {
                e.IsActive = true;
                e.Description = dbAttachment.Description;
            });
        }

        public void DeleteByDocumentId(int documentId)
        {
            var dbResult = this.entities.Where(e => e.IsActive == false && e.Document_Id == documentId).ToList();
            foreach(var item in dbResult)
            {
                Delete(item);
            }

            Commit();
        }
    }
}
