namespace ErrorLog.Business.GenericRepository
{
    using ErrorLog.Business.Core.Constants;
    using ErrorLog.Business.Core.Interfaces;
    using ErrorLog.Entity;
    using ErrorLog.Models;
    using ErrorLog.Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    public class ErrorLogGenericBusiness : IErrorLogBusiness
    {
        /// <summary>
        /// 
        /// </summary>
        public ErrorLogGenericBusiness()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
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

            using (var repo = new ErrorLogRepository<ErrorLogEntity>())
            {
                var entity = repo.FirstOrDefault(q => q.Id == log.Id, asNoTracking: true);
                if (entity != null && entity != default(ErrorLogEntity))
                {
                    repo.Delete(entity);
                    repo.SaveChanges();
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public ErrorLogModel GetById(string oid)
        {
            var result = default(ErrorLogModel);

            if (string.IsNullOrWhiteSpace(oid)
                || oid == Guid.Empty.ToString()
                || oid == Guid.Empty.ToString().Replace('-', '\0'))
            {
                return result;
            }

            using (var repo = new ErrorLogRepository<ErrorLogEntity>())
            {
                result = repo
                    .GetAll(q => q.Id == oid, asNoTracking: true)
                    .Select(q => new ErrorLogModel
                    {
                        ClassName = q.ClassName,
                        CreatedOn = q.CreatedOn,
                        CreatedOnUnixTimestamp = q.CreatedOnUnixTimestamp,
                        ExceptionData = q.ExceptionData,
                        Id = q.Id,
                        LogTime = q.LogTime,
                        LogTimeUnixTimestamp = q.LogTimeUnixTimestamp,
                        Message = q.Message,
                        MethodName = q.MethodName,
                        RequestAddres = q.RequestAddres,
                        ResponseAddress = q.ResponseAddress,
                        ResponseMachineName = q.ResponseMachineName,
                        StackTrace = q.StackTrace,
                        UserId = q.UserId
                    }).FirstOrDefault();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startTimestamp"></param>
        /// <param name="endTimestamp"></param>
        /// <returns></returns>
        public IEnumerable<ErrorLogModel> GetLogs(long? startTimestamp, long? endTimestamp)
        {
            var results = (new ErrorLogModel[] { }).AsEnumerable();

            var start = startTimestamp.GetValueOrDefault();
            var end = endTimestamp.GetValueOrDefault();

            using (var repo = new ErrorLogRepository<ErrorLogEntity>())
            {
                results = repo
                    .GetAll(q =>
            ((q.LogTimeUnixTimestamp >= start || start == 0) && (q.LogTimeUnixTimestamp <= end || end == 0)), asNoTracking: true)
                    .Select(q => new ErrorLogModel
                    {
                        ClassName = q.ClassName,
                        CreatedOn = q.CreatedOn,
                        CreatedOnUnixTimestamp = q.CreatedOnUnixTimestamp,
                        ExceptionData = q.ExceptionData,
                        Id = q.Id,
                        LogTime = q.LogTime,
                        LogTimeUnixTimestamp = q.LogTimeUnixTimestamp,
                        Message = q.Message,
                        MethodName = q.MethodName,
                        RequestAddres = q.RequestAddres,
                        ResponseAddress = q.ResponseAddress,
                        ResponseMachineName = q.ResponseMachineName,
                        StackTrace = q.StackTrace,
                        UserId = q.UserId
                    }).AsEnumerable();
            }

            return results;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
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
            var entity = new ErrorLogEntity
            {
                Id = log.Id,
                RequestAddres = log.RequestAddres,
                ResponseAddress = log.ResponseAddress,
                ResponseMachineName = log.ResponseMachineName,
                UserId = log.UserId,
                ClassName = log.ClassName,
                MethodName = log.MethodName,
                Message = log.Message,
                StackTrace = log.StackTrace,
                ExceptionData = log.ExceptionData,
                LogTime = log.LogTime,
                LogTimeUnixTimestamp = log.LogTimeUnixTimestamp,
                CreatedOn = log.CreatedOn,
                CreatedOnUnixTimestamp = log.CreatedOnUnixTimestamp
            };

            using (var repo = new ErrorLogRepository<ErrorLogEntity>())
            {
                repo.Add(entity);
                repo.SaveChanges();
            }

            return entity.Id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
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

            var result = -1;

            var entity = default(ErrorLogEntity);

            using (var repo = new ErrorLogRepository<ErrorLogEntity>())
            {
                entity = repo.FirstOrDefault(q => q.Id == log.Id);
                if (entity != null && entity != default(ErrorLogEntity))
                {
                    entity = new ErrorLogEntity
                    {
                        Id = log.Id,
                        RequestAddres = log.RequestAddres,
                        ResponseAddress = log.ResponseAddress,
                        ResponseMachineName = log.ResponseMachineName,
                        UserId = log.UserId,
                        ClassName = log.ClassName,
                        MethodName = log.MethodName,
                        Message = log.Message,
                        StackTrace = log.StackTrace,
                        ExceptionData = log.ExceptionData,
                        LogTime = log.LogTime,
                        LogTimeUnixTimestamp = log.LogTimeUnixTimestamp,
                        CreatedOn = log.CreatedOn,
                        CreatedOnUnixTimestamp = log.CreatedOnUnixTimestamp
                    };
                    repo.Update(entity);
                    result = repo.SaveChanges();
                }
            }

            return result;
        }
    }
}