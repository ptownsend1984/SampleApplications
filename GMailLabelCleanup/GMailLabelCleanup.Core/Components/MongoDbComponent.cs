using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MongoDB.Driver;
using System.Configuration;
using System.Threading.Tasks;
using GMailLabelCleanup.Data.Documents;
using GMailLabelCleanup.Core.Exceptions;
using MongoDB.Bson;
using GMailLabelCleanup.Data.Documents.Filters;

namespace GMailLabelCleanup.Core.Components
{
    public class MongoDbComponent
    {

        #region Methods

        public MongoCollection<MessageFilter> MessageFilters
        {
            get { return GetCollection<MessageFilter>("MessageFilters"); }
        }

        public MongoCollection<T> GetCollection<T>(string collectionName)
        {
            var database = GetDatabase();
            return database.GetCollection<T>(collectionName);
        }

        private MongoClient GetClient()
        {
            return new MongoClient(ConfigurationManager.AppSettings["MongoUri"]);
        }
        private MongoServer GetServer(MongoClient client)
        {
            return client.GetServer();
        }
        private MongoDatabase GetDatabase()
        {
            return GetDatabase(ConfigurationManager.AppSettings["MongoDatabase"]);
        }
        private MongoDatabase GetDatabase(string databaseName)
        {
            var client = GetClient();
            var server = GetServer(client);
            return GetDatabase(server, databaseName);
        }
        private MongoDatabase GetDatabase(MongoServer server, string databaseName)
        {
            return server.GetDatabase(databaseName);
        }

        #endregion

    }

    public static class MongoDbExtensions
    {

        public static async Task<IEnumerable<T>> FindByUserIdAsync<T>(this MongoCollection<T> collection, string userId)
            where T : Document
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (userId == null)
                throw new ArgumentNullException("userId");

            return await Task.Run(() =>
            {
                var @params = new Dictionary<string, object>()
                {
                    { "UserId", userId },
                    { "EnvironmentMode", ConfigurationManager.AppSettings["EnvironmentMode"] }
                };

                return collection.Find(new QueryDocument(@params)).ToArray();
            }).ConfigureAwait(false);
        }

        public static async Task<T> FindOneByObjectIdUserIdAsync<T>(this MongoCollection<T> collection, string objectId, string userId)
            where T : Document
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (objectId == null)
                throw new ArgumentNullException("objectId");
            if (userId == null)
                throw new ArgumentNullException("userId");

            return await Task.Run(() =>
            {
                var @params = new Dictionary<string, object>()
                {
                    { "_id", new ObjectId(objectId) },
                    { "UserId", userId },
                    { "EnvironmentMode", ConfigurationManager.AppSettings["EnvironmentMode"] }
                };

                return collection.FindOne(new QueryDocument(@params));
            }).ConfigureAwait(false);
        }

        public static async Task<T> FindOneByIdAsync<T>(this MongoCollection<T> collection, string objectId)
            where T : Document
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (objectId == null)
                throw new ArgumentNullException("objectId");

            return await Task.Run(() =>
            {
                return collection.FindOneById(new ObjectId(objectId));
            }).ConfigureAwait(false);
        }

        public static async Task<string> InsertAsync<T>(this MongoCollection<T> collection, T document)
            where T : Document
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (document == null)
                throw new ArgumentNullException("document");

            var result = await Task.Run(() =>
                {
                    return collection.Insert(document);
                }).ConfigureAwait(false);

            if (!result.Ok)
                throw new MongoDbException("Insert failed.", result);

            return document.Id.ToString();
        }

        public static Task<WriteConcernResult> RemoveAsync<T>(this MongoCollection<T> collection, T document)
            where T : Document
        {
            return RemoveAsync(collection, new QueryDocument("_id", document.Id));
        }
        public static async Task<WriteConcernResult> RemoveAsync<T>(this MongoCollection<T> collection, IMongoQuery query)
            where T : Document
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            var result = await Task.Run(() =>
            {
                return collection.Remove(query);
            }).ConfigureAwait(false);

            if (!result.Ok)
                throw new MongoDbException("Remove failed.", result);

            return result;
        }

        public static async Task<WriteConcernResult> RemoveAllAsync<T>(this MongoCollection<T> collection)
            where T : Document
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            var result = await Task.Run(() =>
            {
                return collection.RemoveAll();
            }).ConfigureAwait(false);

            if (!result.Ok)
                throw new MongoDbException("RemoveAll failed.", result);

            return result;
        }

    }
}