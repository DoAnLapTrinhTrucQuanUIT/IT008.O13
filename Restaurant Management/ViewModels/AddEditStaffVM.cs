using System.Windows;
using System.Windows.Input;
using Restaurant_Management.Models;
using MongoDB.Driver;
using Restaurant_Management.Utilities;
using Restaurant_Management.Views.Component;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System;
using Microsoft.Win32;

namespace Restaurant_Management.ViewModels
{
    public class AddEditStaffVM : ViewModelBase
    {
        private readonly IMongoCollection<Employees> _Employees;

        public AddEditStaffVM()
        {
            _Employees = GetStaffCollection();
            BrowseImageCommand = new RelayCommand<Add_EditStaff>(p => true, p => _BrowseImage(p));
            CancelCommand = new RelayCommand<Add_EditStaff>(p => true, p => _CancelCommand(p));
            ConfirmCommand = new RelayCommand<Add_EditStaff>(p => true, p => _ConfirmCommand(p));
            CloseWDCM = new RelayCommand<Add_EditStaff>(p => true, p => _CloseWD(p));
            MinimizeWDCM = new RelayCommand<Add_EditStaff>(p => true, p => _MinimizeWD(p));
            MoveWDCM = new RelayCommand<Add_EditStaff>(p => true, p => _MoveWD(p));
        }

        public ICommand CancelCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public ICommand CloseWDCM { get; set; }
        public ICommand MinimizeWDCM { get; set; }
        public ICommand MoveWDCM { get; set; }
        public ICommand BrowseImageCommand { get; set; }

        private IMongoCollection<Employees> GetStaffCollection()
        {
            // Set your MongoDB connection string and database name
            string connectionString =
                "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "Restaurant_Management_Application"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Employees>("Employees");
        }

        private void _BrowseImage(Add_EditStaff parameter)
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
                    string imagePath = openFileDialog.FileName;

                    // Load the image and set it to the Image control
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(imagePath);
                    bitmapImage.EndInit();
                    parameter.iconBrowseImage.Visibility = Visibility.Hidden;
                    parameter.loadedImage.Source = bitmapImage;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void _CancelCommand(Add_EditStaff parameter)
        {
            // Implement cancel logic if needed
        }

        private void _ConfirmCommand(Add_EditStaff parameter)
        {
            // Implement confirmation logic if needed
        }

        private void _CloseWD(Add_EditStaff parameter)
        {
            var window = Window.GetWindow(parameter);
            if (window != null)
            {
                window.Close();
            }
        }

        private void _MinimizeWD(Add_EditStaff parameter)
        {
            var window = Window.GetWindow(parameter);
            if (window != null)
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        private void _MoveWD(Add_EditStaff parameter)
        {
            var window = Window.GetWindow(parameter);
            if (window != null && Mouse.LeftButton == MouseButtonState.Pressed)
            {
                window.DragMove();
            }
        }
    }
}
