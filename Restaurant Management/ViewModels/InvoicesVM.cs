using MongoDB.Bson;
using MongoDB.Driver;
using Restaurant_Management.Models;
using Restaurant_Management.Utilities;
using Restaurant_Management.Views;
using Restaurant_Management.Views.Component;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Restaurant_Management.ViewModels
{
    public class InvoicesVM : Utilities.ViewModelBase
    {
        private ObservableCollection<Invoices> _invoicesList;
        public ObservableCollection<Invoices> InvoicesList
        {
            get { return _invoicesList; }
            set
            {
                _invoicesList = value;
                OnPropertyChanged(nameof(InvoicesList));
            }
        }

        private readonly IMongoCollection<Invoices> _Invoices;
        public ICommand SearchInvoicesCommand { get; set; }
        public ICommand ExportInvoiceCommand { get; set; }
        public ICommand DeleteInvoiceCommand { get; set; }
        public ICommand AllInvoicesCommand { get; set; }
        public ICommand PaidInvoicesCommand { get; set; }
        public ICommand UnpaidInvoicesCommand { get; set; }


        public InvoicesVM()
        {
            _Invoices = GetInvoices();
            Load();
            SearchInvoicesCommand = new RelayCommand<InvoicesView>((p) => true, (p) => _SearchInvoices(p));
            ExportInvoiceCommand = new RelayCommand<Invoices>((invoice) => true, (invoice) => _ExportInvoices(invoice));
            DeleteInvoiceCommand = new RelayCommand<Invoices>((invoice) => true, (invoice) => _DeleteInvoice(invoice));
            AllInvoicesCommand = new RelayCommand<InvoicesView>((p) => true, (p) => _AllInvoices(p));
            PaidInvoicesCommand = new RelayCommand<InvoicesView>((p) => true, (p) => _PaidInvoices(p));
            UnpaidInvoicesCommand = new RelayCommand<InvoicesView>((p) => true, (p) => _UnpaidInvoices(p));
        }
        private IMongoCollection<Invoices> GetInvoices()
        {
            // Set your MongoDB connection string and database name
            string connectionString =
                "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "Restaurant_Management_Application"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Invoices>("Invoices");
        }
        private void Load()
        {
            var invoices = _Invoices.Find(Builders<Invoices>.Filter.Empty).ToList();
            InvoicesList = new ObservableCollection<Invoices>(invoices);
        }
        void _SearchInvoices(InvoicesView parameter)
        {
            ObservableCollection<Invoices> temp = new ObservableCollection<Invoices>();
            if (!string.IsNullOrEmpty(parameter.txtSearch.Text))
            {
                var filterBuilder = Builders<Invoices>.Filter;
                FilterDefinition<Invoices> filter;

                var keyword = parameter.txtSearch.Text;

                filter = filterBuilder.Or(
                    filterBuilder.Regex("invoiceId", new BsonRegularExpression(keyword, "i")),
                    filterBuilder.Regex("paidEmployee.employeeId", new BsonRegularExpression(keyword, "i")),
                    filterBuilder.Regex("paidCustomer.customerId", new BsonRegularExpression(keyword, "i"))
                );
                var result = _Invoices.Find(filter).ToList();
                temp = new ObservableCollection<Invoices>(result);
            }
            else
            {
                var result = _Invoices.Find(Builders<Invoices>.Filter.Empty).ToList();
                temp = new ObservableCollection<Invoices>(result);
            }
            parameter.invoicesDataGrid.ItemsSource = temp;
        }

        private void _ExportInvoices(Invoices invoice)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = $"Invoice_{invoice.InvoiceId}.pdf",
                DefaultExt = ".pdf",
                Filter = "PDF documents (.pdf)|*.pdf"
            };
            ExportInvoiceToPdf(invoice, saveFileDialog.FileName);
        }

        private void ExportInvoiceToPdf(Invoices invoice, string filePath)
        {

        }

        private void _DeleteInvoice(Invoices invoice)
        {
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete Invoice ID {invoice.InvoiceId}?",
                                                                      "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _Invoices.DeleteOne(Builders<Invoices>.Filter.Eq("invoiceId", invoice.InvoiceId));
                Load();
            }
        }

        private void _AllInvoices(InvoicesView parameter)
        {
            var result = _Invoices.Find(Builders<Invoices>.Filter.Empty).ToList();
            var filteredInvoices = new ObservableCollection<Invoices>(result);
            parameter.invoicesDataGrid.ItemsSource = filteredInvoices;
        }

        private void _PaidInvoices(InvoicesView parameter)
        {
            FilterInvoicesByStatus(true, parameter);
        }

        private void _UnpaidInvoices(InvoicesView parameter)
        {
            FilterInvoicesByStatus(false, parameter);

        }
        private void FilterInvoicesByStatus(bool isPaid, InvoicesView parameter)
        {
            var filterBuilder = Builders<Invoices>.Filter;
            var filter = filterBuilder.Eq("status", isPaid);

            var result = _Invoices.Find(filter).ToList();
            var filteredInvoices = new ObservableCollection<Invoices>(result);

            parameter.invoicesDataGrid.ItemsSource = filteredInvoices;
        }
    }
}
