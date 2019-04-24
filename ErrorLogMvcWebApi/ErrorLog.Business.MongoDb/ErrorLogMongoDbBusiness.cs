namespace ErrorLog.Business.MongoDb
{
    using Core.Constants;
    using ErrorLog.Business.Core.Interfaces;
    using ErrorLog.Models;
    using Mongo.Models;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An error log mongo database business. </summary>
    ///
    /// <remarks>   Msacli, 24.04.2019. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class ErrorLogMongoDbBusiness : ErrorLogRepository, IErrorLogBusiness
    {
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

            if (string.IsNullOrWhiteSpace(log.Id))
                return CoreConstants.EmptyLogIdResponse;

            var emptystring = Guid.Empty.ToString();

            if (log.Id == emptystring)
                return CoreConstants.EmptyGuidLogIdResponse;

            emptystring = emptystring.Replace('-', '\0');

            if (log.Id == emptystring)
                return CoreConstants.EmptyGuidLogIdResponse;

            var deleteResult = this.Collection.DeleteOne(q => q.Id == log.Id);

            return (int)deleteResult.DeletedCount;
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

            var logModel = this.Collection
                .Find(q => q.Id == oid)
                .ToList()
                .Select(q => new ErrorLogModel
                {
                    Id = q.Id,
                    ClassName = q.ClassName,
                    CreatedOn = q.CreatedOn,
                    CreatedOnTimestamp = q.CreatedOnTimestamp,
                    ExceptionData = q.ExceptionData,
                    LogTime = q.LogTime,
                    LogTimeUnixTimestamp = q.LogTimeUnixTimestamp,
                    Message = q.Message,
                    MethodName = q.MethodName,
                    RequestAddres = q.RequestAddres,
                    ResponseAddress = q.ResponseAddress,
                    ResponseMachineName = q.ResponseMachineName,
                    StackTrace = q.StackTrace,
                    UserId = q.UserId
                })
                .FirstOrDefault();

            return logModel;
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
                return new ErrorLogModel[] { }
                .AsEnumerable();
            }

            var start = startTimestamp.GetValueOrDefault(0);
            var end = endTimestamp.GetValueOrDefault(0);

            var docs = this.Collection
                .Find(q =>
            (q.LogTimeUnixTimestamp >= start || start == 0) && (q.LogTimeUnixTimestamp <= end || end == 0))
            .ToEnumerable()
            .Select(q => new ErrorLogModel
            {
                Id = q.Id,
                ClassName = q.ClassName,
                CreatedOn = q.CreatedOn,
                CreatedOnTimestamp = q.CreatedOnTimestamp,
                ExceptionData = q.ExceptionData,
                LogTime = q.LogTime,
                LogTimeUnixTimestamp = q.LogTimeUnixTimestamp,
                Message = q.Message,
                MethodName = q.MethodName,
                RequestAddres = q.RequestAddres,
                ResponseAddress = q.ResponseAddress,
                ResponseMachineName = q.ResponseMachineName,
                StackTrace = q.StackTrace,
                UserId = q.UserId
            });

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
            log.CreatedOnTimestamp = log.CreatedOn.Ticks;
            var logModel = new ErrorLogModelMongo
            {
                Id = log.Id,
                ClassName = log.ClassName,
                CreatedOn = log.CreatedOn,
                CreatedOnTimestamp = log.CreatedOnTimestamp,
                ExceptionData = log.ExceptionData,
                LogTime = log.LogTime,
                LogTimeUnixTimestamp = log.LogTimeUnixTimestamp,
                Message = log.Message,
                MethodName = log.MethodName,
                RequestAddres = log.RequestAddres,
                ResponseAddress = log.ResponseAddress,
                ResponseMachineName = log.ResponseMachineName,
                StackTrace = log.StackTrace,
                UserId = log.UserId
            };

            this.Collection.InsertOne(logModel);
            return log.Id;
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

            if (string.IsNullOrWhiteSpace(log.Id))
                return CoreConstants.EmptyLogIdResponse;

            var emptystring = Guid.Empty.ToString();

            if (log.Id == emptystring)
                return CoreConstants.EmptyGuidLogIdResponse;

            emptystring = emptystring.Replace('-', '\0');

            if (log.Id == emptystring)
                return CoreConstants.EmptyGuidLogIdResponse;

            var logModel = new ErrorLogModelMongo
            {
                Id = log.Id,
                ClassName = log.ClassName,
                CreatedOn = log.CreatedOn,
                CreatedOnTimestamp = log.CreatedOnTimestamp,
                ExceptionData = log.ExceptionData,
                LogTime = log.LogTime,
                LogTimeUnixTimestamp = log.LogTimeUnixTimestamp,
                Message = log.Message,
                MethodName = log.MethodName,
                RequestAddres = log.RequestAddres,
                ResponseAddress = log.ResponseAddress,
                ResponseMachineName = log.ResponseMachineName,
                StackTrace = log.StackTrace,
                UserId = log.UserId
            };

            var filter = Builders<ErrorLogModelMongo>.Filter.Eq(s => s.Id, logModel.Id);
            var replaceOneResult = this.Collection.ReplaceOne(filter, logModel);

            return (int)replaceOneResult.ModifiedCount;
        }
    }
}