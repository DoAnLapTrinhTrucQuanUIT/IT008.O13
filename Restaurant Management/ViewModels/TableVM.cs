using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MongoDB.Driver;
using Restaurant_Management.Models;
using Restaurant_Management.Utilities;
using Restaurant_Management.Views;

namespace Restaurant_Management.ViewModels
{
    public class TableVM : ViewModelBase
    {
        private ObservableCollection<Invoices> _tableItemList;
        public ObservableCollection<Invoices> TableItemList
        {
            get { return _tableItemList; }
            set
            {
                _tableItemList = value;
                OnPropertyChanged(nameof(TableItemList));
            }
        }

        private Invoices _selectedInvoice;
        public Invoices SelectedInvoice
        {
            get { return _selectedInvoice; }
            set
            {
                _selectedInvoice = value;
                OnPropertyChanged(nameof(SelectedInvoice));
            }
        }

        private readonly IMongoCollection<Tables> _tablesCollection;

        public ICommand LoadedCommand { get; set; }
        public TableVM()
        {
            _tablesCollection = GetTables();
            LoadedCommand = new RelayCommand<TableView>((p) => true, (p) => _Load(p));
        }

        private IMongoCollection<Tables> GetTables()
        {
            // Implement GetTables as in your original code
            // Set your MongoDB connection string and database name
            string connectionString =
                "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "Restaurant_Management_Application"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Tables>("Tables");
        }

        private IMongoCollection<Invoices> GetInvoices()
        {
            // Implement GetInvoices as in your original code
            // Set your MongoDB connection string and database name
            string connectionString =
                "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "Restaurant_Management_Application"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Invoices>("Invoices");
        }

        private void _Load(TableView tableView)
        {

        }
    }
}
