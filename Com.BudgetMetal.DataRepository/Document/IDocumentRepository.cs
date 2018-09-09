using Com.BudgetMetal.DataRepository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.DataRepository.Document
{
    public interface IDocumentRepository: IGenericRepository<Com.BudgetMetal.DBEntities.Document>
    {
        int GetRfqCountByCompany(int companyId);

        int GetQuotationCountByCompany(int companyId);

        int GetRfqCountByCompanyAndWorkingPeriod(int companyId, string workingPeriod);

        int GetQuotationCountByCompanyAndWorkingPeriod(int companyId, string workingPeriod);


    }
}
