﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Com.BudgetMetal.DataRepository.Base;

namespace Com.BudgetMetal.DataRepository.RfqInvites
{
    public interface IRfqInvitesRepository : IGenericRepository<Com.BudgetMetal.DBEntities.RfqInvites>
    {
        void InactiveByRFQId(int rfqId, string UpdatedBy);

        Task<Com.BudgetMetal.DBEntities.RfqInvites> GetRfqInvitesWithEmailandAccessCode(string email, string accessCode);
    }
}