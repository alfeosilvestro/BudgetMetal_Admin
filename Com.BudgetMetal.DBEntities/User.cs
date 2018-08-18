using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.BudgetMetal.DBEntities
{
    public class User : GenericEntity
    {
        //public int UserTypeId { get; set; }

        //public string Email { get; set; }

        //public string Password { get; set; }

        //public string UserName { get; set; }

        //public int RoleId { get; set; }

        //public string SiteAdmin { get; set; }

        //public string Title { get; set; }

        //public bool Status { get; set; }

        //public bool Confirmed { get; set; }

        public User()
        {
            //Clarification = new HashSet<Clarification>();
            //DocumentUser = new HashSet<DocumentUser>();
            //Rating = new HashSet<Rating>();
            UserRoles = new HashSet<UserRoles>();
        }

        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string ContactName { get; set; }
        public string JobTitle { get; set; }
        public string ContactNumber { get; set; }
        public bool IsConfirmed { get; set; }
        

        [ForeignKey("Company")]
        public int Company_Id { get; set; }
        public virtual Company Company { get; set; }
        [ForeignKey("CodeTable")]
        public int UserType { get; set; }
        public virtual CodeTable CodeTable { get; set; }
        //public ICollection<Clarification> Clarification { get; set; }
        //public ICollection<DocumentUser> DocumentUser { get; set; }
        //public ICollection<Rating> Rating { get; set; }
        //public ICollection<UserRoles> UserRoles { get; set; }
        public virtual ICollection<UserRoles> UserRoles { get; } = new List<UserRoles>();
    }
}
