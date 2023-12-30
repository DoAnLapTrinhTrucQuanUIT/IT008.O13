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
        public ObjectId _id { get; set; }

        [BsonElement("tableId")]
        public string TableId { get; set; }

        [BsonElement("invoicesTable")]
        public Invoices InvoicesTable { get; set; }

        [BsonElement("tableName")]
        public string TableName { get; set; }

        [BsonElement("status")]
        public bool Status { get; set; }
    }
}
