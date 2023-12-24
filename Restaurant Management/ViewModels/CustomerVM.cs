using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Restaurant_Management.Models;
using Restaurant_Management.Utilities;
using Restaurant_Management.Views;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Windows.Documents;

namespace Restaurant_Management.ViewModels
{
    public class CustomerVM : Utilities.ViewModelBase
    {
        public int TotalCustomer
        {
            get { return GetTotalCustomerCount(); } // Đơn giản là đếm số lượng phần tử trong danh sách
        }

        public double Percent
        {
            get { return CalculatePercentage(); } // Hàm tính toán tỷ lệ phần trăm
        }

        public string Status
        {
            get { return CalculateStatus(); } // Hàm tính toán trạng thái
        }

        private ObservableCollection<Customers> _customerList;
        public ObservableCollection<Customers> CustomerList
        {
            get { return _customerList; }
            set
            {
                _customerList = value;
                OnPropertyChanged(nameof(CustomerList));
            }
        }
        public ICommand SearchCM { get; set; }
        public ICommand AddCustomerCM { get; set; }
        public ICommand ExportCustomerCM { get; set; }

        private readonly IMongoCollection<Customers> _Customers;
        public CustomerVM() 
        {
            _Customers = GetCustomers();
            SearchCM = new RelayCommand<CustomerView>((p) => true, (p) => _Search(p));
            AddCustomerCM = new RelayCommand<CustomerView>((p) => true, (p) => _AddCustomer(p));
            ExportCustomerCM = new RelayCommand<CustomerView>((p) => true, (p) => _ExportCustomer(p));
        }
        private IMongoCollection<Customers> GetCustomers()
        {
            // Set your MongoDB connection string and database name
            string connectionString =
                "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "Restaurant_Management_Application"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Customers>("Customers");
        }

        void _Search(CustomerView paramater)
        {
            ObservableCollection<Customers> temp = new ObservableCollection<Customers>();
            if(paramater.txtSearch.Text != "")
            {
                var filterBuilder = Builders<Customers>.Filter;
                FilterDefinition<Customers> filter;
                filter = filterBuilder.Regex("customerId", new BsonRegularExpression(paramater.txtSearch.Text, "i"));
                var result = _Customers.Find(filter).ToList();
                temp = new ObservableCollection<Customers>(result);
            }
            else
            {
                var result = _Customers.Find(Builders<Customers>.Filter.Empty).ToList();
                temp = new ObservableCollection<Customers>(result);
            }
            paramater.DataGridCustomers.ItemsSource = temp;
        }

        void _AddCustomer(CustomerView parameter)
        {

        }

        void _ExportCustomer(CustomerView parameter)
        {

        }
        public int GetTotalCustomerCount()
        {
            var totalCustomerCount = _Customers.AsQueryable().Count();
            return totalCustomerCount;
        }
        private double CalculatePercentage()
        {
            return 0.0;
        }

        private string CalculateStatus()
        {
            return "Ready";
        }
    }
}
