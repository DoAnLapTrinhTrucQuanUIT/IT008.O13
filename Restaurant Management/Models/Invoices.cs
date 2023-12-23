using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Management.Models
{
    public class Invoices
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("invoiceId")]
        public string InvoiceId { get; set; }

        [BsonElement("creationDate")]
        public DateTime CreationDate { get; set; }

        [BsonElement("employeeId")]
        public string EmployeeId { get; set; }

        [BsonElement("items")]
        public List<MenuItems> Items { get; set; }

        [BsonElement("totalAmount")]
        public double TotalAmount { get; set; }

        [BsonElement("paymentMethod")]
        public string PaymentMethod { get; set; }
    }
}
