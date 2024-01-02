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
using System.Globalization;

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

        private ObservableCollection<Employees> _searchEmployeeList;
        public ObservableCollection<Employees> SearchEmployeeList
        {
            get { return _searchEmployeeList; }
            set
            {
                _searchEmployeeList = value;
                OnPropertyChanged(nameof(SearchEmployeeList));
            }
        }

        public ICommand SearchCM { get; set; }
        public ICommand AddStaffCM { get; set; }
        public ICommand ExportStaffCM { get; set; }
        public ICommand ImportStaffCM { get; set; }
        public ICommand DeleteStaffCommand { get; set; }

        private readonly IMongoCollection<Employees> _employees;

        public StaffVM()
        {
            _employees = GetEmployees();
            LoadEmployees();

            SearchCM = new RelayCommand<StaffView>((p) => true, (p) => _Search(p));
            AddStaffCM = new RelayCommand<StaffView>((p) => true, (p) => _AddStaff());
            ExportStaffCM = new RelayCommand<StaffView>((p) => true, (p) => _ExportStaff());
            ImportStaffCM = new RelayCommand<StaffView>((p) => true, (p) => _ImportStaff());
            DeleteStaffCommand = new RelayCommand<Employees>((employee) => true, (employee) => _DeleteEmployee(employee));
        }

        private IMongoCollection<Employees> GetEmployees()
        {
            // Implementation to connect to MongoDB and get the collection
            // Similar to what you have in CustomerVM
            // Set your MongoDB connection string and database name
            string connectionString =
                "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "Restaurant_Management_Application"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Employees>("Employees");
        }

        private void LoadEmployees()
        {
            // Implementation to load employees from MongoDB
            // Similar to what you have in CustomerVM
            var employee = _employees.Find(Builders<Employees>.Filter.Empty).ToList();
            EmployeeList = new ObservableCollection<Employees>(employee);
        }

        // Implement methods for searching, adding, exporting, importing, and deleting employees

        private void _Search(StaffView parameter)
        {
            // Implementation for searching employees
            SearchEmployeeList = new ObservableCollection<Employees>();
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
                var result = _employees.Find(filter).ToList();
                SearchEmployeeList = new ObservableCollection<Employees>(result);
            }
            else
            {
                var result = _employees.Find(Builders<Employees>.Filter.Empty).ToList();
                SearchEmployeeList = new ObservableCollection<Employees>(result);
            }
            parameter.staffDataGrid.ItemsSource = SearchEmployeeList;
        }

        private void _AddStaff()
        {
            // Implementation for adding a new employee
            AddStaff addStaff = new AddStaff();
            var window = new Window
            {
                Content = addStaff,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStyle = WindowStyle.None,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();
            LoadEmployees();
        }

        void _ExportStaff()
        {
            // Create a SaveFileDialog
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
                DefaultExt = "csv",
                Title = "Export Staff List"
            };

            // Show the SaveFileDialog and get the selected file path
            var result = saveFileDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                var filePath = saveFileDialog.FileName;

                StringBuilder csvContent = new StringBuilder();

                // Add the header row to the CSV content
                csvContent.AppendLine("Employee ID,Full Name, Date of Birth, Phone Number, Gender, Email, Address, Date of Joining, Is Admin");

                // Choose the list to export based on SearchEmployeeList
                var exportList = (SearchEmployeeList != null && SearchEmployeeList.Count > 0) ? SearchEmployeeList : EmployeeList;

                // Add employee data to the CSV content
                foreach (var employee in exportList)
                {
                    string formattedDateOfBirth = employee.DateOfBirth.ToString("dd/MM/yy");
                    string formattedDateOfJoining = employee.DateOfJoining.ToString("dd/MM/yy");
                    csvContent.AppendLine($"{employee.EmployeeId},{employee.FullName},{formattedDateOfBirth},{employee.PhoneNumber},{employee.Gender},{employee.Email},{employee.Address},{formattedDateOfJoining},{(employee.IsAdmin ? "Owner" : "Staff")}");
                }

                // Write the CSV content to the selected file
                File.WriteAllText(filePath, csvContent.ToString());

                MessageBox.Show($"Staff list exported successfully!");
            }
        }


        void _ImportStaff()
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

                    var employeeDataLines = csvLines.Skip(1);

                    var newEmployees = new List<Employees>();

                    foreach (var line in employeeDataLines)
                    {
                        var values = line.Split(',');

                        DateTime dateOfBirth, dateOfJoining;

                        if (DateTime.TryParseExact(values[2], "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth) &&
                            DateTime.TryParseExact(values[7], "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfJoining))
                        {
                            var newEmployee = new Employees
                            {
                                EmployeeId = values[0],
                                FullName = values[1],
                                DateOfBirth = dateOfBirth,
                                PhoneNumber = values[3],
                                Gender = values[4],
                                Email = values[5],
                                Address = values[6],
                                DateOfJoining = dateOfJoining,
                                IsActive = true,
                                IsAdmin = string.Equals(values[8], "owner", StringComparison.OrdinalIgnoreCase)
                            };

                            var existingEmployee = _employees.Find(Builders<Employees>.Filter.Eq("employeeId", newEmployee.EmployeeId)).FirstOrDefault();

                            if (existingEmployee == null)
                            {
                                newEmployees.Add(newEmployee);
                            }
                        }
                    }

                    foreach (var newEmployee in newEmployees)
                    {
                        EmployeeList.Add(newEmployee);
                        _employees.InsertOne(newEmployee);
                    }

                    MessageBox.Show($"Employees imported successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error importing employees: {ex.Message}");
                }
            }
        }

        private void _DeleteEmployee(Employees employee)
        {
            // Implementation for deleting an employee
            // You can use _employees collection to perform delete operation
            // Implement logic to delete the selected employee
            if (employee != null)
            {
                // Confirm deletion with the user
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {employee.FullName}?",
                                                          "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Perform deletion logic here
                    _employees.DeleteOne(Builders<Employees>.Filter.Eq("employeeId", employee.EmployeeId));

                    // Reload the staff list after deletion
                    LoadEmployees();
                }
            }
        }
    }
}
