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
using System.Windows.Input;
using System.Windows;

namespace Restaurant_Management.ViewModels
{
    public class ChangePasswordVM : Utilities.ViewModelBase
    {
        private string _oldPassword;
        public string OldPassword
        {
            get { return _oldPassword; }
            set
            {
                _oldPassword = value;
                OnPropertyChanged(nameof(OldPassword));
            }
        }

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
        
        public ICommand CancelCommand { get; set; }
        
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

        public ChangePasswordVM()
        {
            _Employees = GetEmployees();

            InitializeCommand();

            OldPassword = string.Empty;
        }

        private void InitializeCommand()
        {
            CancelCommand = new RelayCommand<ChangePassword>((p) => true, (p) => _CancelCommand(p));
            
            ConfirmCommand = new RelayCommand<ChangePassword>((p) => true, (p) => _ConfirmCommand(p));
            
            CloseWDCM = new RelayCommand<ChangePassword>((p) => true, (p) => _CloseWD(p));
            
            MinimizeWDCM = new RelayCommand<ChangePassword>((p) => true, (p) => _MinimizeWD(p));
            
            MoveWDCM = new RelayCommand<ChangePassword>((p) => true, (p) => _MoveWD(p));
        }

        private void _CancelCommand(ChangePassword parameter)
        {
            parameter.OldPassword.Clear();
            
            parameter.NewPassword.Clear();
        }

        private void _ConfirmCommand(ChangePassword paramater)
        {
            string oldPassword = paramater.OldPassword.Text;
            
            string newPassword = paramater.NewPassword.Text;

            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("You did not enter enough information!", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (VerifyOldPassword(oldPassword))
            {
                var updateDefinitionBuilder = Builders<Employees>.Update;
            
                var updateDefinition = updateDefinitionBuilder.Set(x => x.EmployeeId, Const.Instance.UserId)
                                                             .Set(x => x.Password, newPassword);

                var filter = Builders<Employees>.Filter.Eq(x => x.EmployeeId, Const.Instance.UserId);
                
                var result = _Employees.UpdateOne(filter, updateDefinition);

                if (result.IsModifiedCountAvailable && result.ModifiedCount > 0)
                {
                    MessageBox.Show("Password updated successfully!", "Notification");
                }
                else
                {
                    MessageBox.Show("Failed to update password.", "Notification");
                }

                var window = Window.GetWindow(paramater);
                
                if (window != null)
                {
                    window.Close();
                }
            }
            else
            {
                MessageBox.Show("Old password is incorrect.", "Notification");
            }
        }

        private bool VerifyOldPassword(string oldPassword)
        {
            var filter = Builders<Employees>.Filter.Eq(x => x.EmployeeId, Const.Instance.UserId) & Builders<Employees>.Filter.Eq(x => x.Password, oldPassword);
            
            var employee = _Employees.Find(filter).FirstOrDefault();
            
            return employee != null;
        }

        private void _CloseWD(ChangePassword parameter)
        {
            var window = Window.GetWindow(parameter);
            
            if (window != null)
            {
                window.Close();
            }
        }
        
        private void _MinimizeWD(ChangePassword parameter)
        {
            var window = Window.GetWindow(parameter);
            
            if (window != null)
            {
                WindowState originalWindowState = window.WindowState;
 
                window.WindowState = WindowState.Minimized;
            }
        }

        private void _MoveWD(ChangePassword parameter)
        {
            var window = Window.GetWindow(parameter);
           
            if (window != null)
            {
                window.DragMove();
            }
        }
    }
}
