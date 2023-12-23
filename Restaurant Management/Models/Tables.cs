using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Management.Models
{
    public class Tables
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("tableId")]
        public string TableId { get; set; }

        [BsonElement("tableName")]
        public string TableName { get; set; }

        [BsonElement("isReserved")]
        public bool IsReserved { get; set; }
    }
}
