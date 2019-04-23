namespace ErrorLog.Business.RavenDb
{
    using ErrorLog.Business.Core.Interfaces;
    using ErrorLog.Models;
    using Mst.RavenDb.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ErrorLogRavenDbBusiness : RavenDbBaseRepository<ErrorLogModel>, IErrorLogBusiness
    {
        public ErrorLogRavenDbBusiness()
            : base(EnvironmentConstants.DbName, EnvironmentConstants.DbServerUrl)
        {
        }

        public int Delete(ErrorLogModel log)
        {
            DeleteDocument(log);
            return 1;
        }

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

        public int Update(ErrorLogModel log)
        {
            UpdateDocument(log);
            return 1;
        }
    }
}