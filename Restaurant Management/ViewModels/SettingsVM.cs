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
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Windows.Media;

namespace Restaurant_Management.ViewModels
{
    public class SettingsVM : Utilities.ViewModelBase
    {
        private string _userID;
        public string UserID
        {
            get => _userID;
            set
            {
                _userID = value;
                OnPropertyChanged(nameof(UserID));
            }
        }
        
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
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

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private DateTime _dateofBirth;
        public DateTime DateOfBirth
        {
            get => _dateofBirth;
            set
            {
                _dateofBirth = value;
                OnPropertyChanged(nameof(DateOfBirth));
            }
        }

        private string _gender;
        public string Gender
        {
            get => _gender;
            set
            {
                _gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        private string _address;
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
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
        
        public ICommand BrowseImageButton { get; set; }
        
        public ICommand LogoutCommand { get; set; }
        
        public ICommand LoadWindowCommand { get; set; }
        
        public ICommand UpdateProfileCommand { get; set; }
        
        public ICommand ChangePassCommand { get; set; }

        private readonly IMongoCollection<Employees> _Employees;

        private IMongoCollection<Employees> GetMongoCollection()
        {
            string connectionString = "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/"; 
            
            string databaseName = "Restaurant_Management_Application";

            var client = new MongoClient(connectionString);
            
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Employees>("Employees");
        }

        public SettingsVM()
        {
            _Employees = GetMongoCollection();

            InitializeCommand();
        }
        
        private void InitializeCommand()
        {
            BrowseImageButton = new RelayCommand<SettingsView>((p) => true, (p) => _BrowseImage(p));
            
            LogoutCommand = new RelayCommand<SettingsView>((p) => true, (p) => _Logout(p));
            
            LoadWindowCommand = new RelayCommand<SettingsView>((p) => true, (p) => _LoadWindow(p));
            
            UpdateProfileCommand = new RelayCommand<SettingsView>((p) => true, (p) => _UpdateProfile());
            
            ChangePassCommand = new RelayCommand<SettingsView>((p) => true, (p) => _ChangePass());
        }

        private void _LoadWindow(SettingsView p)
        {
            string employeeId = Const.Instance.UserId;

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
                
                PhoneNumber = User.PhoneNumber;
                
                Gender = User.Gender;
                
                Address = User.Address;
                
                AvatarImageSource = User.AvatarImageSource;
            }
        }

        private void _Logout(SettingsView mainWindow)
        {
            LoginWindow loginView = new LoginWindow();

            loginView.Show();

            var window = Window.GetWindow(mainWindow);
            
            if (window != null)
            {
                window.Close();
            }
        }

        private void _ChangePass()
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

        private void _UpdateProfile()
        {
            UpdateProfileVM updateProfileVM = new UpdateProfileVM();

            updateProfileVM.EmployeeList = new ObservableCollection<Employees>
            {
                new Employees
                {
                    FullName = Name,
                    
                    Email = Email,
                    
                    DateOfBirth = DateOfBirth,
                    
                    PhoneNumber = PhoneNumber,
                    
                    Gender = Gender,
                    
                    Address = Address,
                }
            };

            UpdateProfile updateProfile = new UpdateProfile
            {
                DataContext = updateProfileVM
            };

            var window = new Window
            {
                Content = updateProfile,
               
                SizeToContent = SizeToContent.WidthAndHeight,
                
                WindowStyle = WindowStyle.None,
                
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            window.ShowDialog();
        }

        private void _BrowseImage(SettingsView parameter)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*",
                    
                    Title = "Select an image file"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    MessageBoxResult confirmReload = MessageBox.Show("Reload to update profile image", "Notification", MessageBoxButton.OKCancel);

                    if (confirmReload == MessageBoxResult.OK)
                    {
                        string imagePath = openFileDialog.FileName;

                        BitmapImage bitmapImage = new BitmapImage();

                        bitmapImage.BeginInit();

                        bitmapImage.UriSource = new Uri(imagePath);

                        bitmapImage.EndInit();

                        AvatarImageSource = bitmapImage;

                        UpdateAvatarInMongoDB(ConvertBitmapImageToByteArray(bitmapImage));

                        ReloadWindow();
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void UpdateAvatarInMongoDB(byte[] newAvatarBytes)
        {
            try
            {
                string employeeId = Const.Instance.UserId;

                var filter = Builders<Employees>.Filter.Eq(x => x.EmployeeId, employeeId);

                var updateDefinition = Builders<Employees>.Update.Set(x => x.Avatar, newAvatarBytes);

                _Employees.UpdateOne(filter, updateDefinition);

                MessageBox.Show("Avatar image updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating avatar image in MongoDB: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private byte[] ConvertBitmapImageToByteArray(BitmapImage bitmapImage)
        {
            if (bitmapImage == null)
                return null;

            using (MemoryStream stream = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder();

                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

                encoder.Save(stream);

                return stream.ToArray();
            }
        }
    }
}
