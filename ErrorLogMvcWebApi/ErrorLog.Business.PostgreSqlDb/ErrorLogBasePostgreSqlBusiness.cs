////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	ErrorLogBasePostgreSqlBusiness.cs
//
// summary:	Implements the error log base PostgreSQL business class
////////////////////////////////////////////////////////////////////////////////////////////////////

namespace ErrorLog.Business.PostgreSqlDb
{
    using Core.Constants;
    using ErrorLog.Business.Core.Interfaces;
    using ErrorLog.Business.Sql.Core;
    using ErrorLog.Models;
    using Mst.Dexter.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An error log base PostgreSQL business. </summary>
    ///
    /// <remarks>   Mustafa SAÇLI, 9.05.2019. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class ErrorLogBasePostgreSqlBusiness : BaseSqlBusiness, IErrorLogBusiness
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Mustafa SAÇLI, 9.05.2019. </remarks>
        ///
        /// <param name="connectionName">   Name of the connection. </param>
        /// <param name="connectionString"> The connection string. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public ErrorLogBasePostgreSqlBusiness(string connectionName, string connectionString) :
            base(connectionName, connectionString)
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
        public virtual int Delete(ErrorLogModel log)
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
                            dictionary.Add(":Id", log.Id);
                            result =
                                 connection.Execute("DELETE FROM ErrorLog WHERE Id = :Id",
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
        public virtual ErrorLogModel GetById(string oid)
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
                            dictionary.Add(":Id", oid);

                            result = connection.First<ErrorLogModel>("SELECT * FROM ErrorLog WHERE Id=:Id",
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
        public virtual IEnumerable<ErrorLogModel> GetLogs(long? startTimestamp, long? endTimestamp)
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

                            dictionary.Add(":start", start);
                            dictionary.Add(":end", end);

                            var list = connection.GetDynamicResultSet(
                                "SELECT * FROM ErrorLog WHERE (LogTimeUnixTimestamp >= :start OR :start = 0) AND (LogTimeUnixTimestamp <= :end OR :end = 0)",
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
        public virtual string Save(ErrorLogModel log)
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

            var result = string.Empty;

            using (var connection = GetConnection())
            {
                try
                {
                    using (var transaction = connection.OpenAndBeginTransaction())
                    {
                        try
                        {
                            var dictionary = new Dictionary<string, object>();
                            dictionary.Add(":Id", log.Id);
                            dictionary.Add(":RequestAddres", log.RequestAddres);
                            dictionary.Add(":ResponseAddress", log.ResponseAddress);
                            dictionary.Add(":ResponseMachineName", log.ResponseMachineName);
                            dictionary.Add(":UserId", log.UserId);
                            dictionary.Add(":ClassName", log.ClassName);
                            dictionary.Add(":MethodName", log.MethodName);
                            dictionary.Add(":Message", log.Message);
                            dictionary.Add(":StackTrace", log.StackTrace);
                            dictionary.Add(":ExceptionData", log.ExceptionData);
                            dictionary.Add(":LogTime", log.LogTime);
                            dictionary.Add(":LogTimeUnixTimestamp", log.LogTimeUnixTimestamp);
                            dictionary.Add(":CreatedOn", log.CreatedOn);
                            dictionary.Add(":CreatedOnUnixTimestamp", log.CreatedOnUnixTimestamp);

                            connection.Execute(
                                @"INSERT INTO ErrorLog
                                        (Id, RequestAddres, ResponseAddress, ResponseMachineName, UserId, ClassName, MethodName, Message, StackTrace, ExceptionData, LogTime, LogTimeUnixTimestamp, CreatedOn, CreatedOnUnixTimestamp)
                                        VALUES
                                        (:Id, :RequestAddres, :ResponseAddress, :ResponseMachineName, :UserId, :ClassName, :MethodName, :Message, :StackTrace, :ExceptionData, :LogTime, :LogTimeUnixTimestamp, :CreatedOn, :CreatedOnUnixTimestamp)",
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
            result = log.Id;
            return result;
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
        public virtual int Update(ErrorLogModel log)
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
                            dictionary.Add(":RequestAddres", log.RequestAddres);
                            dictionary.Add(":ResponseAddress", log.ResponseAddress);
                            dictionary.Add(":ResponseMachineName", log.ResponseMachineName);
                            dictionary.Add(":UserId", log.UserId);
                            dictionary.Add(":ClassName", log.ClassName);
                            dictionary.Add(":MethodName", log.MethodName);
                            dictionary.Add(":Message", log.Message);
                            dictionary.Add(":StackTrace", log.StackTrace);
                            dictionary.Add(":ExceptionData", log.ExceptionData);
                            dictionary.Add(":LogTime", log.LogTime);
                            dictionary.Add(":LogTimeUnixTimestamp", log.LogTimeUnixTimestamp);
                            dictionary.Add(":CreatedOn", log.CreatedOn);
                            dictionary.Add(":CreatedOnUnixTimestamp", log.CreatedOnUnixTimestamp);
                            dictionary.Add(":Id", log.Id);

                            result =
                                 connection.Execute(
                                     @"UPDATE ErrorLog SET
                                        RequestAddres = :RequestAddres,
                                        ResponseAddress = :ResponseAddress,
                                        ResponseMachineName = :ResponseMachineName,
                                        UserId = :UserId,
                                        ClassName = :ClassName,
                                        MethodName = :MethodName,
                                        Message = :Message,
                                        StackTrace = :StackTrace,
                                        ExceptionData = :ExceptionData,
                                        LogTime = :LogTime,
                                        LogTimeUnixTimestamp = :LogTimeUnixTimestamp,
                                        CreatedOn = :CreatedOn,
                                        CreatedOnUnixTimestamp = :CreatedOnUnixTimestamp
                                        WHERE Id = :Id",
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