namespace ErrorLog.Repository
{
    using ErrorLog.Context;
    using SimpleInfra.Data;

    public class ErrorLogRepository<T>
            : SimpleBaseDataRepository<T> where T : class
    {
        public ErrorLogRepository() : base(new LogContext())
        {
        }
    }
}