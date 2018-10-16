using Com.BudgetMetal.DataRepository.Base;
using Com.BudgetMetal.DB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Com.BudgetMetal.DataRepository.CompanySupplier
{
    public class CompanySupplierRepository : GenericRepository<Com.BudgetMetal.DBEntities.CompanySupplier>, ICompanySupplierRepository
    {
        public CompanySupplierRepository(DataContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory, "CompanySupplierRepository")
        {
        }

        public void InactivePreferedCompanySupplier(int companyId, int supplierId, string updatedBy)
        {
            var dbResult = this.entities.Where(e => e.IsActive == true && e.Company_Id == companyId && e.Supplier_Id==e.Supplier_Id).ToList();
            dbResult.ForEach(e =>
            {
                e.IsActive = false;
                e.UpdatedDate = DateTime.Now;
                e.UpdatedBy = updatedBy;
            }
            );
        }

        public bool IsExistedSupplier(int companyId, int supplierId)
        {
            var dbResult = this.entities.Where(e => e.IsActive == true && e.Company_Id == companyId && e.Supplier_Id == e.Supplier_Id).ToList();
            return (dbResult.Count > 0) ? true : false;
        }
    }
}
