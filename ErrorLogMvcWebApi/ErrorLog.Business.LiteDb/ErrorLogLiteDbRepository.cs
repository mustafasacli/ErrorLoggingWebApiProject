////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	ErrorLogLiteDbRepository.cs
//
// summary:	Implements the error log lite database repository class
////////////////////////////////////////////////////////////////////////////////////////////////////
namespace ErrorLog.Business.LiteDb
{
    using ErrorLog.Models;
    using Mst.LiteDb.Core;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An error log lite database repository. </summary>
    ///
    /// <remarks>   Mustafa SAÇLI, 9.05.2019. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class ErrorLogLiteDbRepository : LiteDbBaseRepository<ErrorLogModel>
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Mustafa SAÇLI, 9.05.2019. </remarks>
        ///
        /// <param name="connectionString"> The connection string. </param>
        /// <param name="mapper">           (Optional) The mapper. </param>
        /// <param name="log">              (Optional) The log. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public ErrorLogLiteDbRepository(string connectionString, LiteDB.BsonMapper mapper = null, LiteDB.Logger log = null)
            : base(connectionString, mapper, log)
        {
        }
    }
}