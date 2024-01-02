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
using System.Windows.Controls;
using System.Windows.Input;
using static Org.BouncyCastle.Crypto.Digests.SkeinEngine;

namespace Restaurant_Management.ViewModels
{
    public class EditCustomerVM : Utilities.ViewModelBase
    {
        private Customers _customer;
        public Customers Customer
        {
            get { return _customer; }
            set
            {
                _customer = value;
                OnPropertyChanged(nameof(Customer));
            }
        }
        public ICommand CancelCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public ICommand LoadWDCM { get; set; }
        public ICommand CloseWDCM { get; set; }
        public ICommand MinimizeWDCM { get; set; }
        public ICommand MoveWDCM { get; set; }


        private readonly IMongoCollection<Customers> _Customers;
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

        public EditCustomerVM(Customers customer)
        {
            Customer = customer;
            _Customers = GetCustomers();

            LoadWDCM = new RelayCommand<EditCustomer>(p => true, p => _LoadWinDow(p));
            CancelCommand = new RelayCommand<EditCustomer>(p => true, p => _CancelCommand(p));
            ConfirmCommand = new RelayCommand<EditCustomer>(p => true, p => _ConfirmCommand(p));
            CloseWDCM = new RelayCommand<EditCustomer>(p => true, p => _CloseWD(p));
            MinimizeWDCM = new RelayCommand<EditCustomer>(p => true, p => _MinimizeWD(p));
            MoveWDCM = new RelayCommand<EditCustomer>(p => true, p => _MoveWD(p));
        }

        private void _LoadWinDow(EditCustomer customer)
        {
            if (customer != null)
            {
                customer.FullName.Text = Customer.FullName;
                customer.PhoneNumber.Text = Customer.PhoneNumber;
                customer.Email.Text = Customer.Email;
                customer.Address.Text = Customer.Address;

                // Set GenderComboBox.SelectedItem based on the existing gender in Customer
                var genderComboBoxItem = customer.GenderComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == Customer.Gender);
                if (genderComboBoxItem != null)
                {
                    customer.GenderComboBox.SelectedItem = genderComboBoxItem;
                }
            }
        }




        private void _CancelCommand(EditCustomer customer)
        {
            if (customer != null)
            {
                // Khôi phục lại giá trị ban đầu của các trường trong EditItem
                customer.FullName.Text = Customer.FullName;
                customer.PhoneNumber.Text = Customer.PhoneNumber;
                customer.Email.Text = Customer.Email;
                customer.Address.Text = Customer.Address;
                customer.GenderComboBox.SelectedItem = Customer.Gender;
                customer.Address.Text = Customer.Address;

                // Đóng cửa sổ khi hủy bỏ lệnh sửa
                var window = Window.GetWindow(customer);
                if (window != null)
                {
                    window.Close();
                }
            }
        }



        private void _ConfirmCommand(EditCustomer customer)
        {
            try
            {
                if (customer != null)
                {
                    // Kiểm tra xem người dùng đã nhập đủ thông tin hay chưa
                    if (string.IsNullOrEmpty(customer.FullName.Text) || string.IsNullOrEmpty(customer.Email.Text) ||
                        string.IsNullOrEmpty(customer.PhoneNumber.Text) || string.IsNullOrEmpty(customer.Address.Text) ||
                        string.IsNullOrEmpty(customer.GenderComboBox.SelectedItem.ToString()))
                    {
                        MessageBox.Show("Please enter complete information!", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Cập nhật thông tin của MenuItem từ giao diện
                    Customer.FullName = customer.FullName.Text;

                    Customer.Email = customer.Email.Text;

                    Customer.Address = customer.Address.Text;

                    Customer.PhoneNumber = customer.PhoneNumber.Text;

                    Customer.Gender = customer.PhoneNumber.Text;

                    // Create a filter based on the unique identifier (CustomerId)
                    var filter = Builders<Customers>.Filter.Eq(x => x.CustomerId, Customer.CustomerId);

                    // Create an update with the fields you want to modify
                    var update = Builders<Customers>.Update
                        .Set(x => x.FullName, Customer.FullName)
                        .Set(x => x.PhoneNumber, Customer.PhoneNumber)
                        .Set(x => x.Address, Customer.Address)
                        .Set(x => x.Email, Customer.Email)
                        .Set(x => x.Gender, Customer.Gender);

                    // Apply the update to the MongoDB document without modifying the _id field
                    _Customers.UpdateOne(filter, update);

                    // Đóng cửa sổ khi cập nhật thành công
                    var window = Window.GetWindow(customer);
                    if (window != null)
                    {
                        window.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error confirming update: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void _CloseWD(EditCustomer parameter)
        {
            var window = Window.GetWindow(parameter);
            if (window != null && Mouse.LeftButton == MouseButtonState.Pressed)
            {
                window.Close();
            }
        }

        private void _MinimizeWD(EditCustomer parameter)
        {
            var window = Window.GetWindow(parameter);
            if (window != null && Mouse.LeftButton == MouseButtonState.Pressed)
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        private void _MoveWD(EditCustomer parameter)
        {
            var window = Window.GetWindow(parameter);
            if (window != null && Mouse.LeftButton == MouseButtonState.Pressed)
            {
                window.DragMove();
            }
        }
    }
}