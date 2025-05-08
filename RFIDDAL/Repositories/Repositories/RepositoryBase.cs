using RFIDDAL.Models;
using RFIDDAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RFIDDAL.Repositories.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RFIDdbContext RepositoryContext { get; set; }
        public RepositoryBase(RFIDdbContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }
        public IQueryable<T> FindAll()
        {
            return RepositoryContext.Set<T>().AsNoTracking();
        }
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return RepositoryContext.Set<T>().Where(expression);
        }
        public T FindById(int id)
        {
            return RepositoryContext.Set<T>().Find(id);
        }
        public void Create(T entity)
        {
            RepositoryContext.Set<T>().Add(entity);
        }
        public void CreateBulk(IEnumerable<T> entities)
        {
            RepositoryContext.Set<T>().AddRange(entities);
        }
        public void Update(T entity)
        {
            RepositoryContext.Set<T>().Update(entity);
        }
        public void UpdateBulk(IEnumerable<T> entities)
        {
            RepositoryContext.Set<T>().UpdateRange(entities);
        }
        public void Delete(T entity)
        {
            RepositoryContext.Set<T>().Remove(entity);
        }
        public void DeleteBulk(IEnumerable<T> entities)
        {
            RepositoryContext.Set<T>().RemoveRange(entities);
        }
        public PagedResult<T> GetAllPaged(int page, int pageSize)
        {
            var result = new PagedResult<T>();
            var query = RepositoryContext.Set<T>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();


            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }
        public PagedResult<T> FindByConditionPager(int page, int pageSize, Expression<Func<T, bool>> expression)
        {
            var result = new PagedResult<T>();
            var query = RepositoryContext.Set<T>().Where(expression);
            result.CurrentPage = page;
            result.PageSize = pageSize;
            try
            {
                result.RowCount = query.Count();
            }
            catch
            {
                result.RowCount = 0;
                return null;
            }
            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);
            var totalRecords = RepositoryContext.Set<T>().Count(); // Count all records in the table
            if (totalRecords > 0)
            {
                result.Percentage = (double)result.RowCount / totalRecords * 100; // Calculate the percentage
            }
            else
            {
                result.Percentage = 0;
            }
            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }
    }
}
