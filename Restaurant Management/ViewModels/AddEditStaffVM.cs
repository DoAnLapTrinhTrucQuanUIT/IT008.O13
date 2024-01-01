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
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.IO;
using System.Windows.Media;

namespace Restaurant_Management.ViewModels
{
    public class AddEditStaffVM : ViewModelBase
    {
        private readonly IMongoCollection<Employees> _Employees;
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

        private readonly IMongoCollection<SalaryInformation> _SalaryInformation;
        private IMongoCollection<SalaryInformation> GetSalaryInformation()
        {
            // Set your MongoDB connection string and database name
            string connectionString =
                "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "Restaurant_Management_Application"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<SalaryInformation>("SalaryInformation");
        }

        public AddEditStaffVM()
        {
            _Employees = GetStaffCollection();
            _SalaryInformation = GetSalaryInformation();
            BrowseImageCommand = new RelayCommand<AddStaff>(p => true, p => _BrowseImage(p));
            CancelCommand = new RelayCommand<AddStaff>(p => true, p => _CancelCommand(p));
            ConfirmCommand = new RelayCommand<AddStaff>(p => true, p => _ConfirmCommand(p));
            CloseWDCM = new RelayCommand<AddStaff>(p => true, p => _CloseWD(p));
            MinimizeWDCM = new RelayCommand<AddStaff>(p => true, p => _MinimizeWD(p));
            MoveWDCM = new RelayCommand<AddStaff>(p => true, p => _MoveWD(p));
        }

        public ICommand CancelCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public ICommand CloseWDCM { get; set; }
        public ICommand MinimizeWDCM { get; set; }
        public ICommand MoveWDCM { get; set; }
        public ICommand BrowseImageCommand { get; set; }

        private void _BrowseImage(AddStaff parameter)
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

        private void _CancelCommand(AddStaff parameter)
        {
            parameter.FullName.Clear();
            parameter.Birthdate.SelectedDate = null;
            parameter.PhoneNumber.Clear();
            parameter.GenderComboBox.SelectedItem = null;
            parameter.DepartmentComboBox.SelectedItem = null;
            parameter.Email.Clear();
            parameter.Address.Clear();
            parameter.loadedImage.Source = null;
        }

