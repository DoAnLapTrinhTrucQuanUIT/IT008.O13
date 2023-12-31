using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Management.Models
{
    public class SalaryInformation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        [BsonElement("employeeInfo")]
        public Employees Employees { get; set; }

        [BsonElement("startDate")]
        public DateTime StartDate { get; set; }

        [BsonElement("payDate")]
        public DateTime PayDate { get; set; }

        [BsonElement("workedDays")]
        public int WorkedDays { get; set; }

        [BsonElement("basicSalary")]
        public decimal BasicSalary { get; set; }

        public decimal TotalSalary { get; set; }

    }
}
