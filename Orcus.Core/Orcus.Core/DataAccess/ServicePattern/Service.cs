using Orcus.Core.Common;
using Orcus.Core.DataAccess.RepositoryPattern;
using Orcus.Core.DataAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Orcus.Core.DataAccess.ServicePattern
{
    public abstract class Service<TEntity> : IService<TEntity> where TEntity : class
    {
        #region Private Fields
        private readonly IRepository<TEntity> _repository;
        #endregion Private Fields

        #region Constructor
        protected Service(IUnitOfWork unitOfWork)
        {
            _repository = unitOfWork.Repository<TEntity>();
        }
        #endregion Constructor

        #region Methods
        public virtual Result<TEntity> Get(Expression<Func<TEntity, bool>> filter = null)
        {
            Result<TEntity> result;

            try
            {
                result = new Result<TEntity>(_repository.Get(filter));
            }
            catch (Exception ex)
            {
                result = new Result<TEntity>(ex);
            }

            return result;
        }

        public virtual Result<IEnumerable<TEntity>> GetList(Expression<Func<TEntity, bool>> filter = null)
        {
            Result<IEnumerable<TEntity>> result;
            try
            {
                result = new Result<IEnumerable<TEntity>>(_repository.GetList(filter));
            }
            catch (Exception ex)
            {
                result = new Result<IEnumerable<TEntity>>(ex);
            }

            return result;
        }

        public virtual Result<IEnumerable<TEntity>> GetListPaging(Expression<Func<TEntity, bool>> filter, out int total, int index = 0, int size = 15)
        {
            Result<IEnumerable<TEntity>> result;

            try
            {
                result = new Result<IEnumerable<TEntity>>(_repository.GetListPaging(filter, out total, index, size));
            }
            catch (Exception ex)
            {
                total = 0;
                result = new Result<IEnumerable<TEntity>>(ex);
            }

            return result;
        }

        public virtual Result<TEntity> Insert(TEntity entity)
        {
            Result<TEntity> result;
            try
            {
                result = new Result<TEntity>(_repository.Insert(entity));
            }
            catch (Exception ex)
            {
                result = new Result<TEntity>(ex);
            }

            return result;
        }

        public virtual Result<TEntity> Update(TEntity entity)
        {
            Result<TEntity> result;
            try
            {
                result = new Result<TEntity>(_repository.Update(entity));
            }
            catch (Exception ex)
            {
                result = new Result<TEntity>(ex);
            }

            return result;
        }

        public virtual Result<bool> Delete(TEntity entity)
        {
            Result<bool> result;
            try
            {
                _repository.Delete(entity);
                result = new Result<bool>(true);
            }
            catch (Exception ex)
            {
                result = new Result<bool>(ex);
            }

            return result;
        }

        public virtual Result<bool> Delete(Expression<Func<TEntity, bool>> filter)
        {
            Result<bool> result;
            try
            {
                _repository.Delete(filter);
                result = new Result<bool>(true);
            }
            catch (Exception ex)
            {
                result = new Result<bool>(ex);
            }

            return result;
        }
        #endregion
    }
}
