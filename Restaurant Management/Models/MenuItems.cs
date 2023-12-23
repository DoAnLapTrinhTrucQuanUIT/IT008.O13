using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Management.Models
{
    public class MenuItems
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("itemId")]
        public string ItemId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("price")]
        public double Price { get; set; }

        [BsonElement("title")]
        public double Title { get; set; }
    }
}
