namespace ErrorLog.Business.MongoDb
{
    using ErrorLog.Models;
    using Mst.MongoDb.Core;
    using System.Configuration;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An error log repository. </summary>
    ///
    /// <remarks>   Msacli, 24.04.2019. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class ErrorLogRepository : MongoDbBaseRepository<ErrorLogModel>
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Msacli, 24.04.2019. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public ErrorLogRepository() :
            base(AppConstants.ErrorLogDbName,
                ConfigurationManager.ConnectionStrings[AppConstants.ErrorLogDbConnectionStringName].ConnectionString)
        {
        }
    }
}