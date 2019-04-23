namespace Mst.RavenDb.Core
{
    using Raven.Abstractions.Logging;
    using Raven.Client;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class RavenDbBaseRepository<TObject>
     : RavenDbDocumentStore,
         IDisposable
    {
        private static readonly ILog logger =
            LogManager.GetLogger(typeof(RavenDbBaseRepository<TObject>));

        private IDocumentSession session;

        protected RavenDbBaseRepository(string defaultDatabaseName, string dbServerUrl)
            : base(defaultDatabaseName, dbServerUrl)
        {
            if (string.IsNullOrWhiteSpace(defaultDatabaseName))
                throw new ArgumentNullException(nameof(defaultDatabaseName));

            if (string.IsNullOrWhiteSpace(dbServerUrl))
                throw new ArgumentNullException(nameof(dbServerUrl));
        }

        /// <summary>
        /// gets IDocumentSession
        /// </summary>
        public IDocumentSession Session
        {
            get
            {
                if (session == null)
                {
                    session = DocumentStore.OpenSession();
                }

                return session;
            }
        }

        public virtual IQueryable<TObject> GetDocuments()
        {
            return Session.Query<TObject>();
        }

        public virtual TObject GetDocument(string oid)
        {
            return Session.Load<TObject>(oid);
        }

        public virtual IQueryable<TObject> GetDocuments(IEnumerable<string> ids)
        {
            return Session.Load<TObject>(ids).AsQueryable();
        }

        public virtual void AddDocument(TObject entity, bool autoSave = true)
        {
            try
            {
                Session.Store(entity);

                if (autoSave)
                    SaveChanges();
            }
            catch (Exception ex)
            {
                logger.Error("Exception message : {0}", ex);
            }
        }

        public virtual void UpdateDocument(TObject entity, bool autoSave = true)
        {
            try
            {
                Session.Store(entity);

                if (autoSave)
                    SaveChanges();
            }
            catch (Exception ex)
            {
                logger.Error("Exception message : {0}", ex);
            }
        }

        public virtual void DeleteDocument(TObject entity, bool autoSave = true)
        {
            try
            {
                Session.Delete(entity);

                if (autoSave)
                    SaveChanges();
            }
            catch (Exception ex)
            {
                logger.Error("Exception message : {0}", ex);
            }
        }

        public virtual void DeleteDocument(string oid, bool autoSave = true)
        {
            try
            {
                TObject entity = GetDocument(oid);
                Session.Delete(entity);

                if (autoSave)
                    SaveChanges();
            }
            catch (Exception ex)
            {
                logger.Error("Exception message : {0}", ex);
            }
        }

        public void Dispose()
        {
            Session.SaveChanges();
            Session.Dispose();
        }

        public void SaveChanges()
        {
            Session.SaveChanges();
        }
    }
}