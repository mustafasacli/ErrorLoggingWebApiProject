namespace ErrorLog.Business.Core.Interfaces
{
    using ErrorLog.Models;
    using System.Collections.Generic;

    public interface IErrorLogBusiness
    {
        string Save(ErrorLogModel log);

        int Update(ErrorLogModel log);

        int Delete(ErrorLogModel log);

        ErrorLogModel GetById(string oid);

        IEnumerable<ErrorLogModel> GetLogs(long? startTimestamp, long? endTimestamp);
    }
}