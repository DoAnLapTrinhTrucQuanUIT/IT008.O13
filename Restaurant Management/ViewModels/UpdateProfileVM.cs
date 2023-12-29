using MongoDB.Driver;
using Restaurant_Management.Models;
using Restaurant_Management.Utilities;
using Restaurant_Management.Views.Component;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Restaurant_Management.ViewModels
{
    public class UpdateProfileVM : Utilities.ViewModelBase
    {
        private ObservableCollection<Employees> _employeeList;
        public ObservableCollection<Employees> EmployeeList
        {
            get { return _employeeList; }
            set
            {
                _employeeList = value;
                OnPropertyChanged(nameof(EmployeeList));
            }
        }

        public ICommand ConfirmCommand { get; set; }
        public ICommand CloseWDCM { get; set; }
        public ICommand MinimizeWDCM { get; set; }
        public ICommand MoveWDCM { get; set; }

        private readonly IMongoCollection<Employees> _Employees;

        public IMongoCollection<Employees> Employees;
        public UpdateProfileVM()
        {
            _Employees = GetEmployees();
            ConfirmCommand = new RelayCommand<UpdateProfile>((p) => true, (p) => _ConfirmCommand(p));
            CloseWDCM = new RelayCommand<UpdateProfile>((p) => true, (p) => _CloseWD(p));
            MinimizeWDCM = new RelayCommand<UpdateProfile>((p) => true, (p) => _MinimizeWD(p));
            MoveWDCM = new RelayCommand<UpdateProfile>((p) => true, (p) => _MoveWD(p));
        }
        private IMongoCollection<Employees> GetEmployees()
        {
            // Set your MongoDB connection string and database name
            string connectionString =
                "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "Restaurant_Management_Application"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Employees>("Employees");
        }
        void _ConfirmCommand(UpdateProfile paramater)
        {
            MessageBoxResult updateProfileNotification = System.Windows.MessageBox.Show("Do you want to update profile ?", "Notification", MessageBoxButton.YesNoCancel);
            if (updateProfileNotification == MessageBoxResult.Yes)
            {
                var updateDefinitionBuilder = Builders<Employees>.Update;
                var updateDefinition = updateDefinitionBuilder.Set(x => x.EmployeeId, Const.Instance.UserId); // Assuming EmployeeId is not editable

                // Lấy thông tin mới từ giao diện người dùng
                string newName = paramater.Name.Text;
                string newEmail = paramater.Email.Text;
                string newPhoneNumber = paramater.PhoneNumber.Text;
                string newDateOfBirthString = paramater.DateOfBirth.Text; // Assuming the date is in a string format
                string newGender = paramater.GenderComboBox.Text;
                string newAddress = paramater.Address.Text;

                // Thêm các trường mà người dùng muốn cập nhật
                if (!string.IsNullOrEmpty(newName))
                    updateDefinition = updateDefinition.Set(x => x.FullName, newName);

                if (!string.IsNullOrEmpty(newEmail))
                    updateDefinition = updateDefinition.Set(x => x.Email, newEmail);

                if (!string.IsNullOrEmpty(newPhoneNumber))
                    updateDefinition = updateDefinition.Set(x => x.PhoneNumber, newPhoneNumber);


                if (!string.IsNullOrEmpty(newDateOfBirthString) && DateTime.TryParse(newDateOfBirthString, out DateTime newDateOfBirth))
                    updateDefinition = updateDefinition.Set(x => x.DateOfBirth, newDateOfBirth);

                if (!string.IsNullOrEmpty(newGender))
                    updateDefinition = updateDefinition.Set(x => x.Gender, newGender);

                if (!string.IsNullOrEmpty(newAddress))
                    updateDefinition = updateDefinition.Set(x => x.Address, newAddress);

                // Cập nhật đối tượng nhân viên trong MongoDB
                var filter = Builders<Employees>.Filter.Eq(x => x.EmployeeId, Const.Instance.UserId);
                var result = _Employees.UpdateOne(filter, updateDefinition);

                if (result.IsModifiedCountAvailable && result.ModifiedCount > 0)
                {
                    MessageBox.Show("Profile updated successfully!", "Notification");
                    var mainWindow = App.Current.MainWindow;
                    App.Current.MainWindow = null;
                    mainWindow.Close();

                    // Tạo mới một MainWindow và hiển thị nó
                    var newMainWindow = new MainWindow();
                    App.Current.MainWindow = newMainWindow;
                    newMainWindow.Show();
                }
                else
                {
                    MessageBox.Show("Failed to update profile.", "Notification");
                }

                var window = Window.GetWindow(paramater);
                if (window != null)
                {
                    window.Close();
                }
            }
        }
        private UpdateDefinition<Employees> UpdateFieldIfNotEmpty<T>(Expression<Func<Employees, T>> field, T value)
        {
            return Builders<Employees>.Update.Set(field, value);
        }

        private UpdateDefinition<Employees> UpdateFieldIfNotNull<T>(Expression<Func<Employees, T>> field, T value)
        {
            return value != null ? Builders<Employees>.Update.Set(field, value) : null;
        }

        private void _CloseWD(UpdateProfile parameter)
        {
            var window = Window.GetWindow(parameter);
            if (window != null)
            {
                window.Close();
            }
        }
        private void _MinimizeWD(UpdateProfile parameter)
        {
            var window = Window.GetWindow(parameter);
            if (window != null)
            {
                WindowState originalWindowState = window.WindowState;
                window.WindowState = WindowState.Minimized;
            }
        }

        private void _MoveWD(UpdateProfile parameter)
        {
            var window = Window.GetWindow(parameter);
            if (window != null)
            {
                window.DragMove();
            }
        }
    }
}
