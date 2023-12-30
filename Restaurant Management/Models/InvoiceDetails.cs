using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Restaurant_Management.Models
{
    
    public class InvoiceDetails
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        [BsonElement("invoiceDetailId")]
        public string InvoiceDetailId { get; set; }

        [BsonElement("invoice")]
        public Invoices Invoice { get; set; }

        [BsonElement("item")]
        public MenuItems Item { get; set; }

        [BsonElement("quantity")]
        public int Quantity { get; set; }

        [BsonElement("amount")]
        public double Amount { get; set; }
    }
}
