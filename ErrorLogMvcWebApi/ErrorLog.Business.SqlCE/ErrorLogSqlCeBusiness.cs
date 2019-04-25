////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	ErrorLogSqlCeBusiness.cs
//
// summary:	Implements the error log SQL ce business class
////////////////////////////////////////////////////////////////////////////////////////////////////

using ErrorLog.Business.Core.Constants;
using ErrorLog.Business.Core.Interfaces;
using ErrorLog.Business.Sql.Core;
using ErrorLog.Models;
using Mst.Dexter.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace ErrorLog.Business.SqlCE
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An error log SQL ce business. </summary>
    ///
    /// <remarks>   Mustafa SAÇLI, 26.04.2019. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class ErrorLogSqlCeBusiness : BaseSqlBusiness, IErrorLogBusiness
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Mustafa SAÇLI, 26.04.2019. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public ErrorLogSqlCeBusiness()
            : base("sqlce", ConfigurationManager.ConnectionStrings["sqlce"].ConnectionString)
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deletes the given log. </summary>
        ///
        /// <remarks>   Mustafa SAÇLI, 26.04.2019. </remarks>
        ///
        /// <param name="log">  The log to save. </param>
        ///
        /// <returns>   An int. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public int Delete(ErrorLogModel log)
        {
            if (log == null)
                return CoreConstants.NullLogResponse;

            if (string.IsNullOrWhiteSpace(log.Id))
                return CoreConstants.EmptyLogIdResponse;

            var emptystring = Guid.Empty.ToString();

            if (log.Id == emptystring)
                return CoreConstants.EmptyGuidLogIdResponse;

            emptystring = emptystring.Replace('-', '\0');

            if (log.Id == emptystring)
                return CoreConstants.EmptyGuidLogIdResponse;

            var result = -1;

            using (var connection = GetConnection())
            {
                try
                {
                    using (var transaction = connection.OpenAndBeginTransaction())
                    {
                        try
                        {
                            var dictionary = new Dictionary<string, object>();
                            dictionary.Add("@Id", log.Id);
                            result =
                                 connection.Execute("DELETE FROM ErrorLog WHERE Id=@Id",
                                 transaction: transaction, inputParameters: dictionary);

                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                finally
                {
                    connection.CloseIfNot();
                }
            }

            return result;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets by id. </summary>
        ///
        /// <remarks>   Mustafa SAÇLI, 26.04.2019. </remarks>
        ///
        /// <param name="oid">  The oid. </param>
        ///
        /// <returns>   The by ıd. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public ErrorLogModel GetById(string oid)
        {
            var result = default(ErrorLogModel);

            if (string.IsNullOrWhiteSpace(oid)
                || oid == Guid.Empty.ToString()
                || oid == Guid.Empty.ToString().Replace('-', '\0'))
            {
                return result;
            }

            using (var connection = GetConnection())
            {
                try
                {
                    using (var transaction = connection.OpenAndBeginTransaction())
                    {
                        try
                        {
                            var dictionary = new Dictionary<string, object>();
                            dictionary.Add("@Id", oid);

                            result = connection.First<ErrorLogModel>("SELECT * FROM ErrorLog WHERE Id=@Id",
                            transaction: transaction, inputParameters: dictionary);

                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                finally
                {
                    connection.CloseIfNot();
                }
            }

            return result;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the logs in this collection. </summary>
        ///
        /// <remarks>   Mustafa SAÇLI, 26.04.2019. </remarks>
        ///
        /// <param name="startTimestamp">   The start timestamp. </param>
        /// <param name="endTimestamp">     The end timestamp. </param>
        ///
        /// <returns>
        /// An enumerator that allows foreach to be used to process the logs in this collection.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IEnumerable<ErrorLogModel> GetLogs(long? startTimestamp, long? endTimestamp)
        {
            var result = new ErrorLogModel[] { }
                .AsEnumerable();

            if (!startTimestamp.HasValue && !endTimestamp.HasValue)
            {
                return result;
            }

            var start = startTimestamp.GetValueOrDefault(0);
            var end = endTimestamp.GetValueOrDefault(0);

            using (var connection = GetConnection())
            {
                try
                {
                    using (var transaction = connection.OpenAndBeginTransaction())
                    {
                        try
                        {
                            var dictionary = new Dictionary<string, object>();

                            dictionary.Add("@start", start);
                            dictionary.Add("@end", end);

                            var list = connection.GetDynamicResultSet(
                                "SELECT * FROM ErrorLog WHERE (LogTimeUnixTimestamp >= @start OR @start = 0) AND (LogTimeUnixTimestamp <= @end OR @end = 0)",
                            transaction: transaction, inputParameters: dictionary);

                            result = list
                            .ConvertToList<ErrorLogModel>()
                            .AsEnumerable();

                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                finally
                {
                    connection.CloseIfNot();
                }
            }

            return result;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves the given log. </summary>
        ///
        /// <remarks>   Mustafa SAÇLI, 26.04.2019. </remarks>
        ///
        /// <param name="log">  The log to save. </param>
        ///
        /// <returns>   A string. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public string Save(ErrorLogModel log)
        {
            if (log == null)
            {
#if DEBUG
                log = new ErrorLogModel();
#else
            return "null";
#endif
            }

            var logId = log?.Id ?? string.Empty;
            var emptyGuid = Guid.Empty.ToString();

            if (string.IsNullOrWhiteSpace(logId)
                || logId == emptyGuid
                || logId == emptyGuid.Replace('-', '\0'))
            {
                log.Id = Guid.NewGuid().ToString();
            }

            if (!log.LogTime.HasValue)
            {
                log.LogTime = DateTime.Now;
                log.LogTimeUnixTimestamp = log.LogTime.Value.Ticks;
            }

            log.CreatedOn = DateTime.Now;
            log.CreatedOnUnixTimestamp = log.CreatedOn.Ticks;

            ///
            /// Insert Logic here.
            /// 

            throw new NotImplementedException();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Updates the given log. </summary>
        ///
        /// <remarks>   Mustafa SAÇLI, 26.04.2019. </remarks>
        ///
        /// <param name="log">  The log to save. </param>
        ///
        /// <returns>   An int. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public int Update(ErrorLogModel log)
        {
            if (log == null)
                return CoreConstants.NullLogResponse;

            if (string.IsNullOrWhiteSpace(log.Id))
                return CoreConstants.EmptyLogIdResponse;

            var emptystring = Guid.Empty.ToString();

            if (log.Id == emptystring)
                return CoreConstants.EmptyGuidLogIdResponse;

            emptystring = emptystring.Replace('-', '\0');

            if (log.Id == emptystring)
                return CoreConstants.EmptyGuidLogIdResponse;

            ///
            /// Update Logic here.
            /// 

            throw new NotImplementedException();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        ///
        /// <remarks>   Mustafa SAÇLI, 26.04.2019. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Dispose()
        {
        }
    }
}