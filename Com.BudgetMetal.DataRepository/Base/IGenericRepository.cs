using Com.BudgetMetal.Common;
using Com.BudgetMetal.DB;
using Com.BudgetMetal.DBEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Base
{
    public interface IGenericRepository<T> where T : GenericEntity
    {
        T Add(T entity);
        T Update(T entity);
        void Delete(T entity);
        
        Task<T> Get(int id);
        Task<IEnumerable<T>> GetAll();
        Task<PageResult<T>> GetPage(string keyword, int page, int totalRecords = 10);

        void Commit();
    }
}
