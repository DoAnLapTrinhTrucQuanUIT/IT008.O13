using Restaurant_Management.Utilities;
using Restaurant_Management.Views.Component;
using Restaurant_Management.Views;
using Restaurant_Management.Utilities.MongoDBManager;
using System;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Security.Cryptography;
using System.Windows.Controls;
using MongoDB.Bson;
using MongoDB.Driver;
using Restaurant_Management.Models;
using System.Collections.ObjectModel;

namespace Restaurant_Management.ViewModels
{
    public class LoginVM : Utilities.ViewModelBase
    {
        private readonly LoginWindow _loginWindow;
        public LoginVM(LoginWindow loginWindow)
        {
            _loginWindow = loginWindow ?? throw new ArgumentNullException(nameof(loginWindow));
        }
        public static bool IsLogin { get; set; }
        private string _username;
        private string _password;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
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
        
        public LoginVM()
        {
            IsLogin = false;
            Password = "";
            Username = "";
            LoginCM = new RelayCommand<LoginWindow>((p) => true, (p) => _Login(p));
            PasswordchangeCM = new RelayCommand<PasswordBox>((p) => true, (p) => { Password = p.Password; });
            ForgetpasswordCM = new RelayCommand<LoginWindow>((p) => true, (p) => _ForgetPassword(p));
        }

        void _Login(LoginWindow p)
        {
            var hashedPassword = HashPassword(_password);
            try
            {
                if (p == null) return;

                var dbContext = new MongoDbContext("mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/", "Restaurant_Management_Application");

                var username_filter = Builders<Account>.Filter.Eq("Username", Username);
                var username = dbContext.Accounts.Find(username_filter).FirstOrDefault();
                if (username == null)
                {
                    MessageBox.Show("Username is not exist!", "Announcement", MessageBoxButton.OK);
                }
                else
                {
                    var acc_filter = Builders<Account>.Filter.Eq("Username", Username) & Builders<Account>.Filter.Eq("Password", HashPassword(Password));
                    var account = dbContext.Accounts.Find(acc_filter).FirstOrDefault();
                    if (account != null)
                    {
                        IsLogin = true;
                        Const.UserName = Username;
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        Username = "";
                        Password = "";
                        p.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        MessageBox.Show("Password is incorrect!", "Announcement", MessageBoxButton.OK);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Announcement", MessageBoxButton.OK);
            }
        }


        void _ForgetPassword(LoginWindow parameter)
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

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
