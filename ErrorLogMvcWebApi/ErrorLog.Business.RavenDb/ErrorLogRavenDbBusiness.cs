namespace ErrorLog.Business.RavenDb
{
    using Core.Constants;
    using ErrorLog.Business.Core.Interfaces;
    using ErrorLog.Models;
    using Mst.RavenDb.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An error log raven database business. </summary>
    ///
    /// <remarks>   Msacli, 24.04.2019. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class ErrorLogRavenDbBusiness : RavenDbBaseRepository<ErrorLogModel>, IErrorLogBusiness
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Msacli, 24.04.2019. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public ErrorLogRavenDbBusiness()
            : base(EnvironmentConstants.DbName, EnvironmentConstants.DbServerUrl)
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deletes the given log. </summary>
        ///
        /// <remarks>   Msacli, 24.04.2019. </remarks>
        ///
        /// <param name="log">  The log to save. </param>
        ///
        /// <returns>   An int. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public int Delete(ErrorLogModel log)
        {
            if (log == null)
                return CoreConstants.NullLogResponse;

            if (string.IsNullOrWhiteSpace(log.LogId))
                return CoreConstants.EmptyLogIdResponse;

            var emptystring = Guid.Empty.ToString();

            if (log.LogId == emptystring)
                return CoreConstants.EmptyGuidLogIdResponse;

            emptystring = emptystring.Replace('-', '\0');

            if (log.LogId == emptystring)
                return CoreConstants.EmptyGuidLogIdResponse;

            DeleteDocument(log);
            return 1;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets by id. </summary>
        ///
        /// <remarks>   Msacli, 24.04.2019. </remarks>
        ///
        /// <param name="oid">  The oid. </param>
        ///
        /// <returns>   The by ıd. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public ErrorLogModel GetById(string oid)
        {
            if (string.IsNullOrWhiteSpace(oid)
                || oid == Guid.Empty.ToString()
                || oid == Guid.Empty.ToString().Replace('-', '\0'))
            {
                return null;
            }

            var result = GetDocument(oid);
            return result;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the logs in this collection. </summary>
        ///
        /// <remarks>   Msacli, 24.04.2019. </remarks>
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
            if (!startTimestamp.HasValue && !endTimestamp.HasValue)
            {
                return new ErrorLogModel[] { }.AsEnumerable();
            }

            var start = startTimestamp.GetValueOrDefault(0);
            var end = endTimestamp.GetValueOrDefault(0);

            var docs = GetDocuments()
                .Where(q =>
            (q.LogTimeUnixTimestamp >= start || start == 0) && (q.LogTimeUnixTimestamp <= end || end == 0))
            .AsEnumerable() ?? new ErrorLogModel[] { }.AsEnumerable();

            return docs;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves the given log. </summary>
        ///
        /// <remarks>   Msacli, 24.04.2019. </remarks>
        ///
        /// <param name="log">  The log to save. </param>
        ///
        /// <returns>   A string. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public string Save(ErrorLogModel log)
        {
            if (string.IsNullOrWhiteSpace(log.LogId)
                || log.LogId == Guid.Empty.ToString()
                || log.LogId == Guid.Empty.ToString().Replace('-', '\0'))
            {
                log.LogId = Guid.NewGuid().ToString();
            }

            if (!log.LogTime.HasValue)
            {
                log.LogTime = DateTime.Now;
                log.LogTimeUnixTimestamp = log.LogTime.Value.Ticks;
            }

            log.CreatedOn = DateTime.Now;
            log.CreatedOnTimestamp = log.CreatedOn.Ticks;
            AddDocument(log);

            return log.LogId;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Updates the given log. </summary>
        ///
        /// <remarks>   Msacli, 24.04.2019. </remarks>
        ///
        /// <param name="log">  The log to save. </param>
        ///
        /// <returns>   An int. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public int Update(ErrorLogModel log)
        {
            if (log == null)
                return CoreConstants.NullLogResponse;

            if (string.IsNullOrWhiteSpace(log.LogId))
                return CoreConstants.EmptyLogIdResponse;

            var emptystring = Guid.Empty.ToString();

            if (log.LogId == emptystring)
                return CoreConstants.EmptyGuidLogIdResponse;

            emptystring = emptystring.Replace('-', '\0');

            if (log.LogId == emptystring)
                return CoreConstants.EmptyGuidLogIdResponse;

            UpdateDocument(log);
            return 1;
        }
    }
}