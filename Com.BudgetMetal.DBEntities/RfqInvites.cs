using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.DBEntities
{
    public class RfqInvites : GenericEntity
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string AccessCode { get; set; }
        public string Status { get; set; }
        public DateTime? LastAccessTimestamp { get; set; }

        public int RfqId { get; set; }

        [ForeignKey("RfqId")]
        public virtual Rfq Rfq { get; set; }
    }
}
