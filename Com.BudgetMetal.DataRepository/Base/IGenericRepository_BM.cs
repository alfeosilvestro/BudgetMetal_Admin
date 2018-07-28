using Com.BudgetMetal.Common;
using Com.BudgetMetal.DBEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Base
{
    public interface IGenericRepository_BM<T> where T : GenericEntity
    {
        T Add(T entity);
        T Update(T entity);
        void Delete(T entity);
        
        Task<T> Get(int id);
        IEnumerable<T> GetAll();
        Task<PageResult<T>> GetPage(string keyword, int page, int totalRecords = 10);

        void Commit();
    }
}
