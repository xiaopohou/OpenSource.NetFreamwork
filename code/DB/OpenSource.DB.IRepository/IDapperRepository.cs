using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using OpenSource.Helps;

namespace OpenSource.DB.IRepository
{
    public interface IDapperRepository<TEntity> where TEntity : class
    {
        TEntity Find(Expression<Func<TEntity, bool>> expression);
        IEnumerable<TEntity> FindAll();
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> expression);
        bool Insert(TEntity instance);

        bool Delete(TEntity instance);

        bool Update(TEntity instance);

        IEnumerable<TEntity> FindAllBetween(object from, object to, Expression<Func<TEntity, object>> btwField);

        IEnumerable<TEntity> FindAllBetween(object from, object to, Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> expression);
        PageListView<TEntity> FindAllPages(long from, long to, Expression<Func<TEntity, bool>> expression);
    }
}