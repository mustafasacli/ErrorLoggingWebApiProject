namespace ErrorLog.Context
{
    using ErrorLog.Entity;
    using System.Data.Entity;

    /// <summary>
    /// 
    /// </summary>
    public class LogContext : DbContext
    {
        /// <summary>
        ///  constructor
        /// </summary>
        public LogContext() : base("name=ErrorLogContext")
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<ErrorLogEntity> ErrorLogList
        { get; set; }
    }
}