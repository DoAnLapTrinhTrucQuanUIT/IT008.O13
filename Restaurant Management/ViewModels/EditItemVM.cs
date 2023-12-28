using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Restaurant_Management.Models;
using Restaurant_Management.Utilities;
using Restaurant_Management.Views;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Windows.Documents;
using Restaurant_Management.Views.Component;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Media.Imaging;

namespace Restaurant_Management.ViewModels
{
    public class EditItemVM : Utilities.ViewModelBase
    {
        private MenuItems _menuItem;
        public MenuItems MenuItem
        {
            get { return _menuItem; }
            set
            {
                _menuItem = value;
                OnPropertyChanged(nameof(MenuItem));
            }
        }
        public ICommand CancelCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public ICommand LoadWDCM { get; set; }
        public ICommand CloseWDCM { get; set; }
        public ICommand MinimizeWDCM { get; set; }
        public ICommand MoveWDCM { get; set; }
        public ICommand BrowseImageCommand { get; set; }

        private readonly IMongoCollection<MenuItems> _MenuItems;

        public EditItemVM(MenuItems menuItem)
        {
            MenuItem = menuItem;
            _MenuItems = GetMenuItems();
            LoadWDCM = new RelayCommand<EditItem>(p => true, p => _LoadWinDow(p));
            BrowseImageCommand = new RelayCommand<EditItem>(p => true, p => _BrowseImage(p));
            CancelCommand = new RelayCommand<EditItem>(p => true, p => _CancelCommand(p));
            ConfirmCommand = new RelayCommand<EditItem>(p => true, p => _ConfirmCommand(p));
            CloseWDCM = new RelayCommand<EditItem>(p => true, p => _CloseWD(p));
            MinimizeWDCM = new RelayCommand<EditItem>(p => true, p => _MinimizeWD(p));
            MoveWDCM = new RelayCommand<EditItem>(p => true, p => _MoveWD(p));
        }

        private IMongoCollection<MenuItems> GetMenuItems()
        {
            // Set your MongoDB connection string and database name
            string connectionString =
                "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "Restaurant_Management_Application"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<MenuItems>("MenuItems");
        }

        private void _LoadWinDow(EditItem item)
        {
            if (item != null)
            {
                item.Name.Text = MenuItem.Name.ToString();
                item.CategoryComboBox.Text = MenuItem.Category;
                item.Price.Text = MenuItem.Price.ToString();
                item.Description.Text = MenuItem.Description.ToString();
                item.loadedImage.Source = ConvertByteArrayToImageSource(MenuItem.Image);
            }
        }
        private System.Windows.Media.ImageSource ConvertByteArrayToImageSource(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
                return null;

            try
            {
                using (MemoryStream stream = new MemoryStream(byteArray))
                {
                    BitmapImage imageSource = new BitmapImage();
                    imageSource.BeginInit();
                    imageSource.CacheOption = BitmapCacheOption.OnLoad;
                    imageSource.StreamSource = stream;
                    imageSource.EndInit();
                    return imageSource;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error converting byte array to ImageSource: " + ex.Message);
                return null;
            }
        }
        private void _BrowseImage(EditItem parameter)
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

                    // Kiểm tra null trước khi sử dụng
                    if (parameter != null && parameter.iconBrowseImage != null && parameter.loadedImage != null)
                    {
                        // Load the image and set it to the Image control
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.UriSource = new Uri(imagePath);
                        bitmapImage.EndInit();

                        // Ẩn iconBrowseImage và thiết lập nguồn ảnh cho loadedImage
                        parameter.iconBrowseImage.Visibility = Visibility.Hidden;
                        parameter.loadedImage.Source = bitmapImage;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void _CancelCommand(EditItem parameter)
        {
            if (parameter != null)
            {
                // Khôi phục lại giá trị ban đầu của các trường trong EditItem
                parameter.Name.Text = MenuItem.Name;
                parameter.CategoryComboBox.Text = MenuItem.Category;
                parameter.Price.Text = MenuItem.Price.ToString();
                parameter.Description.Text = MenuItem.Description;
                parameter.loadedImage.Source = ConvertByteArrayToImageSource(MenuItem.Image);

                // Đóng cửa sổ khi hủy bỏ lệnh sửa
                var window = Window.GetWindow(parameter);
                if (window != null)
                {
                    window.Close();
                }
            }
        }

        private void _ConfirmCommand(EditItem parameter)
        {
            try
            {
                if (parameter != null)
                {
                    // Kiểm tra xem người dùng đã nhập đủ thông tin hay chưa
                    if (string.IsNullOrEmpty(parameter.Name.Text) || string.IsNullOrEmpty(parameter.CategoryComboBox.Text) ||
                        string.IsNullOrEmpty(parameter.Price.Text) || string.IsNullOrEmpty(parameter.Description.Text) ||
                        parameter.loadedImage.Source == null)
                    {
                        MessageBox.Show("Please enter complete information and photo!", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Cập nhật thông tin của MenuItem từ giao diện
                    MenuItem.Name = parameter.Name.Text;
                    MenuItem.Category = parameter.CategoryComboBox.Text;

                    // Kiểm tra và chuyển đổi giá tiền
                    if (double.TryParse(parameter.Price.Text, out double price))
                    {
                        MenuItem.Price = price;
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid price!", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    MenuItem.Description = parameter.Description.Text;

                    // Chuyển đổi ảnh từ ImageSource sang byte array
                    MenuItem.Image = ConvertImageSourceToByteArray(parameter.loadedImage.Source);

                    // Create a filter based on the unique identifier (ItemId)
                    var filter = Builders<MenuItems>.Filter.Eq(x => x.ItemId, MenuItem.ItemId);

                    // Create an update with the fields you want to modify
                    var update = Builders<MenuItems>.Update
                        .Set(x => x.Name, MenuItem.Name)
                        .Set(x => x.Category, MenuItem.Category)
                        .Set(x => x.Price, MenuItem.Price)
                        .Set(x => x.Description, MenuItem.Description)
                        .Set(x => x.Image, MenuItem.Image);

                    // Apply the update to the MongoDB document without modifying the _id field
                    _MenuItems.UpdateOne(filter, update);

                    // Đóng cửa sổ khi cập nhật thành công
                    var window = Window.GetWindow(parameter);
                    if (window != null)
                    {
                        window.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error confirming update: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private byte[] ConvertImageSourceToByteArray(System.Windows.Media.ImageSource imageSource)
        {
            if (imageSource == null)
                return null;

            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create((BitmapSource)imageSource));
                    encoder.Save(stream);
                    return stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error converting ImageSource to byte array: " + ex.Message); return null;
            }
        }

        private void _CloseWD(EditItem parameter)
        {
            var window = Window.GetWindow(parameter);
            if (window != null && Mouse.LeftButton == MouseButtonState.Pressed)
            {
                window.Close();
            }
        }

        private void _MinimizeWD(EditItem parameter)
        {
            var window = Window.GetWindow(parameter);
            if (window != null && Mouse.LeftButton == MouseButtonState.Pressed)
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        private void _MoveWD(EditItem parameter)
        {
            var window = Window.GetWindow(parameter);
            if (window != null && Mouse.LeftButton == MouseButtonState.Pressed)
            {
                window.DragMove();
            }
        }
    }
}