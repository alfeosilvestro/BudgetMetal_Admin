using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DB.Entities
{
    public class single_sign_on: GenericEntity
    {
       
        public string Authentication_Token { get; set; }

        public string User_Email { get; set; }

        public int Status { get; set; }

        public DateTime Timeout { get; set; }
        

    }
}
