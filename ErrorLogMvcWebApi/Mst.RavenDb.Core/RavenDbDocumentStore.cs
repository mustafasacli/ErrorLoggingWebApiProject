namespace Mst.RavenDb.Core
{
    using Raven.Client;
    using Raven.Client.Document;
    using System;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A raven database document store. </summary>
    ///
    /// <remarks>   Msacli, 24.04.2019. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class RavenDbDocumentStore
    {
        private static object lockObj = new object();
        private IDocumentStore docStore = null;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Msacli, 24.04.2019. </remarks>
        ///
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        ///
        /// <param name="defaultDatabaseName">  Gets Default Database Name. </param>
        /// <param name="dbServerUrl">          Gets Db Server Url. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
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

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the document store. </summary>
        ///
        /// <value> The document store. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
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