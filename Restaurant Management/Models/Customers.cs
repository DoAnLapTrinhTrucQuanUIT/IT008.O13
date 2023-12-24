using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Restaurant_Management.Models
{
    public class Customers
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("customerId")]
        public string CustomerId { get; set; }

        [BsonElement("fullName")]
        public string FullName { get; set; }

        [BsonElement("phoneNumber")]
        public string PhoneNumber { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("address")]
        public string Address { get; set; }

        [BsonElement("gender")]
        public string Gender { get; set; }

        [BsonElement("registrationDate")]
        public DateTime RegistrationDate { get; set; }

        [BsonElement("sales")]
        public double Sales { get; set; }
    }
}
