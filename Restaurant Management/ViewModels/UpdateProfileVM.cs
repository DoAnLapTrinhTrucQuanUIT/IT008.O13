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

        private IMongoCollection<Employees> GetEmployees()
        {
            string connectionString = "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/";

            string databaseName = "Restaurant_Management_Application";

            var client = new MongoClient(connectionString);

            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Employees>("Employees");
        }

        public UpdateProfileVM()
        {
            _Employees = GetEmployees();

            InitializeCommand();
        }
        private void InitializeCommand()
        {
            ConfirmCommand = new RelayCommand<UpdateProfile>((p) => true, (p) => _ConfirmCommand(p));
            
            CloseWDCM = new RelayCommand<UpdateProfile>((p) => true, (p) => _CloseWD(p));
            
            MinimizeWDCM = new RelayCommand<UpdateProfile>((p) => true, (p) => _MinimizeWD(p));
            
            MoveWDCM = new RelayCommand<UpdateProfile>((p) => true, (p) => _MoveWD(p));
        }
        
        private void _ConfirmCommand(UpdateProfile paramater)
        {
            MessageBoxResult updateProfileNotification = System.Windows.MessageBox.Show("Do you want to update profile ?", "Notification", MessageBoxButton.YesNo);
            
            if (updateProfileNotification == MessageBoxResult.Yes)
            {
                var updateDefinitionBuilder = Builders<Employees>.Update;
            
                var updateDefinition = updateDefinitionBuilder.Set(x => x.EmployeeId, Const.Instance.UserId); // Assuming EmployeeId is not editable

                string newName = paramater.Name.Text;
                
                string newEmail = paramater.Email.Text;
                
                string newPhoneNumber = paramater.PhoneNumber.Text;
                
                string newDateOfBirthString = paramater.DateOfBirth.Text; 
                
                string newGender = paramater.GenderComboBox.Text;
                
                string newAddress = paramater.Address.Text;

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

                var filter = Builders<Employees>.Filter.Eq(x => x.EmployeeId, Const.Instance.UserId);
                
                var result = _Employees.UpdateOne(filter, updateDefinition);

                if (result.IsModifiedCountAvailable && result.ModifiedCount > 0)
                {
                    MessageBox.Show("Profile updated successfully!", "Notification");

                    var window = Window.GetWindow(paramater);
                    
                    if (window != null)
                    {
                        window.Close();
                    }

                    ReloadWindow();
                }
                else
                {
                    MessageBox.Show("Failed to update profile.", "Notification");
                }
            }
        }

        private void ReloadWindow()
        {
            var navigationVM = (App.Current.MainWindow?.DataContext as NavigationVM);

            var currentUser = navigationVM?.CurrentUser;

            var newMainWindow = new MainWindow();

            if (newMainWindow.DataContext is NavigationVM newNavigationVM)
            {
                newNavigationVM.CurrentUser = currentUser;

                newNavigationVM.Decentralization(newMainWindow);
            }

            newMainWindow.Show();

            App.Current.MainWindow?.Close();

            App.Current.MainWindow = newMainWindow;
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
