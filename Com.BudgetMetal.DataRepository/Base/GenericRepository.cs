using Com.BudgetMetal.DB;
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
    public class GenericRepository<T> : IGenericRepository<T> where T : GenericEntity
    {
        protected readonly DataContext DbContext;
        protected readonly ILogger repoLogger;

        protected DbSet<T> entities;

        string errorMessage = string.Empty;

        public GenericRepository(DataContext context, ILoggerFactory loggerFactory, string childClassName)
        {
            this.DbContext = context;
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
            this.DbContext.SaveChanges();
        }

        public T Add(T entity)
        {
            if (entity == null)
            {
                repoLogger.LogError("Cannot insert entiry as entity is null", null);
                throw new ArgumentNullException("entity");
            }

            entity.CreatedDate = entity.UpdatedDate = DateTime.Now;
            entity.Version = _GenerateVersionNumber();
            entity.IsActive = true;

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

            entity.UpdatedDate = DateTime.Now;
            entity.Version = _GenerateVersionNumber();

            entities.Update(entity);
            //entities.Update(entity).State = EntityState.Modified;
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

        private string _GenerateVersionNumber()
        {
            return Guid.NewGuid().ToShortString();
        }
    }
}
