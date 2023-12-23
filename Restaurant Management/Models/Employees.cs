using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Management.Models
{
    public class Employees
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("employeeId")]
        public string EmployeeId { get; set; }

        [BsonElement("fullName")]
        public string FullName { get; set; }

        [BsonElement("isAdmin")]
        public bool IsAdmin { get; set; }

        [BsonElement("phoneNumber")]
        public string PhoneNumber { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("address")]
        public string Address { get; set; }

        [BsonElement("dateOfJoining")]
        public DateTime DateOfJoining { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("isActive")]
        public bool IsActive { get; set; }
    }
}
