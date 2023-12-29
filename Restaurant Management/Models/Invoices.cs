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
        public string _id { get; set; }

        [BsonElement("invoiceId")]
        public string InvoiceId { get; set; }

        [BsonElement("createdDate")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("paidEmployee")]
        public Employees PaidEmployee { get; set; }

        [BsonElement("paidCustomer")]
        public Customers PaidCustomer { get; set; }

        [BsonElement("items")]
        public List<MenuItems> Items { get; set; }

        [BsonElement("status")]
        public bool Status { get; set; }

        [BsonElement("amount")]
        public double Amount { get; set; }
    }
}
