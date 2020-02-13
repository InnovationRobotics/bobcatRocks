using System; 
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Smartest.Infrastructure.Objects.Mongo
{
    public class DBFilterCreator
    {

        public DBFilterCreator(List<FilterElement> elements)
        {
            Elements = elements;
        }

        public DBFilterCreator()
        {
            Elements = new List<FilterElement>();
        }

        public List<FilterElement> Elements { get; }

        public BsonDocument GenerateFilter()
        {
            var items = new Dictionary<string, object>();

            if (Elements.Count > 0)
            {
                foreach (var filterElement in Elements)
                { 
                    items.Add(filterElement.Key, new BsonDocument($"${filterElement.Operator.ToString().ToLower()}", filterElement.Value));
                }
            }

            return  new BsonDocument(items);
        }
    }
}
