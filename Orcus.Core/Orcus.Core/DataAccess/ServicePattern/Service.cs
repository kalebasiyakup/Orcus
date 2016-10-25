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
            Result<TEntity> result = new Result<TEntity>();

            try
            {
                result.ResultObject = _repository.Get(filter);
                result.ResultCode = (int)ResultStatusCode.OK;
                result.ResultMessage = ResultStatusCode.OK.ToString();
                result.setTrue();

            }
            catch (Exception ex)
            {
                result.ResultCode = (int)ResultStatusCode.InternalServerError;
                result.ResultMessage = "Hata Oluştur => " + ex.ToString();
                result.setFalse();
            }

            return result;
        }

        public virtual Result<IEnumerable<TEntity>> GetList(Expression<Func<TEntity, bool>> filter = null)
        {
            Result<IEnumerable<TEntity>> result = new Result<IEnumerable<TEntity>>();
            try
            {
                result.ResultObject = _repository.GetList(filter);
                result.ResultCode = (int)ResultStatusCode.OK;
                result.ResultMessage = ResultStatusCode.OK.ToString();
                result.setTrue();

            }
            catch (Exception ex)
            {
                result.ResultCode = (int)ResultStatusCode.InternalServerError;
                result.ResultMessage = "Hata Oluştur => " + ex.ToString();
                result.setFalse();
            }

            return result;
        }

        public virtual Result<IEnumerable<TEntity>> GetListPaging(Expression<Func<TEntity, bool>> filter, out int total, int index = 0, int size = 15)
        {
            Result<IEnumerable<TEntity>> result = new Result<IEnumerable<TEntity>>();

            try
            {
                result.ResultObject = _repository.GetListPaging(filter, out total, index, size);
                result.ResultCode = (int)ResultStatusCode.OK;
                result.ResultMessage = ResultStatusCode.OK.ToString();
                result.setTrue();
            }
            catch (Exception ex)
            {
                total = 0;
                result.ResultCode = (int)ResultStatusCode.InternalServerError;
                result.ResultMessage = "Hata Oluştur => " + ex.ToString();
                result.setFalse();
            }

            return result;
        }

        public virtual Result<TEntity> Insert(TEntity entity)
        {
            Result<TEntity> result = new Result<TEntity>();
            try
            {
                result.ResultObject = _repository.Insert(entity);
                result.ResultCode = (int)ResultStatusCode.OK;
                result.ResultMessage = ResultStatusCode.OK.ToString();
                result.setTrue();


            }
            catch (Exception ex)
            {
                result.ResultCode = (int)ResultStatusCode.InternalServerError;
                result.ResultMessage = "Hata Oluştur => " + ex.ToString();
                result.setFalse();
            }

            return result;
        }

        public virtual Result<TEntity> Update(TEntity entity)
        {
            Result<TEntity> result = new Result<TEntity>();
            try
            {
                result.ResultObject = _repository.Update(entity);
                result.ResultCode = (int)ResultStatusCode.OK;
                result.ResultMessage = ResultStatusCode.OK.ToString();
                result.setTrue();

            }
            catch (Exception ex)
            {
                result.ResultCode = (int)ResultStatusCode.InternalServerError;
                result.ResultMessage = "Hata Oluştur => " + ex.ToString();
                result.setFalse();
            }

            return result;
        }

        public virtual Result<bool> Delete(TEntity entity)
        {
            Result<bool> result = new Result<bool>();
            try
            {
                _repository.Delete(entity);
                result.ResultCode = (int)ResultStatusCode.OK;
                result.ResultMessage = ResultStatusCode.OK.ToString();
                result.ResultObject = true;
                result.setTrue();

            }
            catch (Exception ex)
            {
                result.ResultCode = (int)ResultStatusCode.InternalServerError;
                result.ResultMessage = "Hata Oluştur => " + ex.ToString();
                result.ResultObject = true;
                result.setFalse();
            }

            return result;
        }

        public virtual Result<bool> Delete(Expression<Func<TEntity, bool>> filter)
        {
            var result = new Result<bool>();
            try
            {
                _repository.Delete(filter);
                result.ResultCode = (int)ResultStatusCode.OK;
                result.ResultMessage = ResultStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)ResultStatusCode.InternalServerError;
                result.ResultMessage = "Hata Oluştu => " + ex;
            }

            return result;
        } 
        #endregion
    }
}
