using Com.BudgetMetal.DB;
using Com.BudgetMetal.DBEntities;
using Com.BudgetMetal.DataRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Com.BudgetMetal.Common;

namespace Com.BudgetMetal.DataRepository.Base
{
    public class GenericRepository_BM<T> : IGenericRepository_BM<T> where T : GenericEntity
    {
        protected readonly DataContext_BM DbContext_BM;
        protected readonly ILogger repoLogger;

        protected DbSet<T> entities;

        string errorMessage = string.Empty;

        public GenericRepository_BM(DataContext_BM context, ILoggerFactory loggerFactory, string childClassName)
        {
            this.DbContext_BM = context;
            entities = context.Set<T>();
            repoLogger = loggerFactory.CreateLogger(childClassName);
        }

        public IEnumerable<T> GetAll()
        {
            repoLogger.LogDebug("GetAll", null);
            return entities.Where(s => s.IsActive == true).ToList();
        }

        public async Task<T> Get(int id)
        {
            repoLogger.LogDebug("Get by id " + id, null);

            var result = entities.SingleOrDefault(s => s.Id == id && s.IsActive == true);

            return result;
        }

        public void Commit()
        {
            this.DbContext_BM.SaveChanges();
        }

        public T Add(T entity)
        {
            if (entity == null)
            {
                repoLogger.LogError("Cannot insert entiry as entity is null", null);
                throw new ArgumentNullException("entity");
            }

            entities.Add(entity);

            var result = entity;

            return result;
        }

        public T Update(T entity)
        {
            if (entity == null)
            {
                repoLogger.LogError("Cannot insert entiry as entity is null", null);
                throw new ArgumentNullException("entity");
            }

            var result = entity;

            return result;
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                repoLogger.LogError("Cannot insert entiry as entity is null", null);
                throw new ArgumentNullException("entity");
            }

            entities.Remove(entity);
           
        }

        public virtual async Task<PageResult<T>> GetPage(string keyword, int page, int totalRecords = 10)
        {
            var records = await entities.Where(e => e.IsActive == true)
                           .OrderBy(e => e.CreatedDate)
                           .Skip((totalRecords * page) - totalRecords)
                           .Take(totalRecords)
                           .ToListAsync<T>();

            int count = await entities.CountAsync(e => e.IsActive == true);

            var result = new PageResult<T>()
            {
                Records = records,
                TotalPage = (count + totalRecords - 1) / totalRecords,
                CurrentPage = page,
                TotalRecords = count
            };

            return result;
        }
    }
}
