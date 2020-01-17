using ErrorLog.Business.Core.Interfaces;
using ErrorLog.Business.ElasticSearch.Core;
using ErrorLog.Models;
using System.Collections.Generic;
using System.Linq;

namespace ErrorLog.Business.ElasticSearch
{
    public class ElasticSearchBusiness : BaseESBusiness<ErrorLogModel>, IErrorLogBusiness
    {
        public ElasticSearchBusiness() :
            base("http://localhost:9200", "change_log")
        {
        }

        public int Delete(ErrorLogModel log)
        {
            var r = base.Delete(log.Id);
            return r;
        }

        public void Dispose()
        {
            this.Client = null;
        }

        public ErrorLogModel GetById(string oid)
        {
            var result = GetById(oid);
            return result;
        }

        public IEnumerable<ErrorLogModel> GetLogs(long? startTimestamp, long? endTimestamp)
        {
            var start = startTimestamp.GetValueOrDefault();
            var end = endTimestamp.GetValueOrDefault();

            /// TODO : WILL BE TESTED.
            var rs =
            this.Client.Search<ErrorLogModel>(s =>
            s.Query(q =>
            q.Range(r => r.Field(f => f.LogTimeUnixTimestamp).GreaterThanOrEquals(start))
            && q.Range(r => r.Field(f => f.LogTimeUnixTimestamp).LessThanOrEquals(end))
            ));
            var rx = rs.Documents.AsEnumerable();
            return rx;
        }

        public string Save(ErrorLogModel log)
        {
            var result = CheckExistsAndInsert(log);
            return result;

        }

        public int Update(ErrorLogModel log)
        {
            return -100;
        }
    }
}
