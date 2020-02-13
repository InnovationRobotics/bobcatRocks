using MongoDB.Bson;
using MongoDB.Driver;
using Smartest.Infrastructure.Objects.Mongo;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;

namespace Assets.MonogoScripts
{
    public class MongoDBHelper
    {

        private static readonly ConcurrentDictionary<Type, MongoDBHelper> MongoDbHandlers = new ConcurrentDictionary<Type, MongoDBHelper>();
        private MongoClient Client { get; set; }

        private IMongoDatabase Db { get; set; }


        private static MongoDBHelper GetInstance<T>()
        {
            if (!MongoDbHandlers.ContainsKey(typeof(T)))
            {
                MongoDbHandlers.TryAdd(typeof(T), new MongoDBHelper());
            }
            return MongoDbHandlers[typeof(T)];
        }


        public MongoDBHelper()
        {
            try
            {
                CheckMongoServiceIsUp();

                Client = new MongoClient("mongodb://localhost:27017/"); // TODO Get From Config file
                Db = Client.GetDatabase("Smartest"); // TODO Get From Config file
            }
            catch
            {

            }
        }
        public static void SaveToCollection<T>(T item)
        {
            GetCollection<T>().InsertOne(item);
        }


        public static void UpdateToCollection<T>(T item)
        {
            // var filterElements = new List<FilterElement>();
            var filter = new DBFilterCreator();

            var updateElements = new Dictionary<string, object>();

            var props = item.GetType().GetProperties();

            foreach (var propertyInfo in props)
            {

                var propValue = propertyInfo.GetValue(item);
                if (propValue != null)
                {
                    object[] attrs = propertyInfo.GetCustomAttributes(true);
                    if (attrs.Length > 0)
                    {
                        foreach (object attr in attrs)
                        {
                            KeyAttribute authAttr = attr as KeyAttribute;
                            if (authAttr != null)
                            {
                                // Key Attribute
                                filter.Elements.Add(new FilterElement(propertyInfo.Name,
                                    propValue.ToString()));
                            }
                        }
                    }
                    else
                    {
                        bool addToDictionary = true;

                        if (propValue is DateTime)
                        {
                            if ((DateTime)propValue == DateTime.MinValue)
                            {
                                addToDictionary = false;
                            }
                        }

                        if (propValue is IEnumerable<object>)
                        {
                            if (((IEnumerable<object>)propValue).Count() == 0)
                            {
                                addToDictionary = false;
                            }
                        }

                        if (addToDictionary)
                            updateElements.Add(propertyInfo.Name, propValue);
                    }
                }
            }

            // var filter = new DBFilterCreator(filterElements);

            var updateFilter = new BsonDocument(updateElements);

            GetCollection<T>().FindOneAndUpdate(filter.GenerateFilter(), new BsonDocument("$set", updateFilter));
        }


        public static List<T> GetCollectionAsList<T>(DBFilterCreator newfilter = null)
        {
            FilterDefinition<T> filter;
            if (newfilter == null)
                filter = FilterDefinition<T>.Empty;
            else
            {
                filter = newfilter.GenerateFilter();
            }
            return GetCollection<T>().FindSync(filter).ToList();
        }



        #region Private Methodes

        private static IMongoCollection<T> GetCollection<T>()
        {
            var mongoDbHandlerInstance = GetInstance<T>();
            return mongoDbHandlerInstance.Db.GetCollection<T>(typeof(T).Name);
        }

        private void CheckMongoServiceIsUp()
        {
            ServiceController sc = new ServiceController("MongoDB");// TODO Get From Config file

            switch (sc.Status)
            {
                case ServiceControllerStatus.Stopped:
                case ServiceControllerStatus.Paused:
                case ServiceControllerStatus.StopPending:
                    sc.Start();
                    break;
            }
        }

        #endregion

    }
}