using Nest;
using System;

namespace ErrorLog.Business.ElasticSearch.Core
{
    /// <summary>
    /// Base Elastic Search Business.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseESBusiness<T> where T : class, new()
    {
        /// <summary>
        /// Gets, sets ElasticClient.
        /// </summary>
        protected ElasticClient Client { get; set; }

        /// <summary>
        /// Gets uri.
        /// </summary>
        public string Uri
        { get; protected set; }

        /// <summary>
        /// Gets index name.
        /// </summary>
        public string IndexName
        { get; protected set; }

        /// <summary>
        /// protected base elastic search business ctor.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="indexName"></param>
        protected BaseESBusiness(string uri, string indexName)
        {
            if (string.IsNullOrWhiteSpace(uri))
                throw new ArgumentNullException(nameof(uri));

            if (string.IsNullOrWhiteSpace(indexName))
                throw new ArgumentNullException(nameof(indexName));

            this.Uri = uri;
            this.IndexName = indexName;
            Uri node = new Uri(this.Uri);
            ConnectionSettings settings = new ConnectionSettings(node);
            this.Client = new ElasticClient(settings);
        }

        /// <summary>
        /// Checks Index, if not exist create.
        /// </summary>
        protected virtual void CheckIndex()
        {
            if (!this.Client.Indices.Exists(this.IndexName).Exists)
            {
                this.Client.Indices.Create(this.IndexName, idx => idx.Index(this.IndexName).Map<T>(q => q.AutoMap()));
            }
        }

        /// <summary>
        /// Insert and return id of model.
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public string CheckExistsAndInsert(T log)
        {
            CheckIndex();
            // TODO : Elastic Search Result nesnesi için uygun geri dönüş modeli, oluşturulacak.
            IndexResponse response = this.Client.Index(log, idx => idx.Index(this.IndexName));
            string result = response.Id;
            return result;
        }

        /// <summary>
        /// Gets model by with given id.
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public T Get(string oid)
        {
            CheckIndex();
            T model = new T();

            if (!string.IsNullOrWhiteSpace(oid))
            {
                GetResponse<T> response = this.Client.Get<T>(oid, idx => idx.Index(this.IndexName));
                model = response.Source;
            }

            return model;
        }

        /// <summary>
        /// Error = 0, Created = 1, Updated = 2, Deleted = 3, NotFound = 4, Noop = 5
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public int Delete(string oid)
        {
            CheckIndex();
            var result = -10;

            if (!string.IsNullOrWhiteSpace(oid))
            {
                var response = this.Client.Delete<T>(oid, idx => idx.Index(this.IndexName));
                result = (int)response.Result;
            }

            return result;
        }
    }
}