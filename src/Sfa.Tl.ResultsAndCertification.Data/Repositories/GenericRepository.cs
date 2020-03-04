using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity, new()
    {
        private readonly ILogger _logger;
        private readonly ResultsAndCertificationDbContext _dbContext;

        public GenericRepository(ILogger<GenericRepository<T>> logger, ResultsAndCertificationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public virtual async Task<int> CreateAsync(T entity)
        {
            await _dbContext.AddAsync(entity);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }

            return entity.Id;
        }

        public virtual async Task<int> CreateManyAsync(IList<T> entities)
        {
            await _dbContext.AddRangeAsync(entities);

            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }
        }

        public virtual async Task<int> UpdateAsync(T entity)
        {
            _dbContext.Update(entity);

            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }
        }

        public virtual async Task<int> UpdateWithSpecifedColumnsOnlyAsync(T entity, params Expression<Func<T, object>>[] properties)
        {
            properties.ToList().ForEach(p =>
            {
                _dbContext.Entry(entity).Property(p).IsModified = true;
            });

            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _dbContext.Remove(entity);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }
        }

        public virtual async Task<int> DeleteAsync(int id)
        {
            var entity = new T
            {
                Id = id
            };

            _dbContext.Attach(entity);
            _dbContext.Remove(entity);

            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }
        }

        public virtual async Task DeleteManyAsync(IList<T> entities)
        {
            if (entities.Count == 0) return;

            _dbContext.RemoveRange(entities);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }
        }

        public IQueryable<T> GetManyAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = GetQueryableWithIncludes(predicate, null, true, navigationPropertyPath);
            return predicate != null ? queryable.Where(predicate) : queryable;
        }
       
        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = GetQueryableWithIncludes(predicate, null, true, navigationPropertyPath);
            return await queryable.FirstOrDefaultAsync();
        }

        public async Task<TDto> GetFirstOrDefaultAsync<TDto>(Expression<Func<T, bool>> predicate, Expression<Func<T, TDto>> selector, Expression<Func<T, object>> orderBy, bool asendingorder = true, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = GetQueryableWithIncludes(predicate, orderBy, asendingorder, navigationPropertyPath);
            return await queryable.Select(selector).FirstOrDefaultAsync();
        }

        public async Task<T> GetSingleOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = GetQueryableWithIncludes(predicate, null, true, navigationPropertyPath);

            try
            {
                return await queryable.SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TDto> GetSingleOrDefaultAsync<TDto>(Expression<Func<T, bool>> predicate, Expression<Func<T, TDto>> selector, Expression<Func<T, object>> orderBy, bool asendingorder = true, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = GetQueryableWithIncludes(predicate, orderBy, asendingorder, navigationPropertyPath);
            return await queryable.Select(selector).SingleOrDefaultAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            return predicate != null ? await _dbContext.Set<T>().CountAsync(predicate) :
                await _dbContext.Set<T>().CountAsync();
        }

        private IQueryable<T> GetQueryableWithIncludes(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderBy, bool asendingorder = true, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = _dbContext.Set<T>().AsQueryable();

            if (navigationPropertyPath != null && navigationPropertyPath.Any())
            {
                queryable = navigationPropertyPath.Aggregate(queryable, (current, navProp) => current.Include(navProp));
            }

            if (predicate != null)
                queryable = queryable.Where(predicate);

            if (orderBy != null)
                queryable = asendingorder ? queryable.OrderBy(orderBy) : queryable.OrderByDescending(orderBy);

            return queryable;
        }
    }
}
