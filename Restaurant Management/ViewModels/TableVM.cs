using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MongoDB.Driver;
using Restaurant_Management.Models;
using Restaurant_Management.Utilities;
using Restaurant_Management.Views;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace Restaurant_Management.ViewModels
{
    public class TableVM : ViewModelBase
    {
        private string _customerName1;
        public string CustomerName1
        {
            get => _customerName1;
            set
            {
                _customerName1 = value;
                OnPropertyChanged(nameof(CustomerName1));
            }
        }

        private string _customerName2;
        public string CustomerName2
        {
            get => _customerName2;
            set
            {
                _customerName2 = value;
                OnPropertyChanged(nameof(CustomerName2));
            }
        }

        private string _customerName3;
        public string CustomerName3
        {
            get => _customerName3;
            set
            {
                _customerName3 = value;
                OnPropertyChanged(nameof(CustomerName3));
            }
        }

        private string _customerName4;
        public string CustomerName4
        {
            get => _customerName4;
            set
            {
                _customerName4 = value;
                OnPropertyChanged(nameof(CustomerName4));
            }
        }

        private string _customerName5;
        public string CustomerName5
        {
            get => _customerName5;
            set
            {
                _customerName5 = value;
                OnPropertyChanged(nameof(CustomerName5));
            }
        }

        private string _customerName6;
        public string CustomerName6
        {
            get => _customerName6;
            set
            {
                _customerName6 = value;
                OnPropertyChanged(nameof(CustomerName6));
            }
        }

        private string _customerName7;
        public string CustomerName7
        {
            get => _customerName7;
            set
            {
                _customerName7 = value;
                OnPropertyChanged(nameof(CustomerName7));
            }
        }

        private string _customerName8;
        public string CustomerName8
        {
            get => _customerName8;
            set
            {
                _customerName8 = value;
                OnPropertyChanged(nameof(CustomerName8));
            }
        }

        private string _customerName9;
        public string CustomerName9
        {
            get => _customerName9;
            set
            {
                _customerName9 = value;
                OnPropertyChanged(nameof(CustomerName9));
            }
        }

        private double _totalAmount1;
        public double TotalAmount1
        {
            get => _totalAmount1;
            set
            {
                _totalAmount1 = value;
                OnPropertyChanged(nameof(TotalAmount1));
            }
        }

        private double _totalAmount2;
        public double TotalAmount2
        {
            get => _totalAmount2;
            set
            {
                _totalAmount2 = value;
                OnPropertyChanged(nameof(TotalAmount2));
            }
        }

        private double _totalAmount3;
        public double TotalAmount3
        {
            get => _totalAmount3;
            set
            {
                _totalAmount3 = value;
                OnPropertyChanged(nameof(TotalAmount3));
            }
        }

        private double _totalAmount4;
        public double TotalAmount4
        {
            get => _totalAmount4;
            set
            {
                _totalAmount4 = value;
                OnPropertyChanged(nameof(TotalAmount4));
            }
        }

        private double _totalAmount5;
        public double TotalAmount5
        {
            get => _totalAmount5;
            set
            {
                _totalAmount5 = value;
                OnPropertyChanged(nameof(TotalAmount5));
            }
        }

        private double _totalAmount6;
        public double TotalAmount6
        {
            get => _totalAmount6;
            set
            {
                _totalAmount6 = value;
                OnPropertyChanged(nameof(TotalAmount6));
            }
        }

        private double _totalAmount7;
        public double TotalAmount7
        {
            get => _totalAmount7;
            set
            {
                _totalAmount7 = value;
                OnPropertyChanged(nameof(TotalAmount7));
            }
        }

        private double _totalAmount8;
        public double TotalAmount8
        {
            get => _totalAmount8;
            set
            {
                _totalAmount8 = value;
                OnPropertyChanged(nameof(TotalAmount8));
            }
        }

        private double _totalAmount9;
        public double TotalAmount9
        {
            get => _totalAmount9;
            set
            {
                _totalAmount9 = value;
                OnPropertyChanged(nameof(TotalAmount9));
            }
        }

        private void InitializeObservableCollection()
        {
            InvoiceDetailsList1 = new ObservableCollection<InvoiceDetails>();
            InvoiceDetailsList2 = new ObservableCollection<InvoiceDetails>();
            InvoiceDetailsList3 = new ObservableCollection<InvoiceDetails>();
            InvoiceDetailsList4 = new ObservableCollection<InvoiceDetails>();
            InvoiceDetailsList5 = new ObservableCollection<InvoiceDetails>();
            InvoiceDetailsList6 = new ObservableCollection<InvoiceDetails>();
            InvoiceDetailsList7 = new ObservableCollection<InvoiceDetails>();
            InvoiceDetailsList8 = new ObservableCollection<InvoiceDetails>();
            InvoiceDetailsList9 = new ObservableCollection<InvoiceDetails>();
        }

        private void InitializeCommand()
        {
            PaidCommand1 = new RelayCommand<TableView>((p) => true, (p) => _PaidCommand1());
            PaidCommand2 = new RelayCommand<TableView>((p) => true, (p) => _PaidCommand2());
            PaidCommand3 = new RelayCommand<TableView>((p) => true, (p) => _PaidCommand3());
            PaidCommand4 = new RelayCommand<TableView>((p) => true, (p) => _PaidCommand4());
            PaidCommand5 = new RelayCommand<TableView>((p) => true, (p) => _PaidCommand5());
            PaidCommand6 = new RelayCommand<TableView>((p) => true, (p) => _PaidCommand6());
            PaidCommand7 = new RelayCommand<TableView>((p) => true, (p) => _PaidCommand7());
            PaidCommand8 = new RelayCommand<TableView>((p) => true, (p) => _PaidCommand8());
            PaidCommand9 = new RelayCommand<TableView>((p) => true, (p) => _PaidCommand9());
        }

        public void LoadInvoiceDetail()
        {
            var unpaidInvoices = _invoicesCollection.Find(t => t.Status == false).ToList();

            foreach (var invoice in unpaidInvoices)
            {
                var invoiceDetails = _invoiceDetailsCollection.Find(id => id.Invoice.InvoiceId == invoice.InvoiceId).ToList();

                switch (invoice.Table.TableName)
                {
                    case "Table 1":
                        foreach (var detail in invoiceDetails)
                        {
                            InvoiceDetailsList1.Add(detail);
                            CustomerName1 = detail.Invoice.Customer.FullName;
                            TotalAmount1 += detail.Amount;
                        }
                        break;

                    case "Table 2":
                        foreach (var detail in invoiceDetails)
                        {
                            InvoiceDetailsList2.Add(detail);
                            CustomerName2 = detail.Invoice.Customer.FullName;
                            TotalAmount2 += detail.Amount;
                        }
                        break;

                    case "Table 3":
                        foreach (var detail in invoiceDetails)
                        {
                            InvoiceDetailsList3.Add(detail);
                            CustomerName3 = detail.Invoice.Customer.FullName;
                            TotalAmount3 += detail.Amount;
                        }
                        break;

                    case "Table 4":
                        foreach (var detail in invoiceDetails)
                        {
                            InvoiceDetailsList4.Add(detail);
                            CustomerName4 = detail.Invoice.Customer.FullName;
                            TotalAmount4 += detail.Amount;
                        }
                        break;

                    case "Table 5":
                        foreach (var detail in invoiceDetails)
                        {
                            InvoiceDetailsList5.Add(detail);
                            CustomerName5 = detail.Invoice.Customer.FullName;
                            TotalAmount5 += detail.Amount;
                        }
                        break;

                    case "Table 6":
                        foreach (var detail in invoiceDetails)
                        {
                            InvoiceDetailsList6.Add(detail);
                            CustomerName6 = detail.Invoice.Customer.FullName;
                            TotalAmount6 += detail.Amount;
                        }
                        break;

                    case "Table 7":
                        foreach (var detail in invoiceDetails)
                        {
                            InvoiceDetailsList7.Add(detail);
                            CustomerName7 = detail.Invoice.Customer.FullName;
                            TotalAmount7 += detail.Amount;
                        }
                        break;

                    case "Table 8":
                        foreach (var detail in invoiceDetails)
                        {
                            InvoiceDetailsList8.Add(detail);
                            CustomerName8 = detail.Invoice.Customer.FullName;
                            TotalAmount8 += detail.Amount;
                        }
                        break;

                    case "Table 9":
                        foreach (var detail in invoiceDetails)
                        {
                            InvoiceDetailsList9.Add(detail);
                            CustomerName9 = detail.Invoice.Customer.FullName;
                            TotalAmount9 += detail.Amount;
                        }
                        break;
                }
            }
        }

        public void _PaidCommand1()
        {
            ProcessPayment("TABLE1");
            InvoiceDetailsList1.Clear();
            CustomerName1 = "";
            TotalAmount1 = 0;
        }

        public void _PaidCommand2()
        {
            ProcessPayment("TABLE2");
            InvoiceDetailsList2.Clear();
            CustomerName2 = "";
            TotalAmount2 = 0;
        }

        public void _PaidCommand3()
        {
            ProcessPayment("TABLE3");
            InvoiceDetailsList3.Clear();
            CustomerName3 = "";
            TotalAmount3 = 0;
        }

        public void _PaidCommand4()
        {
            ProcessPayment("TABLE4");
            InvoiceDetailsList4.Clear();
            CustomerName4 = "";
            TotalAmount4 = 0;
        }

        public void _PaidCommand5()
        {
            ProcessPayment("TABLE5");
            InvoiceDetailsList5.Clear();
            CustomerName5 = "";
            TotalAmount5 = 0;
        }

        public void _PaidCommand6()
        {
            ProcessPayment("TABLE6");
            InvoiceDetailsList6.Clear();
            CustomerName6 = "";
            TotalAmount6 = 0;
        }

        public void _PaidCommand7()
        {
            ProcessPayment("TABLE7");
            InvoiceDetailsList7.Clear();
            CustomerName7 = "";
            TotalAmount7 = 0;
        }

        public void _PaidCommand8()
        {
            ProcessPayment("TABLE8");
            InvoiceDetailsList8.Clear();
            CustomerName8 = "";
            TotalAmount8 = 0;
        }

        public void _PaidCommand9()
        {
            ProcessPayment("TABLE9");
            InvoiceDetailsList9.Clear();
            CustomerName9 = "";
            TotalAmount9 = 0;
        }

        private void ProcessPayment(string tableId)
        {
            var table = _tablesCollection.Find(t => t.TableId == tableId).FirstOrDefault();

            if (table != null && table.Status)
            {
                var updateTable = Builders<Tables>.Update.Set(t => t.Status, false);

                _tablesCollection.UpdateOne(t => t.TableId == tableId, updateTable);

                var unpaidInvoice = _invoicesCollection.Find(i => i.Table.TableId == tableId && i.Status == false).FirstOrDefault();

                if (unpaidInvoice != null)
                {
                    var updateInvoice = Builders<Invoices>.Update.Set(i => i.Status, true);

                    _invoicesCollection.UpdateOne(i => i.InvoiceId == unpaidInvoice.InvoiceId, updateInvoice);

                    MessageBox.Show($"Payment for {tableId} completed!");
                }
                else
                {
                    MessageBox.Show($"No unpaid invoice found for {tableId}.");
                }
            }
            else
            {
                MessageBox.Show($"{tableId} not found or already paid.");
            }
        }

        public ObservableCollection<InvoiceDetails> InvoiceDetailsList1 { get; set; }

        public ObservableCollection<InvoiceDetails> InvoiceDetailsList2 { get; set; }

        public ObservableCollection<InvoiceDetails> InvoiceDetailsList3 { get; set; }

        public ObservableCollection<InvoiceDetails> InvoiceDetailsList4 { get; set; }

        public ObservableCollection<InvoiceDetails> InvoiceDetailsList5 { get; set; }

        public ObservableCollection<InvoiceDetails> InvoiceDetailsList6 { get; set; }

        public ObservableCollection<InvoiceDetails> InvoiceDetailsList7 { get; set; }

        public ObservableCollection<InvoiceDetails> InvoiceDetailsList8 { get; set; }

        public ObservableCollection<InvoiceDetails> InvoiceDetailsList9 { get; set; }

        private readonly IMongoCollection<Tables> _tablesCollection;

        private IMongoCollection<Tables> GetTablesCollection()
        {
            // Set your MongoDB connection string and database name
            string connectionString =
                "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "Restaurant_Management_Application"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Tables>("Tables");
        }

        private readonly IMongoCollection<Invoices> _invoicesCollection;

        private readonly IMongoCollection<InvoiceDetails> _invoiceDetailsCollection;

        private IMongoCollection<Invoices> GetInvoicesCollection()
        {
            // Set your MongoDB connection string and database name
            string connectionString =
                "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "Restaurant_Management_Application"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Invoices>("Invoices");
        }

        private IMongoCollection<InvoiceDetails> GetInvoiceDetailsCollection()
        {
            // Set your MongoDB connection string and database name
            string connectionString =
                "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "Restaurant_Management_Application"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<InvoiceDetails>("InvoiceDetails");
        }

        public ICommand PaidCommand1 { get; set; }

        public ICommand PaidCommand2 { get; set; }

        public ICommand PaidCommand3 { get; set; }

        public ICommand PaidCommand4 { get; set; }

        public ICommand PaidCommand5 { get; set; }

        public ICommand PaidCommand6 { get; set; }

        public ICommand PaidCommand7 { get; set; }

        public ICommand PaidCommand8 { get; set; }

        public ICommand PaidCommand9 { get; set; }

        public TableVM()
        {
            _tablesCollection = GetTablesCollection();
            _invoicesCollection = GetInvoicesCollection();
            _invoiceDetailsCollection = GetInvoiceDetailsCollection();

            InitializeObservableCollection();

            InitializeCommand();

            LoadInvoiceDetail();
        }



    }
}
