using RFIDDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RFIDDAL.Repositories.Contracts
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        T FindById(int id);
        void Create(T entity);
        void CreateBulk(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateBulk(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteBulk(IEnumerable<T> entities);
        PagedResult<T> GetAllPaged(int page, int pageSize);
        public PagedResult<T> FindByConditionPager(int page, int pageSize, Expression<Func<T, bool>> expression);

    }
}
