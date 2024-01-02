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
using System.Diagnostics;
using System.ComponentModel;
using System.Globalization;

namespace Restaurant_Management.ViewModels
{
    public class CustomerVM : Utilities.ViewModelBase
    {
        private bool _newStatus;
        public bool NewStatus
        {
            get { return _newStatus; }
            set
            {
                _newStatus = value;
                OnPropertyChanged(nameof(NewStatus));
            }
        }

        private int _totalCustomerQuantity;
        public int TotalCustomerQuantity
        {
            get { return _totalCustomerQuantity; }
            set
            {
                _totalCustomerQuantity = value;
                OnPropertyChanged(nameof(TotalCustomerQuantity));
            }
        }

        private double _totalCustomerPercent;
        public double TotalCustomerPercent
        {
            get { return _totalCustomerPercent; }
            set
            {
                _totalCustomerPercent = value;
                OnPropertyChanged(nameof(TotalCustomerPercent));
            }
        }

        private int _newCustomerQuantity;
        public int NewCustomerQuantity
        {
            get { return _newCustomerQuantity; }
            set
            {
                _newCustomerQuantity = value;
                OnPropertyChanged(nameof(NewCustomerQuantity));
            }
        }

        private double _newCustomerPercent;
        public double NewCustomerPercent
        {
            get { return _newCustomerPercent; }
            set
            {
                _newCustomerPercent = value;
                OnPropertyChanged(nameof(NewCustomerPercent));
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

        private ObservableCollection<Customers> _searchCustomerList;

        public ObservableCollection<Customers> SearchCustomerList
        {
            get { return _searchCustomerList; }
            set
            {
                _searchCustomerList = value;
                OnPropertyChanged(nameof(SearchCustomerList));
            }
        }

        public ICommand SearchCM { get; set; }

        public ICommand AddCustomerCM { get; set; }

        public ICommand ExportCustomerCM { get; set; }

        public ICommand ImportCustomerCM { get; set; }

        public ICommand DeletedCustomerCommand { get; set; }

        private readonly IMongoCollection<Customers> _Customers;

        private IMongoCollection<Customers> GetCustomers()
        {
            string connectionString = "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/";
            
            string databaseName = "Restaurant_Management_Application"; 

            var client = new MongoClient(connectionString);
            
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Customers>("Customers");
        }

        public CustomerVM() 
        {
            _Customers = GetCustomers();

            LoadCustomerBar();
            
            LoadCustomers();
            
            InitalizeCommand();
        }

        private void InitalizeCommand()
        {
            SearchCM = new RelayCommand<CustomerView>((p) => true, (p) => _Search(p));
            
            AddCustomerCM = new RelayCommand<CustomerView>((p) => true, (p) => _AddCustomer());
            
            ExportCustomerCM = new RelayCommand<CustomerView>((p) => true, (p) => _ExportCustomer());
            
            ImportCustomerCM = new RelayCommand<CustomerView>((p) => true, (p) => _ImportCustomer());
            
            DeletedCustomerCommand = new RelayCommand<Customers>((customer) => true, (customer) => _DeleteCustomer(customer));
        }
        private void _Search(CustomerView parameter)
        {   
            SearchCustomerList = new ObservableCollection<Customers>();
            
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
                
                SearchCustomerList = new ObservableCollection<Customers>(result);
            }
            else
            {
                var result = _Customers.Find(Builders<Customers>.Filter.Empty).ToList();
                
                SearchCustomerList = new ObservableCollection<Customers>(result);
            }
            
            parameter.DataGridCustomers.ItemsSource = SearchCustomerList;
        }

        private void _DeleteCustomer(Customers customer)
        {
            if (customer != null)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {customer.FullName} ?",
                                                          "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _Customers.DeleteOne(Builders<Customers>.Filter.Eq("customerId", customer.CustomerId));

                    LoadCustomerBar();

                    LoadCustomers();
                }
            }
        }

        private void LoadCustomers()
        {
            var customers = _Customers.Find(Builders<Customers>.Filter.Empty).ToList();

            CustomerList = new ObservableCollection<Customers>(customers);
        }

        private void _AddCustomer()
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

            LoadCustomerBar();
        }

        private void _ExportCustomer()
        {

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",

                DefaultExt = "csv",

                Title = "Export Customer List"
            };

            var result = saveFileDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                var filePath = saveFileDialog.FileName;

                StringBuilder csvContent = new StringBuilder();

                csvContent.AppendLine("Customer ID,Full Name, Address, Phone Number, Email, Gender, Registration Date, Sales");

                var exportList = (SearchCustomerList != null && SearchCustomerList.Count > 0) ? SearchCustomerList : CustomerList;

                foreach (var customer in exportList)
                {
                    string formattedRegistrationDate = customer.RegistrationDate.ToString("dd/MM/yy");
                    
                    string formattedSales = customer.Sales.ToString("0.00", CultureInfo.InvariantCulture);

                    csvContent.AppendLine($"{customer.CustomerId},{customer.FullName},{customer.Address},{customer.PhoneNumber},{customer.Email},{customer.Gender},{formattedRegistrationDate},{formattedSales}");
                }

                File.WriteAllText(filePath, csvContent.ToString());

                LoadCustomerBar();
                
                MessageBox.Show("Export file successfully!");
            }
        }

        private void _ImportCustomer()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",

                DefaultExt = "csv",

                Title = "Import Customer List"
            };

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
                            
                            Sales = double.Parse(values[7], CultureInfo.InvariantCulture)
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

                    LoadCustomerBar();
                   
                    MessageBox.Show($"Import file successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error importing customers: {ex.Message}");
                }
            }
        }

        private void LoadCustomerBar()
        {
            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            DateTime lastDayOfPreviousMonth = firstDayOfMonth.AddDays(-1);

            var previousMonthFilter = Builders<Customers>.Filter.Gte("RegistrationDate", firstDayOfMonth.AddMonths(-1)) &
                                      Builders<Customers>.Filter.Lte("RegistrationDate", lastDayOfPreviousMonth);

            int totalNewCustomersPreviousMonth = (int)_Customers.CountDocuments(previousMonthFilter);

            int totalCustomersQuantity = (int)_Customers.CountDocuments(Builders<Customers>.Filter.Empty);

            int newCustomerQuantity = (int)_Customers.CountDocuments(Builders<Customers>.Filter.Gte("RegistrationDate", firstDayOfMonth));

            double totalCustomerQuantityPreviousMonth = totalCustomersQuantity - newCustomerQuantity;

            double newCustomerPercent = 0;

            if(totalNewCustomersPreviousMonth > 0)
            {
                if(totalNewCustomersPreviousMonth < newCustomerQuantity)
                {
                    NewStatus = true;
                }
                else
                {
                    NewStatus = false;
                }
                
                newCustomerPercent = Math.Abs((1- ((double)newCustomerQuantity / totalNewCustomersPreviousMonth))) * 100;
            }
            else
            {
                NewStatus = true;
                
                newCustomerPercent = 100;
            }

            double totalCustomerPercent = ((double)(newCustomerQuantity / totalCustomerQuantityPreviousMonth) * 100);

            TotalCustomerQuantity = totalCustomersQuantity;

            NewCustomerQuantity = newCustomerQuantity;

            TotalCustomerPercent = totalCustomerPercent;

            NewCustomerPercent = newCustomerPercent;
        }
    }
}
