using MongoDB.Driver;
using Restaurant_Management.Models;
using Restaurant_Management.Utilities;
using Restaurant_Management.Views;
using Restaurant_Management.Views.Component;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Restaurant_Management.ViewModels
{
    public class SalaryVM : Utilities.ViewModelBase
    {
        private bool _isPaidButtonClicked;

        public bool IsPaidButtonClicked
        {
            get { return _isPaidButtonClicked; }
            set
            {
                if (_isPaidButtonClicked != value)
                {
                    _isPaidButtonClicked = value;
                    OnPropertyChanged(nameof(IsPaidButtonClicked));
                    UpdateEditSalaryButtonVisibility();
                }
            }
        }

        private bool _isEditSalaryButtonVisible;

        public bool IsEditSalaryButtonVisible
        {
            get { return _isEditSalaryButtonVisible; }
            set
            {
                if (_isEditSalaryButtonVisible != value)
                {
                    _isEditSalaryButtonVisible = value;
                    OnPropertyChanged(nameof(IsEditSalaryButtonVisible));
                }
            }
        }

        private void UpdateEditSalaryButtonVisibility()
        {
            IsEditSalaryButtonVisible = IsPaidButtonClicked;
            OnPropertyChanged(nameof(IsEditSalaryButtonVisible));
        }

        private decimal _filteredSelectedBasicSalary;

        public decimal FilteredSelectedBasicSalary
        {
            get { return _filteredSelectedBasicSalary; }
            set
            {
                if (_filteredSelectedBasicSalary != value)
                {
                    _filteredSelectedBasicSalary = value;
                    OnPropertyChanged(nameof(FilteredSelectedBasicSalary));
                }
            }
        }

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

        private ObservableCollection<SalaryInformation> _salaryList;

        public ObservableCollection<SalaryInformation> SalaryList
        {
            get { return _salaryList; }
            set
            {
                _salaryList = value;
                OnPropertyChanged(nameof(SalaryList));
            }
        }

        private ObservableCollection<SalaryInformation> _salaryInformationList;

        public ObservableCollection<SalaryInformation> SalaryInformationList
        {
            get { return _salaryInformationList; }
            set
            {
                _salaryInformationList = value;
                OnPropertyChanged(nameof(SalaryInformationList));
            }
        }

        private SalaryInformation _selectedSalaryItem;

        public SalaryInformation SelectedSalaryItem
        {
            get { return _selectedSalaryItem; }
            set
            {
                if (_selectedSalaryItem != value)
                {
                    _selectedSalaryItem = value;
                    OnPropertyChanged(nameof(SelectedSalaryItem));
                }
            }
        }

        private SalaryInformation GetSelectedSalary()
        {
            return SelectedSalaryItem;
        }

        private SalaryInformation _filteredSelectedSalaryItem;

        public SalaryInformation FilteredSelectedSalaryItem
        {
            get { return _filteredSelectedSalaryItem; }
            set
            {
                if (_filteredSelectedSalaryItem != value)
                {
                    _filteredSelectedSalaryItem = value;
                    OnPropertyChanged(nameof(FilteredSelectedSalaryItem));

                    if (_filteredSelectedSalaryItem != null)
                    {
                        FilteredSelectedBasicSalary = _filteredSelectedSalaryItem.BasicSalary;
                    }
                    else
                    {
                        FilteredSelectedBasicSalary = 0;
                    }
                }
            }
        }

        public ICommand ShowSalaryInformationCommand { get; set; }

        public ICommand EditSalaryCommand { get; set; }

        public ICommand ConfirmCommand { get; set; }

        private readonly IMongoCollection<Employees> _employees;

        protected readonly IMongoCollection<SalaryInformation> _salary;

        private IMongoCollection<Employees> GetEmployees()
        {
            string connectionString = "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/";
            
            string databaseName = "Restaurant_Management_Application";

            var client = new MongoClient(connectionString);
            
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Employees>("Employees");
        }

        private IMongoCollection<SalaryInformation> GetSalaryInformation()
        {
            string connectionString = "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/";
            
            string databaseName = "Restaurant_Management_Application";

            var client = new MongoClient(connectionString);
            
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<SalaryInformation>("SalaryInformation");
        }

        public SalaryVM()
        {
            _employees = GetEmployees();
            
            _salary = GetSalaryInformation();
            
            LoadEmployees();

            InitializeCommand();
        }
        private void InitializeCommand()
        {
            EditSalaryCommand = new RelayCommand<SalaryInformation>((p) => true, (parameter) => EditSalary(parameter));
            
            ConfirmCommand = new RelayCommand<object>((p) => true, (p) => Confirm(p));
            
            ShowSalaryInformationCommand = new RelayCommand<SalaryInformation>((p) => true, p => ShowSalaryInformation(p));
        }

        private void Confirm(object parameter)
        {
            if (IsPaidButtonClicked == false)
            {
                MessageBox.Show("Please select an employee before confirming the changes.", "Information", MessageBoxButton.OK);
            }
            else if (SelectedSalaryItem != null)
            {
                SelectedSalaryItem.StartDate = SelectedSalaryItem.PayDate.AddDays(1);

                SelectedSalaryItem.PayDate = SelectedSalaryItem.StartDate.AddMonths(1);

                SelectedSalaryItem.WorkedDays = 0;

                OnPropertyChanged(nameof(SelectedSalaryItem));

                UpdateSalaryInformationInDatabase(SelectedSalaryItem);

                MessageBox.Show($"Successfully paid salary to employee {SelectedSalaryItem.Employees.FullName}.");
            }
        }

        private void UpdateSalaryInformationInDatabase(SalaryInformation salaryInformation)
        {
            try
            {
                var filter = Builders<SalaryInformation>.Filter.Eq("employeeInfo.employeeId", salaryInformation.Employees.EmployeeId);

                var update = Builders<SalaryInformation>.Update
                    .Set("StartDate", salaryInformation.StartDate)
                    .Set("PayDate", salaryInformation.PayDate)
                    .Set("WorkedDays", salaryInformation.WorkedDays);

                _salary.UpdateOne(filter, update);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating SalaryInformation in the database: " + ex.Message);
            }
        }

        private void EditSalary(SalaryInformation selectedSalary)
        {
            EditSalaryVM editSalaryViewModel = new EditSalaryVM();

            editSalaryViewModel.SelectedSalary = selectedSalary;

            EditSalary editSalaryControl = new EditSalary();

            editSalaryControl.DataContext = editSalaryViewModel;

            Window editSalaryWindow = new Window
            {
                Content = editSalaryControl,

                SizeToContent = SizeToContent.WidthAndHeight,

                WindowStyle = WindowStyle.None,

                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            editSalaryViewModel.BasicSalaryUpdated += (sender, editedBasicSalary) =>
            {
                if (sender is EditSalaryVM vm && vm.SelectedSalary != null)
                {
                    SalaryInformation updatedSalary = vm.SelectedSalary;

                    updatedSalary.BasicSalary = editedBasicSalary;

                    UpdateSalaryInformation(updatedSalary);
                }
            };

            editSalaryWindow.ShowDialog();
        }

        private void UpdateSalaryInformation(SalaryInformation salaryInformation)
        {
            try
            {
                var filter = Builders<SalaryInformation>.Filter.Eq("employeeInfo.employeeId", salaryInformation.Employees.EmployeeId);

                var update = Builders<SalaryInformation>.Update.Set("BasicSalary", salaryInformation.BasicSalary);

                _salary.UpdateOne(filter, update);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating SalaryInformation: " + ex.Message);
            }
        }

        private void ShowSalaryInformation(SalaryInformation parameter)
        {
            var filter = Builders<SalaryInformation>.Filter.Eq("employeeInfo.employeeId", parameter.Employees.EmployeeId);

            var salary = _salary.Find(filter).FirstOrDefault();

            SelectedSalaryItem = salary;

            IsPaidButtonClicked = true;

            if (salary != null)
            {
                FilteredSelectedSalaryItem = new SalaryInformation
                {
                    Employees = salary.Employees,

                    StartDate = salary.StartDate,

                    PayDate = salary.PayDate,

                    WorkedDays = salary.WorkedDays,

                    BasicSalary = salary.BasicSalary,

                    TotalSalary = salary.WorkedDays * salary.BasicSalary,
                };
            }
            else
            {
                FilteredSelectedSalaryItem = new SalaryInformation();
            }
        }

        private void LoadEmployees()
        {
            var filter = Builders<Employees>.Filter.Eq("IsAdmin", false);

            var employees = _employees.Find(filter).ToList();

            EmployeeList = new ObservableCollection<Employees>(employees);

            if (EmployeeList.Any())
            {
                SalaryList = new ObservableCollection<SalaryInformation>();

                foreach (var employee in EmployeeList)
                {
                    var salaryInfo = new SalaryInformation
                    {
                        Employees = employee,
                    };

                    SalaryList.Add(salaryInfo);
                }
            }
        }

    }
}
