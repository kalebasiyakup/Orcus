﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Orcus.Core.DataAccess.RepositoryPattern
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(Expression<Func<TEntity, bool>> filter = null);

        IQueryable<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null);

        IQueryable<TEntity> GetListPaging(Expression<Func<TEntity, bool>> filter, out int total, int index, int size);

        TEntity Insert(TEntity entity);
        TEntity Update(TEntity entity);
        void Delete(TEntity entity);
        void Delete(Expression<Func<TEntity, bool>> filter);
        IQueryable<TEntity> SelectQuery(string query, params object[] parameters);
        
        List<TEntity> GetMany(Expression<Func<TEntity, bool>> where);

        IRepository<T> GetRepository<T>() where T : class;

        IQueryable<TEntity> Queryable();
    }
} 