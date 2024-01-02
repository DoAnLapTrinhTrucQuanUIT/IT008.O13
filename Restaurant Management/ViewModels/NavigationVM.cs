using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant_Management.Utilities;
using System.Windows.Input;
using Restaurant_Management.Models;
using System.Security.Permissions;
using Restaurant_Management.Views;
using System.Windows;

namespace Restaurant_Management.ViewModels
{
    public class NavigationVM : Utilities.ViewModelBase
    {
        private Employees _CurrentUser;
        public Employees CurrentUser
        {
            get { return _CurrentUser; }
            set { _CurrentUser = value; OnPropertyChanged(); }
        }

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }
        public ICommand EmptyCommand { get; set; }
        public ICommand MenuCommand { get; set; }
        public ICommand TableCommand { get; set; }
        public ICommand CustomerCommand { get; set; }
        public ICommand ScheduleCommand { get; set; }
        public ICommand StaffInformationCommand { get; set; }
        public ICommand SalaryCommand { get; set; }
        public ICommand InventoryOverviewCommand { get; set; }
        public ICommand ProductCommand { get; set; }
        public ICommand SalesOverviewCommand { get; set; }
        public ICommand InvoiceCommand { get; set; }
        public ICommand SettingCommand { get; set; }

        private void Empty(object obj) => CurrentView = new EmptyVM();
        private void Menu(object obj) => CurrentView = new MenuVM();
        private void Table(object obj) => CurrentView = new TableVM();
        private void Customer(object obj) => CurrentView = new CustomerVM();
        private void StaffInformation(object obj) => CurrentView = new StaffInformationVM();
        private void Salary(object obj) => CurrentView = new SalaryVM();
        private void Product(object obj) => CurrentView = new ProductsVM();
        private void Invoice(object obj) => CurrentView = new InvoicesVM();
        private void Setting(object obj) => CurrentView = new SettingsVM();
        public NavigationVM()
        {
            EmptyCommand = new RelayCommand(Empty);
            MenuCommand = new RelayCommand(Menu);
            TableCommand = new RelayCommand(Table);
            CustomerCommand = new RelayCommand(Customer);
            StaffInformationCommand = new RelayCommand(StaffInformation);
            SalaryCommand = new RelayCommand(Salary);
            ProductCommand = new RelayCommand(Product);
            InvoiceCommand = new RelayCommand(Invoice);
            SettingCommand = new RelayCommand(Setting);

            CurrentView = new EmptyView();
        }
        public void Decentralization(MainWindow mainWindow)
        {
            if (CurrentUser == null && mainWindow.DataContext is NavigationVM navigationVM)
            {
                CurrentUser = navigationVM.CurrentUser;
            }

            if (CurrentUser != null && CurrentUser.IsAdmin)
            {
                mainWindow.AdminLeftSidebar.Visibility = Visibility.Visible;
                mainWindow.StaffLeftSidebar.Visibility = Visibility.Collapsed;
            }
            else
            {
                mainWindow.AdminLeftSidebar.Visibility = Visibility.Collapsed;
                mainWindow.StaffLeftSidebar.Visibility = Visibility.Visible;
            }
        }


    }
}
