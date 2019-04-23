namespace Mst.RavenDb.Core
{
    using Raven.Client;
    using Raven.Client.Document;
    using System;

    public class RavenDbDocumentStore
    {
        private static object lockObj = new object();
        private IDocumentStore docStore = null;

        public RavenDbDocumentStore(string defaultDatabaseName, string dbServerUrl)
        {
            if (string.IsNullOrWhiteSpace(defaultDatabaseName))
                throw new ArgumentNullException(nameof(defaultDatabaseName));

            if (string.IsNullOrWhiteSpace(dbServerUrl))
                throw new ArgumentNullException(nameof(dbServerUrl));

            this.DefaultDatabaseName = defaultDatabaseName;
            this.DbServerUrl = dbServerUrl;
        }

        /// <summary>
        /// gets Db Server Url
        /// </summary>
        public string DbServerUrl
        { get; private set; }

        /// <summary>
        /// gets Default Database Name
        /// </summary>
        public string DefaultDatabaseName
        { get; private set; }

        public IDocumentStore DocumentStore
        {
            get
            {
                if (docStore == null)
                {
                    lock (lockObj)
                    {
                        if (docStore == null)
                        {
                            var docStoreInstance = new DocumentStore();
                            docStoreInstance.Url = this.DbServerUrl;
                            docStoreInstance.DefaultDatabase = this.DefaultDatabaseName;

                            docStoreInstance.Initialize();
                            docStore = docStoreInstance;
                        }
                    }
                }

                return docStore;
            }
        }
    }
}