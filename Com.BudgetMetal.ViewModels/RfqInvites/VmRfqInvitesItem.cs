using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.BudgetMetal.ViewModels.RfqInvites
{
    public class VmRfqInvitesItem : ViewModelItemBase
    {
        public int RfqId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string AccessCode { get; set; }
    }
}