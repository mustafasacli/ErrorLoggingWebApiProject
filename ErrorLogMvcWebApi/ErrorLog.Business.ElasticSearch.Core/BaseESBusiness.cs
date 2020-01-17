namespace ErrorLog.Business.ElasticSearch.Core
{
    using Nest;
    using System;

    public abstract class BaseESBusiness<T> where T : class, new()
    {
        protected ElasticClient Client { get; set; }

        public string Uri
        { get; protected set; }

        public string IndexName
        { get; protected set; }



        protected BaseESBusiness(string uri, string indexName)
        {
            if (string.IsNullOrWhiteSpace(uri))
                throw new ArgumentNullException(nameof(uri));

            if (string.IsNullOrWhiteSpace(indexName))
                throw new ArgumentNullException(nameof(indexName));

            this.Uri = uri;
            this.IndexName = indexName;
            var node = new Uri(this.Uri);
            var settings = new ConnectionSettings(node);
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
            /// TODO : Elastic Search Result nesnesi için uygun geri dönüş modeli, oluşturulacak.
            var response = this.Client.Index(log, idx => idx.Index(this.IndexName));
            var result = response.Id;
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
            var model = new T();

            if (!string.IsNullOrWhiteSpace(oid))
            {
                var response = this.Client.Get<T>(oid, idx => idx.Index(this.IndexName));
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
