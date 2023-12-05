using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant_Management.Utilities;
using System.Windows.Input;
namespace Restaurant_Management.ViewModels
{
    class NavigationVM : Utilities.ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }
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
        private void Menu(object obj) => CurrentView = new MenuVM();
        private void Table(object obj) => CurrentView = new TableVM();
        private void Customer(object obj) => CurrentView = new CustomerVM();
        private void Schedule(object obj) => CurrentView = new ScheduleVM();
        private void StaffInformation(object obj) => CurrentView = new StaffInformationVM();
        private void Salary(object obj) => CurrentView = new SalaryVM();
        private void InventoryOverview(object obj) => CurrentView = new InventoryOverviewVM();
        private void Product(object obj) => CurrentView = new ProductsVM();
        private void SalesOverview(object obj) => CurrentView = new SalesOverviewVM();
        private void Invoice(object obj) => CurrentView = new InvoicesVM();
        private void Setting(object obj) => CurrentView = new SettingsVM();
 
        public NavigationVM()
        {
            MenuCommand = new RelayCommand(Menu);
            TableCommand = new RelayCommand(Table); 
            CustomerCommand = new RelayCommand(Customer);
            ScheduleCommand = new RelayCommand(Schedule);
            StaffInformationCommand = new RelayCommand(StaffInformation);
            SalaryCommand = new RelayCommand(Salary);
            InventoryOverviewCommand = new RelayCommand(InventoryOverview); 
            ProductCommand = new RelayCommand(Product);
            SalesOverviewCommand = new RelayCommand(SalesOverview);
            InvoiceCommand = new RelayCommand(Invoice);
            SettingCommand = new RelayCommand(Setting);

            CurrentView = new MenuVM();
        }
    }
}
