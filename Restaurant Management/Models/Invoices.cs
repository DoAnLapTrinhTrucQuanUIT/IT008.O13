using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.RightsManagement;

namespace Restaurant_Management.Models
{
    public class Invoices
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        [BsonElement("invoiceId")]
        public string InvoiceId { get; set; }

        [BsonElement("employee")]
        public Employees Employee { get; set; }

        [BsonElement("customer")]
        public Customers Customer { get; set; }

        [BsonElement("table")]
        public Tables Table { get; set; }

        [BsonElement("createdDate")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("status")]
        public bool Status { get; set; }

        [BsonElement("totalAmount")]
        public double TotalAmount { get; set; }
    }
}
