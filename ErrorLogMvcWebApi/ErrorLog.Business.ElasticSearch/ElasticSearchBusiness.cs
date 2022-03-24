using ErrorLog.Business.Core.Interfaces;
using ErrorLog.Business.ElasticSearch.Core;
using ErrorLog.Models;
using Nest;
using System.Collections.Generic;
using System.Linq;

namespace ErrorLog.Business.ElasticSearch
{
    /// <summary>
    /// ElasticSearchBusiness instance.
    /// </summary>
    public class ElasticSearchBusiness : BaseESBusiness<ErrorLogModel>, IErrorLogBusiness
    {
        /// <summary>
        /// ElasticSearchBusiness ctor.
        /// </summary>
        public ElasticSearchBusiness() :
            base("http://localhost:9200", "error_log")// "change_log")
        {
        }

        /// <summary>
        /// Deletes log model.
        /// </summary>
        /// <param name="log">log model.</param>
        /// <returns>returns delete result.</returns>
        public int Delete(ErrorLogModel log)
        {
            int result = base.Delete(log.Id);
            return result;
        }

        /// <summary>
        /// Disposes  elastic search client.
        /// </summary>
        public void Dispose()
        {
            this.Client = null;
        }

        /// <summary>
        /// Gets ErrorLogModel model by id.
        /// </summary>
        /// <param name="oid">error log oid</param>
        /// <returns>ErrorLogModel instance</returns>
        public ErrorLogModel GetById(string oid)
        {
            ErrorLogModel result = GetById(oid);
            return result;
        }

        /// <summary>
        /// Gets error logs between given timestamp values.
        /// </summary>
        /// <param name="startTimestamp">The start timestamp. </param>
        /// <param name="endTimestamp">The end timestamp. </param>
        /// <returns>Returns enumarble ErrorLogModel objects.</returns>
        public IEnumerable<ErrorLogModel> GetLogs(long? startTimestamp, long? endTimestamp)
        {
            long start = startTimestamp.GetValueOrDefault();
            long end = endTimestamp.GetValueOrDefault(long.MaxValue);

            // TODO : WILL BE TESTED.
            ISearchResponse<ErrorLogModel> searchResponse =
            this.Client.Search<ErrorLogModel>(s =>
            s.Query(q =>
            q.Range(r => r.Field(f => f.LogTimeUnixTimestamp).GreaterThanOrEquals(start))
            && q.Range(r => r.Field(f => f.LogTimeUnixTimestamp).LessThanOrEquals(end))
            ));
            IEnumerable<ErrorLogModel> results = searchResponse.Documents.AsEnumerable();
            return results;
        }

        /// <summary>
        /// saves error log model.
        /// </summary>
        /// <param name="log">log model.</param>
        /// <returns>Returns result as string.</returns>
        public string Save(ErrorLogModel log)
        {
            string result = CheckExistsAndInsert(log);
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="log"></param>
        /// <returns>returns -100 value.</returns>
        public int Update(ErrorLogModel log)
        {
            return -100;
        }
    }
}