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

namespace Restaurant_Management.ViewModels
{
    public class SettingsVM : Utilities.ViewModelBase
    {
        private string _Name;
        public string Name { get => _Name; set { _Name = value; OnPropertyChanged(); } }

        private string _Email;
        public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }

        private string _PhoneNumber;
        public string PhoneNumber { get => _PhoneNumber; set { _PhoneNumber = value; OnPropertyChanged(); } }

        private DateTime _DateofBirth;
        public DateTime DateofBirth { get => _DateofBirth; set { _DateofBirth = value; OnPropertyChanged(); } }

        private bool _Gender;
        public bool Gender { get => _Gender; set { _Gender = value; OnPropertyChanged(); } }

        private string _Address;
        public string Address { get => _Address; set { _Address = value; OnPropertyChanged(); } }

        private string _Image;
        public string Image { get => _Image; set { _Image = value; OnPropertyChanged(); } }

        public ICommand LogoutCommand { get; set; }
        public ICommand LoadWindowCommand { get; set; }
        public ICommand UpdateProfileCommand { get; set; }
        public ICommand ChangePassCommand { get; set; }
        public SettingsVM()
        {
            LogoutCommand = new RelayCommand<SettingsView>((p) => true, (p) => _Logout());
            LoadWindowCommand = new RelayCommand<SettingsView>((p) => true, (p) => _LoadWindow(p));
            UpdateProfileCommand = new RelayCommand<SettingsView>((p) => true, (p) => _UpdateProfile());
            ChangePassCommand = new RelayCommand<SettingsView>((p) => true, (p) => _ChangePass());
        }

        void _LoadWindow(SettingsView p)
        {

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
    }
}
