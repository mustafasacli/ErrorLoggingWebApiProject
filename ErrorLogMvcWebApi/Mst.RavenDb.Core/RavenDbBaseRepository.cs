namespace Mst.RavenDb.Core
{
    using Raven.Abstractions.Logging;
    using Raven.Client;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A raven database repository. </summary>
    ///
    /// <remarks>   Msacli, 24.04.2019. </remarks>
    ///
    /// <typeparam name="TObject">  Type of the object. </typeparam>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public abstract class RavenDbBaseRepository<TObject>
     : RavenDbDocumentStore,
         IDisposable
    {
        private static readonly ILog logger =
            LogManager.GetLogger(typeof(RavenDbBaseRepository<TObject>));

        private IDocumentSession session;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Specialised constructor for use only by derived class. </summary>
        ///
        /// <remarks>   Msacli, 24.04.2019. </remarks>
        ///
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
        ///                                             null. </exception>
        ///
        /// <param name="defaultDatabaseName">  The default database name. </param>
        /// <param name="dbServerUrl">          URL of the database server. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
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

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the documents. </summary>
        ///
        /// <remarks>   Msacli, 24.04.2019. </remarks>
        ///
        /// <returns>   The documents. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual IQueryable<TObject> GetDocuments()
        {
            return Session.Query<TObject>();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets a document with given oid. </summary>
        ///
        /// <remarks>   Msacli, 24.04.2019. </remarks>
        ///
        /// <param name="oid">  The oid. </param>
        ///
        /// <returns>   The document. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual TObject GetDocument(string oid)
        {
            return Session.Load<TObject>(oid);
        }

        public virtual IQueryable<TObject> GetDocuments(IEnumerable<string> ids)
        {
            return Session.Load<TObject>(ids).AsQueryable();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Adds a document to 'autoSave'. </summary>
        ///
        /// <remarks>   Msacli, 24.04.2019. </remarks>
        ///
        /// <param name="entity">   The entity. </param>
        /// <param name="autoSave"> (Optional) True to automatically save. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
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

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Updates the document. </summary>
        ///
        /// <remarks>   Msacli, 24.04.2019. </remarks>
        ///
        /// <param name="entity">   The entity. </param>
        /// <param name="autoSave"> (Optional) True to automatically save. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
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

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deletes the document. </summary>
        ///
        /// <remarks>   Msacli, 24.04.2019. </remarks>
        ///
        /// <param name="entity">   The entity. </param>
        /// <param name="autoSave"> (Optional) True to automatically save. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
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

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deletes the document. </summary>
        ///
        /// <remarks>   Msacli, 24.04.2019. </remarks>
        ///
        /// <param name="oid">      The oid. </param>
        /// <param name="autoSave"> (Optional) True to automatically save. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
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

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        ///
        /// <remarks>   Msacli, 24.04.2019. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Dispose()
        {
            Session.SaveChanges();
            Session.Dispose();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves the changes. </summary>
        ///
        /// <remarks>   Msacli, 24.04.2019. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SaveChanges()
        {
            Session.SaveChanges();
        }
    }
}