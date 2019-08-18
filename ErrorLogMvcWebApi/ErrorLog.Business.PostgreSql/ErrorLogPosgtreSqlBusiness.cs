namespace ErrorLog.Business.PostgreSql
{
    using ErrorLog.Business.PostgreSqlDb;
    using System.Configuration;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An error log SQL business. </summary>
    ///
    /// <remarks>   Mustafa SAÇLI, 26.04.2019. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class ErrorLogPosgtreSqlBusiness : ErrorLogBasePostgreSqlBusiness
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Mustafa SAÇLI, 26.04.2019. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public ErrorLogPosgtreSqlBusiness()
            : base(
                  ConfigurationManager.AppSettings["errorLogConnName"],
                  ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["errorLogConnStringName"]].ConnectionString)
        { }
    }
}
