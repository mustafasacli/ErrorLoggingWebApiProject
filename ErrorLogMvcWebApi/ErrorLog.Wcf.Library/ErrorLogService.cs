namespace ErrorLog.Wcf.Library
{
    using ErrorLog.Business.Core.Interfaces;
    using ErrorLog.Models;
    using SimpleFileLogging;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ErrorLogService : IErrorLogService
    {
        private IErrorLogBusiness logBusiness;

        public ErrorLogService()
        {
            this.logBusiness = WcfIoC.Instance.Container.GetInstance<IErrorLogBusiness>();
        }

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