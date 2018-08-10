using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.DataRepository.Attachment
{
   
    public class AttachmentRepository : GenericRepository<Com.BudgetMetal.DBEntities.Attachment>, IAttachmentRepository
    {
        public AttachmentRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "AttachmentRepository")
        {

        }

    }
}
