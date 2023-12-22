using MongoDB.Driver;
using Restaurant_Management.Models;
using Restaurant_Management.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Restaurant_Management.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly IMongoCollection<Account> _Account;
        public LoginWindow()
        {
            InitializeComponent();
            _Account = GetMongoCollection();
        }
        private IMongoCollection<Account> GetMongoCollection()
        {
            // Set your MongoDB connection string and database name
            string connectionString =
                "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "Restaurant_Management_Application"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Account>("Account");
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedUsername = username.Text;
            var enteredPassword = password.Password;

            if (selectedUsername != null && !string.IsNullOrEmpty(enteredPassword))
            {
                var user = GetUser(selectedUsername);

                if (user == null)
                {
                    MessageBox.Show("Sai tên tài khoản hoặc mật khẩu.");
                    return;
                }

                if (enteredPassword == user.Password)
                {
                    // Password is valid, grant access

                    // You can open the main window or perform other actions here.
                    // Open the main window
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();

                    // Close the login window
                    this.Close();
                }
                else
                {
                    // Password is invalid
                    MessageBox.Show("Sai tên tài khoản hoặc mật khẩu.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
            }
        }

        private Account GetUser(string username)
        {
            var filter = Builders<Account>.Filter.Eq(u => u.Username, username);
            return _Account.Find(filter).FirstOrDefault();
        }
    }
}