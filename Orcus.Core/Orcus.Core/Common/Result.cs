
using System;
using System.Text;

namespace Orcus.Core.Common
{
    public class Result
    {
        public bool ResultStatus { get; set; }
        public ResultStatusCode ResultCode { get; set; }
        public string ResultMessage { get; set; }
        public Exception Exception { get; set; }
        public bool HasError { get; set; }

        public Result()
        {
            ResultCode = ResultStatusCode.OK;
            ResultMessage = ResultStatusCode.OK.ToString();
            this.ResultStatus = true;
            this.HasError = false;
        }

        public Result(string message, bool hasError)
        {
            this.ResultCode = ResultStatusCode.InternalServerError;
            this.ResultMessage = message;
            this.ResultStatus = hasError;
            this.HasError = hasError;
        }

        public Result(Exception exception)
        {
            this.Exception = exception;

            StringBuilder builder = new StringBuilder();
            Exception iteration = exception;

            while (iteration != null)
            {
                builder.AppendLine(string.Format("{0} {1}", string.IsNullOrEmpty(builder.ToString()) ? "" : " - ", iteration.Message));
                iteration = iteration.InnerException;
            }

            this.ResultCode = ResultStatusCode.InternalServerError;
            this.ResultMessage = builder.ToString();
            this.ResultStatus = false;
            this.HasError = true;
        }
    }

    public class Result<T> : Result
    {
        public T ResultObject { get; set; }

        public Result() : base()
        {
        }

        public Result(T data) : base()
        {
            this.ResultObject = data;
        }

        public Result(T data, string message, bool hasError) : base(message, hasError)
        {
            this.ResultObject = data;
        }

        public Result(T data, Exception exception) : base(exception)
        {
            this.ResultObject = data;
        }

        public Result(Exception exception) : base(exception)
        {
        }
    }
}
