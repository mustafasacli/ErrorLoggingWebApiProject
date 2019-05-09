////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	ErrorLogService.cs
//
// summary:	Implements the error log service class
////////////////////////////////////////////////////////////////////////////////////////////////////
namespace ErrorLog.Wcf.Library
{
    using ErrorLog.Business.Core.Interfaces;
    using ErrorLog.Models;
    using IoC.Library;
    using SimpleFileLogging;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A service for accessing error logs information. </summary>
    ///
    /// <remarks>   Mustafa SAÇLI, 9.05.2019. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class ErrorLogService : IErrorLogService
    {
        /// <summary>   The log business. </summary>
        private IErrorLogBusiness logBusiness;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Mustafa SAÇLI, 9.05.2019. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public ErrorLogService()
        {
            this.logBusiness = ErrorLogIoC.Instance.Container.GetInstance<IErrorLogBusiness>();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets an error log model using the given oid. </summary>
        ///
        /// <remarks>   Mustafa SAÇLI, 9.05.2019. </remarks>
        ///
        /// <param name="oid">  The oid to get. </param>
        ///
        /// <returns>   An ErrorLogModel. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public ErrorLogModel Get(string oid)
        {
            var result = new ErrorLogModel { };

            try
            {
                result = logBusiness.GetById(oid);
            }
            catch (Exception e)
            {
                SimpleFileLogger.Instance.LogError(e);
            }

            result = result ?? new ErrorLogModel { };
            return result;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves the given error log. </summary>
        ///
        /// <remarks>   Mustafa SAÇLI, 9.05.2019. </remarks>
        ///
        /// <param name="errorLog"> The error log to save. </param>
        ///
        /// <returns>   A string. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public string Save(ErrorLogModel errorLog)
        {
            var result = string.Empty;

            try
            {
                result = logBusiness.Save(errorLog);
            }
            catch (Exception e)
            {
                SimpleFileLogger.Instance.LogError(e);
                result = "err";
            }

            return result;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Enumerates the items in this collection that meet given criteria. </summary>
        ///
        /// <remarks>   Mustafa SAÇLI, 9.05.2019. </remarks>
        ///
        /// <param name="startTimestamp">   The start timestamp. </param>
        /// <param name="endTimestamp">     The end timestamp. </param>
        ///
        /// <returns>   An enumerator that allows foreach to be used to process the matched items. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IEnumerable<ErrorLogModel> Search(long? startTimestamp, long? endTimestamp)
        {
            var result = new ErrorLogModel[] { }.AsEnumerable();

            try
            {
                result = logBusiness.GetLogs(startTimestamp, endTimestamp);
            }
            catch (Exception e)
            {
                SimpleFileLogger.Instance.LogError(e);
            }

            result = result ?? new ErrorLogModel[] { }.AsEnumerable();

            return result;
        }
    }
}