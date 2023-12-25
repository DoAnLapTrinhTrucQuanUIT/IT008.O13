using MongoDB.Driver;
using Restaurant_Management.Models;
using Restaurant_Management.Utilities;
using Restaurant_Management.Views.Component;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Restaurant_Management.ViewModels
{
    public class AddCustomerVM : Utilities.ViewModelBase
    {
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
        public ICommand CancelCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }

        private readonly IMongoCollection<Customers> _Customers;
        public AddCustomerVM()
        {
            _Customers = GetCustomers();
            CancelCommand = new RelayCommand<AddCustomer>((p) => true, (p) => _CancelCommand(p));
            ConfirmCommand = new RelayCommand<AddCustomer>((p) => true, (p) => _ConfirmCommand(p));
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
        void _CancelCommand(AddCustomer paramater)
        {
            paramater.FullName.Clear();
            paramater.PhoneNumber.Clear();
            paramater.GenderComboBox.SelectedItem=null;
            paramater.Email.Clear();
            paramater.Address.Clear();
        }

        void _ConfirmCommand(AddCustomer paramater)
        {
            if (paramater.FullName.Text==""||paramater.PhoneNumber.Text==""||paramater.GenderComboBox.SelectedItem==null||paramater.Address.Text==""||paramater.Email.Text=="")
            {
                MessageBox.Show("You did not enter enough information!", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBoxResult addCusNoti = System.Windows.MessageBox.Show("Do you want to add customers?", "Notification", MessageBoxButton.YesNoCancel);
            if (addCusNoti == MessageBoxResult.Yes)
            {
                if (string.IsNullOrEmpty(paramater.FullName.Text) || string.IsNullOrEmpty(paramater.PhoneNumber.Text) || string.IsNullOrEmpty(paramater.GenderComboBox.Text) || string.IsNullOrEmpty(paramater.Address.Text))
                {
                    MessageBox.Show("Incomplete information!", "Notification");
                }
                else
                {
                    var filter = Builders<Customers>.Filter.Eq(x => x.CustomerId, paramater.FullName.Text);
                    var User = _Customers.Find(filter).FirstOrDefault();
                    if (User != null)
                    {
                        MessageBox.Show("Customer already exists!", "Notification");
                    }
                    else
                    {
                        AddCustomer(paramater);
                    }
                }
            }

        }

        private void AddCustomer(AddCustomer paramater)
        {
            // Tạo một đối tượng Person mới
            var Customer = new Customers
            {
                CustomerId = GenerateRandomCustomerId(),
                FullName = paramater.FullName.Text.ToString(),
                PhoneNumber = paramater.PhoneNumber.Text.ToString(),
                Gender = paramater.GenderComboBox.Text.ToString(),
                Address = paramater.Address.Text.ToString(),
                Email = paramater.Email.Text.ToString(),
                RegistrationDate = DateTime.Now,
                Sales = 0
            };
            // Thêm đối tượng vào collection
            _Customers.InsertOne(Customer);

            MessageBox.Show("Customer added successfully.", "Notification");
            paramater.FullName.Clear();
            paramater.PhoneNumber.Clear();
            paramater.GenderComboBox.SelectedItem=null;
            paramater.Email.Clear();
            paramater.Address.Clear();
        }

        string GenerateRandomCustomerId()
        {
            // Lấy `CustomerId` lớn nhất hiện có
            var maxCustomerId = _Customers.AsQueryable()
                .OrderByDescending(c => c.CustomerId)
                .FirstOrDefault()?.CustomerId;

            // Tạo `CustomerId` mới với số thứ tự kế tiếp
            string newCustomerId = GenerateNextCustomerId(maxCustomerId);

            // Kiểm tra nếu `CustomerId` mới đã tồn tại
            while (Check(newCustomerId))
            {
                newCustomerId = GenerateNextCustomerId(newCustomerId);
            }

            return newCustomerId;
        }

        string GenerateNextCustomerId(string currentMaxCustomerId)
        {
            if (string.IsNullOrEmpty(currentMaxCustomerId))
            {
                return "CUS1";
            }

            // Trích xuất số từ `CustomerId` lớn nhất hiện có
            string maxNumberStr = currentMaxCustomerId.Substring(3);
            if (int.TryParse(maxNumberStr, out int maxNumber))
            {
                // Tạo `CustomerId` mới với số thứ tự kế tiếp
                return "CUS" + (maxNumber + 1).ToString();
            }

            // Trong trường hợp không thành công, trả về `CUS1`
            return "CUS1";
        }

        bool Check(string customerId)
        {
            return _Customers.AsQueryable().Any(temp => temp.CustomerId == customerId);
        }
    }
}