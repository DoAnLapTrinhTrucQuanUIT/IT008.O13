using System;
using MongoDB.Driver;
using Restaurant_Management.Models;
using Restaurant_Management.Utilities;
using Restaurant_Management.Views;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows;
using Restaurant_Management.Views.Component;
using System.Xml.Linq;

namespace Restaurant_Management.ViewModels
{
    public class StaffInformationVM : Utilities.ViewModelBase
    {
        private string _fullname;
        public string FullName
        {
            get => _fullname;
            set
            {
                _fullname = value;
                OnPropertyChanged(nameof(FullName));
            }
        }

        private string _isAdmin;
        public string IsAdmin
        {
            get => _isAdmin;
            set
            {
                _isAdmin = value;
                OnPropertyChanged(nameof(IsAdmin));
            }
        }

        private BitmapImage _avatarimagesource;
        public BitmapImage AvatarImageSource
        {
            get => _avatarimagesource;
            set
            {
                _avatarimagesource = value;
                OnPropertyChanged(nameof(AvatarImageSource));
            }
        }
        public ICommand LoadWindowCommand { get; set; }

        private readonly IMongoCollection<Employees> _Employees;

        public StaffInformationVM()
        {
            _Employees = GetMongoCollection();
            LoadWindowCommand = new RelayCommand<NavigationBar>((p) => true, (p) => _LoadWindow());
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
        void _LoadWindow()
        {
            string employeeId = Const.Instance.UserId;

            var filter = Builders<Employees>.Filter.Eq(x => x.EmployeeId, employeeId);
            var User = _Employees.Find(filter).FirstOrDefault();

            if (User != null)
            {
                FullName = User.FullName;
                switch (User.IsAdmin)
                {
                    case true:
                        IsAdmin = "Admin";
                        break;
                    case false:
                        IsAdmin = "Employee";
                        break;
                }
                AvatarImageSource = User.AvatarImageSource;
            }
        }
    }
}
