namespace ErrorLog.Business.MongoDb
{
    using ErrorLog.Business.Core.Interfaces;
    using ErrorLog.Models;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ErrorLogMongoDbBusiness : ErrorLogRepository, IErrorLogBusiness
    {
        public int Delete(ErrorLogModel log)
        {
            if (string.IsNullOrWhiteSpace(log.LogId))
                return -1;
            var emptystring = Guid.Empty.ToString();

            if (log.LogId == emptystring)
                return -2;

            emptystring = emptystring.Replace('-', '\0');

            if (log.LogId == emptystring)
                return -3;

            var deleteResult = this.Collection.DeleteOne(q => q.LogId == log.LogId);

            return (int)deleteResult.DeletedCount;
        }

        public ErrorLogModel GetById(string oid)
        {
            if (string.IsNullOrWhiteSpace(oid)
                || oid == Guid.Empty.ToString()
                || oid == Guid.Empty.ToString().Replace('-', '\0'))
            {
                return null;
            }

            var logModel = this.Collection
                .Find(q => q.LogId == oid)
                .FirstOrDefault();

            return logModel;
        }

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
            .ToEnumerable() ?? new ErrorLogModel[] { }.AsEnumerable();

            return docs;
        }

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
            this.Collection.InsertOne(log);
            return log.LogId;
        }

        public int Update(ErrorLogModel log)
        {
            if (string.IsNullOrWhiteSpace(log.LogId))
                return -1;
            var emptystring = Guid.Empty.ToString();

            if (log.LogId == emptystring)
                return -2;

            emptystring = emptystring.Replace('-', '\0');

            if (log.LogId == emptystring)
                return -3;

            var filter = Builders<ErrorLogModel>.Filter.Eq(s => s.LogId, log.LogId);
            var replaceOneResult = this.Collection.ReplaceOne(filter, log);

            return (int)replaceOneResult.ModifiedCount;
        }
    }
}