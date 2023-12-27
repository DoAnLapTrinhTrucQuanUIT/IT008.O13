using System;
using System.IO;
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
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Restaurant_Management.ViewModels
{
    public class StaffVM : Utilities.ViewModelBase
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

        public ICommand SearchCM { get; set; }
        public ICommand AddStaffCM { get; set; }
        public ICommand ExportStaffCM { get; set; }
        public ICommand ImportStaffCM { get; set; }

        private readonly IMongoCollection<Employees> _Employees;

        public StaffVM()
        {
            _Employees = GetStaff();
            LoadStaff();
            SearchCM = new RelayCommand<StaffView>((p) => true, (p) => _Search(p));
            AddStaffCM = new RelayCommand<StaffView>((p) => true, (p) => _AddStaff());
            ExportStaffCM = new RelayCommand<StaffView>((p) => true, (p) => _ExportStaff());
            ImportStaffCM = new RelayCommand<StaffView>((p) => true, (p) => _ImportStaff());
        }

        private IMongoCollection<Employees> GetStaff()
        {
            // Set your MongoDB connection string and database name
            string connectionString =
                "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "Restaurant_Management_Application"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Employees>("Employees");
        }

        private void LoadStaff()
        {
            var employee = _Employees.Find(Builders<Employees>.Filter.Empty).ToList();
            EmployeeList = new ObservableCollection<Employees>(employee);
        }

        private void _Search(StaffView parameter)
        {
            ObservableCollection<Employees> temp = new ObservableCollection<Employees>();
            if (!string.IsNullOrEmpty(parameter.txtSearch.Text))
            {
                var filterBuilder = Builders<Employees>.Filter;
                FilterDefinition<Employees> filter;

                var keyword = parameter.txtSearch.Text;

                filter = filterBuilder.Or(
                    filterBuilder.Regex("employeeId", new BsonRegularExpression(keyword, "i")),
                    filterBuilder.Regex("fullName", new BsonRegularExpression(keyword, "i")),
                    filterBuilder.Regex("phoneNumber", new BsonRegularExpression(keyword, "i"))
                );
                var result = _Employees.Find(filter).ToList();
                temp = new ObservableCollection<Employees>(result);
            }
            else
            {
                var result = _Employees.Find(Builders<Employees>.Filter.Empty).ToList();
                temp = new ObservableCollection<Employees>(result);
            }
            parameter.staffDataGrid.ItemsSource = temp;
        }

        private void _AddStaff()
        {
            Add_EditStaff addStaff = new Add_EditStaff();
            var window = new Window
            {
                Content = addStaff,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStyle = WindowStyle.None,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();
            LoadStaff();
        }

        private void _ExportStaff()
        {
            // Create a SaveFileDialog
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
                DefaultExt = "csv",
                Title = "Export Employee List"
            };

            // Show the SaveFileDialog and get the selected file path
            var result = saveFileDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                var filePath = saveFileDialog.FileName;

                StringBuilder csvContent = new StringBuilder();

                // Add the header row to the CSV content
                csvContent.AppendLine("Employee ID,Full Name, Department, Phone Number, Email, Hire Date");

                // Add staff data to the CSV content
                foreach (var staff in EmployeeList)
                {
                    csvContent.AppendLine($"{staff.EmployeeId},{staff.FullName},{(staff.IsAdmin ? "Owner" : "Staff")},{staff.PhoneNumber},{staff.Email},{staff.DateOfJoining}");
                }

                // Write the CSV content to the selected file
                File.WriteAllText(filePath, csvContent.ToString());

                MessageBox.Show($"Employee list exported successfully!");
            }
        }

        private void _ImportStaff()
        {
            // Create an OpenFileDialog
            var openFileDialog = new OpenFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
                DefaultExt = "csv",
                Title = "Import Staff List"
            };

            // Show the OpenFileDialog and get the selected file path
            var result = openFileDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                var filePath = openFileDialog.FileName;

                try
                {
                    var csvLines = File.ReadAllLines(filePath);

                    var staffDataLines = csvLines.Skip(1);

                    var newStaff = new List<Employees>();

                    foreach (var line in staffDataLines)
                    {
                        var values = line.Split(',');

                        var newStaffMember = new Employees
                        {
                            EmployeeId = values[0],
                            FullName = values[1],
                            IsAdmin = values[2].ToLower() == "owner", // Assuming "Owner" is true, "Staff" is false
                            PhoneNumber = values[3],
                            Email = values[4],
                            DateOfJoining = DateTime.Parse(values[5])
                        };

                        var existingStaff = _Employees.Find(Builders<Employees>.Filter.Eq("employeeId", newStaffMember.EmployeeId)).FirstOrDefault();

                        if (existingStaff == null)
                        {
                            newStaff.Add(newStaffMember);
                        }
                    }

                    var newStaffCsvContent = new StringBuilder();

                    // Add staff data to the CSV content
                    foreach (var staffMember in newStaff)
                    {
                        newStaffCsvContent.AppendLine($"{staffMember.EmployeeId},{staffMember.FullName},{staffMember.IsAdmin},{staffMember.PhoneNumber},{staffMember.Email},{staffMember.DateOfJoining}");
                    }

                    // Append the new staff data to the existing staff data
                    var allStaffCsvContent = new StringBuilder(File.ReadAllText(filePath));
                    allStaffCsvContent.AppendLine(newStaffCsvContent.ToString());

                    // Write the combined CSV content to the file
                    File.WriteAllText(filePath, allStaffCsvContent.ToString());

                    MessageBox.Show($"Employee members imported successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error importing employee members: {ex.Message}");
                }
            }
        }
    }
}
