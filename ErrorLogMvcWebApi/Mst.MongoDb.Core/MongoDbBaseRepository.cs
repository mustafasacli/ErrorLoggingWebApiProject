namespace Mst.MongoDb.Core
{
    using MongoDB.Driver;
    using System;

    public abstract class MongoDbBaseRepository<T> : IDisposable where T : class
    {
        protected MongoDbBaseRepository(string databaseName, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(databaseName))
                throw new ArgumentNullException(nameof(databaseName));

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            var client = new MongoClient(connectionString);
            this.Collection = client.GetDatabase(databaseName).GetCollection<T>(typeof(T).Name.ToLowerInvariant());
        }

        public IMongoCollection<T> Collection
        { get; private set; }

        public void Dispose()
        {
            this.Collection = null;
        }
    }
}