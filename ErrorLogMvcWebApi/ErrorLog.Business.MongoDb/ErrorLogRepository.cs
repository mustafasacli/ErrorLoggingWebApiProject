namespace ErrorLog.Business.MongoDb
{
    using ErrorLog.Models;
    using Mst.MongoDb.Core;
    using System.Configuration;

    public class ErrorLogRepository : MongoDbBaseRepository<ErrorLogModel>
    {
        public ErrorLogRepository() :
            base(AppConstants.ErrorLogDbName,
                ConfigurationManager.ConnectionStrings[AppConstants.ErrorLogDbConnectionStringName].ConnectionString)
        {
        }
    }
}