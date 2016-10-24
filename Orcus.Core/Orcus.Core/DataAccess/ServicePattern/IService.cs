using Orcus.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Orcus.Core.DataAccess.ServicePattern
{
    public interface IService<TEntity>
    {
        Result<TEntity> Get(Expression<Func<TEntity, bool>> filter = null);
        Result<IEnumerable<TEntity>> GetList(Expression<Func<TEntity, bool>> filter = null);
        Result<IEnumerable<TEntity>> GetListPaging(Expression<Func<TEntity, bool>> filter, out int total, int index = 0, int size = 15);
        Result<TEntity> Insert(TEntity entity);
        Result<TEntity> Update(TEntity entity);
        Result<bool> Delete(TEntity entity);
        Result<bool> Delete(Expression<Func<TEntity, bool>> filter);
    }
}
