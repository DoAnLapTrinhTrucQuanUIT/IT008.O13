using Restaurant_Management.Utilities;
using Restaurant_Management.Views.Component;
using Restaurant_Management.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Security.Policy;
using Restaurant_Management.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using MongoDB.Driver;
using System.IO;
using System.Windows.Media.Imaging;

namespace Restaurant_Management.ViewModels
{
    public class SettingsVM : Utilities.ViewModelBase
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
            }
        }

        private string _isAdmin;
        public string IsAdmin
        {
            get => _isAdmin;
            set
            {
                _isAdmin = value;
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
            }
        }

        private DateTime _dateofBirth;
        public DateTime DateOfBirth
        {
            get => _dateofBirth;
            set
            {
                _dateofBirth = value;
            }
        }

        private string _gender;
        public string Gender
        {
            get => _gender;
            set
            {
                _gender = value;
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
            }
        }

        private string _address;
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
            }
        }
        private BitmapImage _avatarimagesource;
        public BitmapImage AvatarImageSource
        {
            get => _avatarimagesource;
            set
            {
                _avatarimagesource = value;
            }
        }

        public ICommand LogoutCommand { get; set; }
        public ICommand LoadWindowCommand { get; set; }
        public ICommand UpdateProfileCommand { get; set; }
        public ICommand ChangePassCommand { get; set; }

        private readonly IMongoCollection<Employees> _Employees;
        public SettingsVM()
        {
            _Employees = GetMongoCollection();
            LogoutCommand = new RelayCommand<SettingsView>((p) => true, (p) => _Logout());
            LoadWindowCommand = new RelayCommand<SettingsView>((p) => true, (p) => _LoadWindow(p));
            UpdateProfileCommand = new RelayCommand<SettingsView>((p) => true, (p) => _UpdateProfile());
            ChangePassCommand = new RelayCommand<SettingsView>((p) => true, (p) => _ChangePass());
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
        void _LoadWindow(SettingsView p)
        {
            string employeeId = Const.UserID;

            var filter = Builders<Employees>.Filter.Eq(x => x.EmployeeId, employeeId);
            var User = _Employees.Find(filter).FirstOrDefault();

            if (User != null)
            {
                Name = User.FullName;
                switch (User.IsAdmin)
                {
                    case true:
                        IsAdmin = "Admin";
                        break;
                    case false:
                        IsAdmin = "Employee";
                        break;
                }
                Email = User.Email;
                DateOfBirth = User.DateOfBirth;
                Gender = User.Gender;
                Address = User.Address;
                AvatarImageSource = User.AvatarImageSource;
            }
        }
        void _Logout()
        {
            LoginWindow login = new LoginWindow();
            var window = new Window
            {
                Content = login,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStyle = WindowStyle.None,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();
        }


        void _UpdateProfile()
        {
            UpdateProfile updateProfile = new UpdateProfile();
            var window = new Window
            {
                Content = updateProfile,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStyle = WindowStyle.None,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();
        }

        void _ChangePass()
        {
            ChangePassword changePassword = new ChangePassword();
            var window = new Window
            {
                Content = changePassword,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStyle = WindowStyle.None,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();
        }

        private BitmapImage ConvertByteArrayToBitmapImage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
                return null;

            BitmapImage bitmapImage = new BitmapImage();
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                stream.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
            }

            return bitmapImage;
        }
    }
}
