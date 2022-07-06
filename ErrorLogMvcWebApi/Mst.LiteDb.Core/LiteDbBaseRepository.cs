using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mst.LiteDb.Core
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A lite database repository. </summary>
    ///
    /// <remarks>   Mustafa SAÇLI, 9.05.2019. </remarks>
    ///
    /// <typeparam name="T">    Generic type parameter. </typeparam>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public abstract class LiteDbBaseRepository<T> : IDisposable where T : class
    {
        private LiteDatabase database = null;
        private readonly LiteCollection<T> liteCollection;
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Specialised constructor for use only by derived class. </summary>
        ///
        /// <remarks>   Mustafa SAÇLI, 9.05.2019. </remarks>
        ///
        /// <param name="connectionString"> The connection string. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        protected LiteDbBaseRepository(string connectionString, BsonMapper mapper = null, Logger log = null)
        {
            database = new LiteDatabase(connectionString, mapper, log);
            Collection = database.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
            liteCollection = database.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the collection. </summary>
        ///
        /// <value> The collection. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public LiteCollection<T> Collection
        { get; private set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        ///
        /// <remarks>   Mustafa SAÇLI, 9.05.2019. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Dispose()
        {
            database?.Dispose();
        }
    }
}
