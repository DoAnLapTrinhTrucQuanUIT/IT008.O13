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

        }

        void _ForgetPassword(LoginWindow p)
        {

        }
    }
}