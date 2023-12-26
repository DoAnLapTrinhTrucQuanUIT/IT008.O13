using System;
using System.IO;
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
using Restaurant_Management.Views.Component;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace Restaurant_Management.ViewModels
{
    public class CustomerVM : Utilities.ViewModelBase
    {
        private string _totalCustomer;
        public string TotalCustomer
        {
            get { return GetTotalCustomerCount(); } // Đơn giản là đếm số lượng phần tử trong danh sách
            set
            {
                value = _totalCustomer;
                OnPropertyChanged(nameof(_totalCustomer));
            }
        }

        private string _percent;
        public string Percent
        {
            get { return CalculatePercentage(); } // Hàm tính toán tỷ lệ phần trăm
            set
            {
                value = _percent;
                OnPropertyChanged(nameof(_percent));
            }
        }

        private string _status;

        public string Status
        {
            get { return CalculateStatus(); }// Hàm tính toán trạng thái
            set
            {
                value = _status;
                OnPropertyChanged(nameof(_status));
            }
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
            LoadCustomers();
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
        private void LoadCustomers()
        {
            var customers = _Customers.Find(Builders<Customers>.Filter.Empty).ToList();
            CustomerList = new ObservableCollection<Customers>(customers);
        }
        void _Search(CustomerView parameter)
        {
            ObservableCollection<Customers> temp = new ObservableCollection<Customers>();
            if (!string.IsNullOrEmpty(parameter.txtSearch.Text))
            {
                var filterBuilder = Builders<Customers>.Filter;
                FilterDefinition<Customers> filter;

                var keyword = parameter.txtSearch.Text;

                filter = filterBuilder.Or(
                    filterBuilder.Regex("customerId", new BsonRegularExpression(keyword, "i")),
                    filterBuilder.Regex("fullName", new BsonRegularExpression(keyword, "i")),
                    filterBuilder.Regex("phoneNumber", new BsonRegularExpression(keyword, "i"))
                );
                var result = _Customers.Find(filter).ToList();
                temp = new ObservableCollection<Customers>(result);
            }
            else
            {
                var result = _Customers.Find(Builders<Customers>.Filter.Empty).ToList();
                temp = new ObservableCollection<Customers>(result);
            }
            parameter.DataGridCustomers.ItemsSource = temp;
        }

        void _AddCustomer(CustomerView parameter)
        {
            AddCustomer addCustomer = new AddCustomer();
            var window = new Window
            {
                Content = addCustomer,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStyle = WindowStyle.None,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();
            LoadCustomers();
        }

        void _ExportCustomer(CustomerView parameter)
        {
            StringBuilder csvContent = new StringBuilder();

            // Add the header row to the CSV content
            csvContent.AppendLine("Customer ID,Full Name, Address, Phone Number, Email, Gender, Registration Date, Sales");

            // Add customer data to the CSV content
            foreach (var customer in CustomerList)
            {
                csvContent.AppendLine($"{customer.CustomerId},{customer.FullName}, {customer.Address},{customer.PhoneNumber}, {customer.Email}, {customer.Gender}, {customer.RegistrationDate}, {customer.Sales}");
            }

            // Specify the file path
            var filePath = "\\\\Mac\\Home\\Documents\\FresherYear\\LTTQ\\DACK\\CustomerList.csv"; // Set your desired file path

            // Write the CSV content to the file
            File.WriteAllText(filePath, csvContent.ToString());

            MessageBox.Show("Customer list exported successfully to CSV!");
        }
        public string GetTotalCustomerCount()
        {
            return "1000";
        }
        private string CalculatePercentage()
        {
            return "345";
        }

        private string CalculateStatus()
        {
            return "57";
        }
    }
}
