using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.DB.Entities
{
    public class User_2 : GenericEntity
    {
        public User_2()
        {
            Clarification = new HashSet<Clarification>();
            DocumentUser = new HashSet<DocumentUser>();
            Rating = new HashSet<Rating>();
            UserRoles = new HashSet<UserRoles>();
        }

        
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string ContactName { get; set; }
        public string JobTitle { get; set; }
        public string ContactNumber { get; set; }
        public bool? IsConfirmed { get; set; }
        public uint CompanyId { get; set; }
        public uint UserType { get; set; }

        public Company Company { get; set; }
        public CodeTable UserTypeNavigation { get; set; }
        public ICollection<Clarification> Clarification { get; set; }
        public ICollection<DocumentUser> DocumentUser { get; set; }
        public ICollection<Rating> Rating { get; set; }
        public ICollection<UserRoles> UserRoles { get; set; }
    }
}
