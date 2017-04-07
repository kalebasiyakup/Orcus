namespace Orcus.Core.Common
{
    public enum ResultStatusCode
    {
        OK = 200,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        InternalServerError = 500,
        ExistingItem = 600,
        Warning = 700,
        Info = 800
    }

    public enum LogType
    {
        Error = -1,
        Info = 0,
        Success = 1
    }

    public enum Priority
    {
        None = -2,
        Low = -1,
        Normal = 0,
        High = 1
    }

    public enum OrderType
    {
        Ascending,
        Descending
    }

    public enum LoggingLevel
    {
        NoLog,
        Normal,
    }

    public enum ProductEnvironment
    {
        Development,
        Production
    }

    public enum LogModule
    {
        Unknown,
        Repository,
        AdminUI,
        WebUI,
        Business,
        Framework,
        ExceptionHandler
    }
}