        private void _ConfirmCommand(AddStaff parameter)
        {
            if (parameter.FullName.Text == "" || parameter.PhoneNumber.Text == "" || parameter.GenderComboBox.SelectedItem == null || parameter.DepartmentComboBox.SelectedItem == null || parameter.Address.Text == "" || parameter.Email.Text == "")
            {
                MessageBox.Show("You did not enter enough information!", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBoxResult addCusNoti = System.Windows.MessageBox.Show("Do you want to add staff?", "Notification", MessageBoxButton.YesNoCancel);
            if (addCusNoti == MessageBoxResult.Yes)
            {
                if (string.IsNullOrEmpty(parameter.FullName.Text) || string.IsNullOrEmpty(parameter.PhoneNumber.Text) || string.IsNullOrEmpty(parameter.GenderComboBox.Text) || string.IsNullOrEmpty(parameter.Address.Text) || string.IsNullOrEmpty(parameter.DepartmentComboBox.Text) || string.IsNullOrEmpty(parameter.Email.Text))
                {
                    MessageBox.Show("Incomplete information!", "Notification");
                }
                else
                {
                    var filter = Builders<Employees>.Filter.Eq(x => x.FullName, parameter.FullName.Text);
                    var User = _Employees.Find(filter).FirstOrDefault();
                    if (User != null)
                    {
                        MessageBox.Show("Employee already exists!", "Notification");
                    }
                    else
                    {
                        AddStaff(parameter);
                        var window = Window.GetWindow(parameter);
                        if (window != null)
                        {
                            window.Close();
                        }
                    }
                }
            }
        }

        private void AddStaff(AddStaff parameter)
        {
            // Kiểm tra xem ảnh có được chọn hay không
            byte[] imageBytes = null;
            if (parameter.loadedImage.Source != null)
            {
                // Nếu ảnh được chọn, chuyển đổi ảnh thành byte array
                imageBytes = ConvertImageToBytes(parameter.loadedImage);
            }

            // Tạo một đối tượng Employee mới
            var Employee = new Employees
            {
                EmployeeId = GenerateRandomStaffId((parameter.DepartmentComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() == "Owner"),
                FullName = parameter.FullName.Text.ToString(),
                DateOfBirth = parameter.Birthdate.SelectedDate.Value,
                PhoneNumber = parameter.PhoneNumber.Text.ToString(),
                Gender = parameter.GenderComboBox.Text.ToString(),
                Address = parameter.Address.Text.ToString(),
                Email = parameter.Email.Text.ToString(),
                Avatar = imageBytes, // Ảnh có thể là null
                DateOfJoining = DateTime.Now,
                Password = "123", // Đổi thành chuỗi
                IsActive = true,
                IsAdmin = (parameter.DepartmentComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() == "Owner"
            };

            // Thêm đối tượng vào collection
            _Employees.InsertOne(Employee);
            MessageBox.Show("Employee added successfully.", "Notification");

            // Thêm SalaryInformation
            var salaryInformation = new SalaryInformation
            {
                Employees = Employee,
                StartDate = DateTime.Now,
                PayDate = DateTime.Now.AddDays(30),
                WorkedDays = 0,
                BasicSalary = 0
            };
            _SalaryInformation.InsertOne(salaryInformation);

            // Xóa thông tin trên giao diện
            parameter.FullName.Clear();
            parameter.Birthdate.SelectedDate = null;
            parameter.PhoneNumber.Clear();
            parameter.GenderComboBox.SelectedItem = null;
            parameter.DepartmentComboBox.SelectedItem = null;
            parameter.Email.Clear();
            parameter.Address.Clear();
            parameter.loadedImage.Source = null; // Đặt ảnh về null
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

        private string GenerateRandomStaffId(bool isAdmin)
        {
            // Lấy `CustomerId` lớn nhất hiện có
            var maxCustomerId = _Employees.AsQueryable()
                .OrderByDescending(c => c.EmployeeId)
                .FirstOrDefault()?.EmployeeId;

            // Tạo `CustomerId` mới với số thứ tự kế tiếp
            string newCustomerId = GenerateNextStaffId(maxCustomerId, isAdmin);

            // Kiểm tra nếu `CustomerId` mới đã tồn tại
            while (Check(newCustomerId))
            {
                newCustomerId = GenerateNextStaffId(newCustomerId, isAdmin);
            }

            return newCustomerId;
        }

        private string GenerateNextStaffId(string currentMaxStaffId, bool isAdmin)
        {
            if (isAdmin)
            {
                if (string.IsNullOrEmpty(currentMaxStaffId) || !currentMaxStaffId.StartsWith("ADMIN"))
                {
                    return "ADMIN1";
                }

                // Trích xuất số từ `CustomerId` lớn nhất hiện có
                string maxNumberStr = currentMaxStaffId.Substring(5);
                if (int.TryParse(maxNumberStr, out int maxNumber))
                {
                    // Tạo `CustomerId` mới với số thứ tự kế tiếp
                    return "ADMIN" + (maxNumber + 1).ToString();
                }
            }
            else
            {
                if (string.IsNullOrEmpty(currentMaxStaffId) || !currentMaxStaffId.StartsWith("EMP"))
                {
                    return "EMP1";
                }

                // Trích xuất số từ `CustomerId` lớn nhất hiện có
                string maxNumberStr = currentMaxStaffId.Substring(3);
                if (int.TryParse(maxNumberStr, out int maxNumber))
                {
                    // Tạo `CustomerId` mới với số thứ tự kế tiếp
                    return "EMP" + (maxNumber + 1).ToString();
                }
            }

            // Trong trường hợp không thành công, trả về `EMP1`
            return "EMP1";
        }

        bool Check(string employeeId)
        {
            return _Employees.AsQueryable().Any(temp => temp.EmployeeId == employeeId);
        }

        private void _CloseWD(AddStaff parameter)
        {
            var window = Window.GetWindow(parameter);
            if (window != null)
            {
                window.Close();
            }
        }

        private void _MinimizeWD(AddStaff parameter)
        {
            var window = Window.GetWindow(parameter);
            if (window != null)
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        private void _MoveWD(AddStaff parameter)
        {
            var window = Window.GetWindow(parameter);
            if (window != null && Mouse.LeftButton == MouseButtonState.Pressed)
            {
                window.DragMove();
            }
        }
    }
}
