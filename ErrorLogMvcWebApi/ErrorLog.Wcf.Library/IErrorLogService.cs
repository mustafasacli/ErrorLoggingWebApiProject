namespace ErrorLog.Wcf.Library
{
    using ErrorLog.Models;
    using System.Collections.Generic;
    using System.ServiceModel;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Interface for error log service. </summary>
    ///
    /// <remarks>   Mustafa SAÇLI, 9.05.2019. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    [ServiceContract(Namespace = "http://127.0.0.1:8081/ErrorLogService")]
    public interface IErrorLogService
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves the given error log. </summary>
        ///
        /// <param name="errorLog"> The error log to save. </param>
        ///
        /// <returns>   A string. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [OperationContract]
        string Save(ErrorLogModel errorLog);

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets an error log model using the given oid. </summary>
        ///
        /// <param name="oid">  The oid to get. </param>
        ///
        /// <returns>   An ErrorLogModel. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [OperationContract]
        ErrorLogModel Get(string oid);

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Enumerates the items in this collection that meet given criteria. </summary>
        ///
        /// <param name="startTimestamp">   The start timestamp. </param>
        /// <param name="endTimestamp">     The end timestamp. </param>
        ///
        /// <returns>   An enumerator that allows foreach to be used to process the matched items. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [OperationContract]
        IEnumerable<ErrorLogModel> Search(long? startTimestamp, long? endTimestamp);
    }
}