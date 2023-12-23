using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Management.Models
{
    public class InvoiceDetails
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("invoiceDetailId")]
        public string InvoiceDetailId { get; set; }

        [BsonElement("invoiceId")]
        public string InvoiceId { get; set; }

        [BsonElement("itemId")]
        public string ItemId { get; set; }

        [BsonElement("quantity")]
        public int Quantity { get; set; }

        [BsonElement("subtotal")]
        public double Subtotal { get; set; }
    }
}
