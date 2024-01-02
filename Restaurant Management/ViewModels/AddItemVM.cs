using System;
using System.IO;
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
using System.Windows.Media;
using System.Drawing;
using System.Windows.Controls;
using System.Diagnostics;
using System.Xml.Linq;
using System.Data.Common;
using System.Globalization;
using iTextSharp.text;

namespace Restaurant_Management.ViewModels
{
    public class AddItemVM : Utilities.ViewModelBase
    {
        public ICommand CancelCommand { get; set; }
        
        public ICommand ConfirmCommand { get; set; }
        
        public ICommand CloseWDCM { get; set; }
        
        public ICommand MinimizeWDCM { get; set; }
        
        public ICommand MoveWDCM { get; set; }
        
        public ICommand BrowseImageCommand { get; set; }

        private readonly IMongoCollection<MenuItems> _MenuItems;

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

        public AddItemVM()
        {
            _MenuItems = GetMenuItems();

            InitializeCommand();
        }

        private void InitializeCommand()
        {
            BrowseImageCommand = new RelayCommand<AddItem>(p => true, p => _BrowseImage(p));
            
            CancelCommand = new RelayCommand<AddItem>(p => true, p => _CancelCommand(p));
            
            ConfirmCommand = new RelayCommand<AddItem>(p => true, p => _ConfirmCommand(p));
            
            CloseWDCM = new RelayCommand<AddItem>(p => true, p => _CloseWD(p));
            
            MinimizeWDCM = new RelayCommand<AddItem>(p => true, p => _MinimizeWD(p));
            
            MoveWDCM = new RelayCommand<AddItem>(p => true, p => _MoveWD(p));
        }

        private byte[] ConvertImageToBytes(System.Windows.Controls.Image image)
        {
            // Convert the WPF Image to a MemoryStream
            using (MemoryStream ms = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image.Source));
                encoder.Save(ms);

                // Get the bytes from the MemoryStream
                return ms.ToArray();
            }
        }
        
        private string GenerateRandomItemId()
        {
            var maxItemId = _MenuItems.AsQueryable()
                .OrderByDescending(c => c.ItemId)
                .FirstOrDefault()?.ItemId;

            string newItemId = GenerateNextItemId(maxItemId);

            while (Check(newItemId))
            {
                newItemId = GenerateNextItemId(newItemId);
            }

            return newItemId;
        }
        
        private string GenerateNextItemId(string currentMaxItemId)
        {
            if (string.IsNullOrEmpty(currentMaxItemId))
            {
                return "DISHES1";
            }

            string maxNumberStr = currentMaxItemId.Substring(6);
            if (int.TryParse(maxNumberStr, out int maxNumber))
            {
                // Tạo `CustomerId` mới với số thứ tự kế tiếp
                return "DISHES" + (maxNumber + 1).ToString();
            }

            return "DISHES1";
        }
        
        private bool Check(string itemId)
        {
            return _MenuItems.AsQueryable().Any(temp => temp.ItemId == itemId);
        }

        private void _BrowseImage(AddItem parameter)
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

        private void _CancelCommand(AddItem parameter)
        {
            parameter.Name.Clear();
            parameter.CategoryComboBox.SelectedItem = null;
            parameter.Price.Clear();
            parameter.Description.Clear();
            parameter.loadedImage.Source = null;
        }

        private void _ConfirmCommand(AddItem parameter)
        {
            if (parameter.Name.Text == "" || parameter.CategoryComboBox.SelectedItem == null || parameter.Price.Text == "")
            {
                MessageBox.Show("You did not enter enough information!", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBoxResult addCusNoti = System.Windows.MessageBox.Show("Do you want to add the item?", "Notification", MessageBoxButton.YesNoCancel);
            if (addCusNoti == MessageBoxResult.Yes)
            {
                if (string.IsNullOrEmpty(parameter.Name.Text) || string.IsNullOrEmpty(parameter.Price.Text) || string.IsNullOrEmpty(parameter.CategoryComboBox.Text))
                {
                    MessageBox.Show("Incomplete information!", "Notification");
                }
                else
                {
                    var filter = Builders<MenuItems>.Filter.Eq(x => x.Name, parameter.Name.Text);
                    var User = _MenuItems.Find(filter).FirstOrDefault();
                    if (User != null)
                    {
                        MessageBox.Show("Item already exists!", "Notification");
                    }
                    else
                    {
                        AddItem(parameter);
                        var window = Window.GetWindow(parameter);
                        if (window != null)
                        {
                            window.Close();
                        }
                    }
                }
            }
        }

        private void AddItem(AddItem parameter)
        {
            // byte[] imageBytes = ConvertImageToBytes(parameter.loadedImage);

            // Tạo một đối tượng MenuItems mới
            var menuItems = new MenuItems
            {
                ItemId = GenerateRandomItemId(),
                Name = parameter.Name.Text.ToString(),
                Category = parameter.CategoryComboBox.Text.ToString(),
                Price = Double.Parse(parameter.Price.Text, CultureInfo.InvariantCulture),
                Description = parameter.Description.Text.ToString(),
                Image = null
            };
            // Thêm đối tượng vào collection
            _MenuItems.InsertOne(menuItems);

            MessageBox.Show("Item added successfully.", "Notification");
            parameter.Name.Clear();
            parameter.CategoryComboBox.SelectedItem = null;
            parameter.Price.Clear();
            parameter.Description.Clear();
            // Clear the image in the loadedImage control
            parameter.loadedImage.Source = null;
        }

        private void _CloseWD(AddItem parameter)
        {
            var window = Window.GetWindow(parameter);
            if (window != null)
            {
                window.Close();
            }
        }

        private void _MinimizeWD(AddItem parameter)
        {
            var window = Window.GetWindow(parameter);
            if (window != null)
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        private void _MoveWD(AddItem parameter)
        {
            var window = Window.GetWindow(parameter);
            if (window != null && Mouse.LeftButton == MouseButtonState.Pressed)
            {
                window.DragMove();
            }
        }
    }
}
