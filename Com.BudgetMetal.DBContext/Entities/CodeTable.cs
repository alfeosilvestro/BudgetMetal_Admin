using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.DB.Entities
{
    public class CodeTable : GenericEntity
    {
        public CodeTable()
        {
            DocumentDocumentStatus = new HashSet<Document>();
            DocumentDocumentType = new HashSet<Document>();
            User = new HashSet<User>();
        }

       
        public uint CodeCategoryId { get; set; }
        public string Name { get; set; }

        public CodeCategory CodeCategory { get; set; }
        public ICollection<Document> DocumentDocumentStatus { get; set; }
        public ICollection<Document> DocumentDocumentType { get; set; }
        public ICollection<User> User { get; set; }
    }
}
