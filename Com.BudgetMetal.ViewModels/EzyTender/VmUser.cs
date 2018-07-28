﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.EzyTender
{
    public class VmUser : ViewModelItemBase
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

        //public User()
        //{
        //    Clarification = new HashSet<Clarification>();
        //    DocumentUser = new HashSet<DocumentUser>();
        //    Rating = new HashSet<Rating>();
        //    UserRoles = new HashSet<UserRoles>();
        //}


        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string ContactName { get; set; }
        public string JobTitle { get; set; }
        public string ContactNumber { get; set; }
        public bool? IsConfirmed { get; set; }
        public uint CompanyId { get; set; }
        public uint UserType { get; set; }

        public VmCompany Company { get; set; }
        public VmCodeTable UserTypeNavigation { get; set; }
        public ICollection<VmClarification> Clarification { get; set; }
        public ICollection<VmDocumentUser> DocumentUser { get; set; }
        public ICollection<VmRating> Rating { get; set; }
        public ICollection<VmUserRoles> UserRoles { get; set; }
    }
}