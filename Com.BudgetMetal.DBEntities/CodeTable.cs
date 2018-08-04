using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.DBEntities
{
    public class CodeTable : GenericEntity
    {
        //public CodeTable()
        //{
        //    DocumentDocumentStatus = new HashSet<Document>();
        //    DocumentDocumentType = new HashSet<Document>();
        //    User = new HashSet<User>();
        //}
        
        public string Name { get; set; }

        [ForeignKey("CodeCategory")]
        public int CodeCategory_Id { get; set; }
        public virtual CodeCategory CodeCategory { get; set; }
        //public ICollection<Document> DocumentDocumentStatus { get; set; }
        //public ICollection<Document> DocumentDocumentType { get; set; }
        //public ICollection<User> User { get; set; }
    }
}
