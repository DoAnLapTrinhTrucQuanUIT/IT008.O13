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
using System.Threading.Tasks;

namespace Restaurant_Management.ViewModels
{
    public class LoginVM : Utilities.ViewModelBase
    {
        private bool _isAdmin;
        public bool IsAdmin
        {
            get { return _isAdmin; }
            set
            {
                _isAdmin = value;
                OnPropertyChanged(nameof(IsAdmin));
            }
        }

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

        public ICommand CloseWDCM { get; set; }

        public ICommand MinimizeWDCM { get; set; }

        public ICommand MoveWDCM { get; set; }

        private Employees GetUser(string employeeId)
        {
            return _Employees.Find(u => u.EmployeeId == employeeId).FirstOrDefault();
        }

        private readonly IMongoCollection<Employees> _Employees;

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
        
        public LoginVM()
        {
            _Employees = GetMongoCollection();

            InitializeUser();

            InitializeCommand();
        }

        private void InitializeUser()
        {
            IsLogin = false;

            Password = "";
            
            EmployeeId = "";
        }

        private void InitializeCommand()
        {
            LoginCM = new RelayCommand<LoginWindow>((p) => true, (p) => _Login(p));
            
            PasswordchangeCM = new RelayCommand<PasswordBox>((p) => true, (p) => { Password = p.Password; });
            
            ForgetpasswordCM = new RelayCommand<LoginWindow>((p) => true, (p) => _ForgetPassword());

            CloseWDCM = new RelayCommand<LoginWindow>((p) => true, (p) => _CloseWD(p));
            
            MinimizeWDCM = new RelayCommand<LoginWindow>((p) => true, (p) => _MinimizeWD(p));
            
            MoveWDCM = new RelayCommand<LoginWindow>((p) => true, (p) => _MoveWD(p));
        }

        private void _Login(LoginWindow p)
        {
            
            if (EmployeeId != null && !string.IsNullOrEmpty(Password))
            {
                var user = GetUser(EmployeeId);

                if (user == null || Password != user.Password)
                {
                    MessageBox.Show("Wrong account or password name !");
                    return;
                }

                IsAdmin = user.IsAdmin;

                Const.Instance.SetUser(EmployeeId);

                Task.Run(() =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MainWindow mainWindow = new MainWindow();

                        App.Current.MainWindow = mainWindow;

                        if (Application.Current.MainWindow.DataContext is NavigationVM navigationVM)
                        {
                            navigationVM.CurrentUser = user;
                        }

                        mainWindow.Show();
                    });

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        IsLogin = true;
                        
                        p.Close();
                    });
                });
                
            }
            else
            {
                MessageBox.Show("Please enter completely information !");
            }
        }
   
        private void _ForgetPassword()
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
        
        private void _CloseWD(LoginWindow parameter)
        {
            var window = Window.GetWindow(parameter);
            
            if (window != null)
            {
                window.Close();
            }
        }
        
        private void _MinimizeWD(LoginWindow parameter)
        {
            var window = Window.GetWindow(parameter);
            
            if (window != null)
            {
                WindowState originalWindowState = window.WindowState;
                
                window.WindowState = WindowState.Minimized;
            }
        }

        private void _MoveWD(LoginWindow parameter)
        {
            var window = Window.GetWindow(parameter);
            
            if (window != null)
            {
                window.DragMove();
            }
        }
    }
}
