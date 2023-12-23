using Restaurant_Management.Utilities;
using Restaurant_Management.Views.Component;
using Restaurant_Management.Views;
using System;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Security.Cryptography;
using System.Windows.Controls;
using Restaurant_Management.Models;
using System.Collections.ObjectModel;
using System.Security.Principal;
using MongoDB.Driver;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Restaurant_Management.ViewModels
{
    public class LoginVM : Utilities.ViewModelBase
    {
        public static bool IsLogin { get; set; }
        private string _employeeId;
        private string _password;
        public string EmployeeId
        {
            get => _employeeId;
            set
            {
                _employeeId = value;
                OnPropertyChanged(nameof(EmployeeId));
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public ICommand LoginCM { get; set; }
        public ICommand PasswordchangeCM { get; set; }
        public ICommand ForgetpasswordCM { get; set; }

        private readonly IMongoCollection<Employees> _Employees;
        public LoginVM()
        {
            _Employees = GetMongoCollection();
            IsLogin = false;
            Password = "";
            EmployeeId = "";
            LoginCM = new RelayCommand<LoginWindow>((p) => true, (p) => _Login());
            PasswordchangeCM = new RelayCommand<PasswordBox>((p) => true, (p) => { Password = p.Password; });
            ForgetpasswordCM = new RelayCommand<LoginWindow>((p) => true, (p) => _ForgetPassword(p));
        }

        private IMongoCollection<Employees> GetMongoCollection()
        {
            // Set your MongoDB connection string and database name
            string connectionString =
                "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "Restaurant_Management_Application"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Employees>("Employees");
        }

       
        private void _Login()
        {
            if (EmployeeId != null && !string.IsNullOrEmpty(Password))
            {
                var user = GetUser(EmployeeId);

                if (user == null)
                {
                    MessageBox.Show("Sai tên tài khoản hoặc mật khẩu.");
                    return;
                }

                if (Password == user.Password)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    CloseAction?.Invoke();
                }
                else
                {
                    MessageBox.Show("Sai tên tài khoản hoặc mật khẩu.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
            }
        }

        public Action CloseAction { get; set; }

        private Employees GetUser(string employeeId)
        {
            var filter = Builders<Employees>.Filter.Eq(u => u.EmployeeId, employeeId);
            return _Employees.Find(filter).FirstOrDefault();
        }

        void _ForgetPassword(LoginWindow p)
        {
            ForgetPasswordView forgetPassControl = new ForgetPasswordView();

            Window forgetPassWindow = new Window
            {
                Content = forgetPassControl,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStyle = WindowStyle.None,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            forgetPassWindow.ShowDialog();
        }
    }
}