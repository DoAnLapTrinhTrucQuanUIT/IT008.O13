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
using Microsoft.Win32;

namespace Restaurant_Management.ViewModels
{
    public class CustomerVM : Utilities.ViewModelBase
    {
        public string TotalCustomer
        {
            get { return GetTotalCustomerCount(); } // Đơn giản là đếm số lượng phần tử trong danh sách
        }

        public string Percent
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
        public ICommand ImportCustomerCM { get; set; }
        public ICommand DeletedCustomerCommand { get; set; }

        private readonly IMongoCollection<Customers> _Customers;
        public CustomerVM() 
        {
            _Customers = GetCustomers();
            LoadCustomers();
            SearchCM = new RelayCommand<CustomerView>((p) => true, (p) => _Search(p));
            AddCustomerCM = new RelayCommand<CustomerView>((p) => true, (p) => _AddCustomer());
            ExportCustomerCM = new RelayCommand<CustomerView>((p) => true, (p) => _ExportCustomer());
            ImportCustomerCM = new RelayCommand<CustomerView>((p) => true, (p) => _ImportCustomer());
            DeletedCustomerCommand = new RelayCommand<Customers>((customer) => true, (customer) => _DeleteCustomer(customer));

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

        void _AddCustomer()
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

        void _ExportCustomer()
        {
            // Create a SaveFileDialog
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
                DefaultExt = "csv",
                Title = "Export Customer List"
            };

            // Show the SaveFileDialog and get the selected file path
            var result = saveFileDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                var filePath = saveFileDialog.FileName;

                StringBuilder csvContent = new StringBuilder();

                // Add the header row to the CSV content
                csvContent.AppendLine("Customer ID,Full Name, Address, Phone Number, Email, Gender, Registration Date, Sales");

                // Add customer data to the CSV content
                foreach (var customer in CustomerList)
                {
                    csvContent.AppendLine($"{customer.CustomerId},{customer.FullName},{customer.Address},{customer.PhoneNumber},{customer.Email},{customer.Gender},{customer.RegistrationDate},{customer.Sales}");
                }

                // Write the CSV content to the selected file
                File.WriteAllText(filePath, csvContent.ToString());

                MessageBox.Show($"Customer list exported successfully!");
            }
        }

        void _ImportCustomer()
        {
            // Create an OpenFileDialog
            var openFileDialog = new OpenFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
                DefaultExt = "csv",
                Title = "Import Customer List"
            };

            // Show the OpenFileDialog and get the selected file path
            var result = openFileDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                var filePath = openFileDialog.FileName;

                try
                {
                    var csvLines = File.ReadAllLines(filePath);

                    var customerDataLines = csvLines.Skip(1);

                    var newCustomers = new List<Customers>();

                    foreach (var line in customerDataLines)
                    {
                        var values = line.Split(',');

                        var newCustomer = new Customers
                        {
                            CustomerId = values[0],
                            FullName = values[1],
                            Address = values[2],
                            PhoneNumber = values[3],
                            Email = values[4],
                            Gender = values[5],
                            RegistrationDate = DateTime.Parse(values[6]),
                            Sales = double.Parse(values[7])
                        };

                        var existingCustomer = _Customers.Find(Builders<Customers>.Filter.Eq("customerId", newCustomer.CustomerId)).FirstOrDefault();

                        if (existingCustomer == null)
                        {
                            newCustomers.Add(newCustomer);
                        }
                    }

                    foreach (var newCustomer in newCustomers)
                    {
                        CustomerList.Add(newCustomer);
                        _Customers.InsertOne(newCustomer);
                    }

                    MessageBox.Show($"Customers imported successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error importing customers: {ex.Message}");
                }
            }
        }
        private void _DeleteCustomer(Customers customer)
        {
            // Implementation for deleting an employee
            // You can use _employees collection to perform delete operation
            // Implement logic to delete the selected employee
            if (customer != null)
            {
                // Confirm deletion with the user
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {customer.FullName}?",
                                                          "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Perform deletion logic here
                    _Customers.DeleteOne(Builders<Customers>.Filter.Eq("customerId", customer.CustomerId));

                    // Reload the staff list after deletion
                    LoadCustomers();
                }
            }
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
