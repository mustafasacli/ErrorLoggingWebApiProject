////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	basesqlbusiness.cs
//
// summary:	Implements the basesqlbusiness class
////////////////////////////////////////////////////////////////////////////////////////////////////

using Mst.Dexter.Factory;
using System;
using System.Data;

////using Mst.DexterCfg.Factory;

namespace ErrorLog.Business.Sql.Core
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A Base SQL business. </summary>
    ///
    /// <remarks>   Mustafa SAÇLI, 25.04.2019. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public abstract class BaseSqlBusiness
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the name of the connection. </summary>
        ///
        /// <value> The name of the connection. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public string ConnectionName
        { get; protected set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the connection string. </summary>
        ///
        /// <value> The connection string. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        protected string ConnectionString
        { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the connection. </summary>
        ///
        /// <remarks>   Mustafa SAÇLI, 25.04.2019. </remarks>
        ///
        /// <returns>   The connection. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        protected virtual IDbConnection GetConnection()
        {
            var connection = DxConnectionFactory.Instance.GetConnection(this.ConnectionName);
            connection.ConnectionString = this.ConnectionString;
            return connection;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Specialised constructor for use only by derived class. </summary>
        ///
        /// <remarks>   Mustafa SAÇLI, 25.04.2019. </remarks>
        ///
        /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        ///
        /// <param name="connectionName">   The name of the connection. </param>
        /// <param name="connectionString"> The connection string. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        protected BaseSqlBusiness(string connectionName, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionName))
                throw new ArgumentNullException(nameof(connectionName));

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            this.ConnectionName = connectionName;
            this.ConnectionString = connectionString;
        }
    }
}