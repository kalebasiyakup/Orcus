using Orcus.Core.DataAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

namespace Orcus.Core.DataAccess.RepositoryPattern
{
    class EfRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly DbContext _dataContext;
        private readonly IUnitOfWork _unitOfWork;
        private string _errorMessage;

        public EfRepository(DbContext dataContext, IUnitOfWork unitOfWork)
        {
            _dataContext = dataContext;
            _unitOfWork = unitOfWork;
        }

        private DbSet<TEntity> _dbset => _dataContext.Set<TEntity>();

        public virtual TEntity Get(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter == null ? _dbset.FirstOrDefault() : _dbset.FirstOrDefault(filter);
        }

        public virtual IQueryable<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter == null ? _dbset : _dbset.Where(filter);
        }

        public IQueryable<TEntity> GetListPaging(Expression<Func<TEntity, bool>> filter, out int total, int index, int size)
        {
            int skipCount = index * size;
            var resetSet = filter != null ? _dbset.Where(filter).AsQueryable() : _dbset.AsQueryable();

            resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);

            total = resetSet.Count();

            return resetSet;
        }
        
        public virtual TEntity Insert(TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    throw GetDbEntityValidationExceptionError(null, "Insert(T entity) - entity is null");
                }

                return _dbset.Add(entity);
            }
            catch (DbEntityValidationException dbEx)
            {
                throw GetDbEntityValidationExceptionError(dbEx, "Insert(T entity)");
            }
        }

        public virtual TEntity Update(TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    throw GetDbEntityValidationExceptionError(null, "Update(T entity) - entity is null");
                }

                var updateEntity = _dataContext.Entry(entity);
                updateEntity.State = EntityState.Modified;
                return updateEntity.Entity;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw GetDbEntityValidationExceptionError(dbEx, "Update(T entity)");
            }
        }
        
        public virtual void Delete(TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    throw GetDbEntityValidationExceptionError(null, "Delete(T entity) - entity is null");
                }

                var entry = _dataContext.Entry(entity);
                if (entry.State == EntityState.Detached)
                    _dbset.Attach(entity);
                _dbset.Remove(entity);
            }
            catch (DbEntityValidationException dbEx)
            {
                throw GetDbEntityValidationExceptionError(dbEx, "Delete(T entity)");
            }
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> filter)
        {
            TEntity entity = Get(filter);
            if (entity != null)
            {
                try
                {
                    var entry = _dataContext.Entry(entity);
                    if (entry.State == EntityState.Detached)
                        _dbset.Attach(entity);
                    _dbset.Remove(entity);

                }
                catch (DbEntityValidationException dbEx)
                {
                    throw GetDbEntityValidationExceptionError(dbEx, "Delete(T entity)");
                }
            }
        }

        public virtual IQueryable<TEntity> SelectQuery(string query, params object[] parameters)
        {
            return _dbset.SqlQuery(query, parameters).AsQueryable();
        }
        
        public virtual List<TEntity> GetMany(Expression<Func<TEntity, bool>> where)
        {
            return _dbset.Where(where).ToList();
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return _unitOfWork.Repository<T>();
        }

        public IQueryable<TEntity> Queryable()
        {
            return _dbset;
        }

        private Exception GetDbEntityValidationExceptionError(DbEntityValidationException dbEx, string methodName)
        {
            Exception exception = new Exception(methodName);

            if (dbEx == null)
            {
                return exception;
            }

            foreach (var validationError in dbEx.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors))
            {
                _errorMessage += $"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}" + Environment.NewLine;
            }

            exception.Data.Add("Method Name", methodName);
            exception.Data.Add("Log Detail", _errorMessage);

            return exception;
        }
    }
}
