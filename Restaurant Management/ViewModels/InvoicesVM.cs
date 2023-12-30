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
using System.Windows.Controls;
using Amazon.Runtime.Documents;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace Restaurant_Management.ViewModels
{
    public class BoughtItems
    {
        public string ITEMID { get; set; }
        public string NAME { get; set; }
        public int QUANTITY { get; set; }
        public double UNITPRICE { get; set; }
        public double TOTALAMOUNT { get; set; }

        public BoughtItems(string ITEMID = "", string NAME = "", int QUANTITY = 0, double UNITPRICE = 0, double TOTALAMOUNT = 0)
        {
            this.ITEMID = ITEMID;
            this.NAME = NAME;
            this.QUANTITY = QUANTITY;
            this.UNITPRICE = UNITPRICE;
            this.TOTALAMOUNT = TOTALAMOUNT;
        }

    }
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

        private ObservableCollection<BoughtItems> _listBoughtItems;
        public ObservableCollection<BoughtItems> ListBoughtItems
        {
            get { return _listBoughtItems; }
            set
            {
                _listBoughtItems = value;
                OnPropertyChanged(nameof(ListBoughtItems));
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
            PrintInvoiceView print = new PrintInvoiceView();

            // Set the height of the PrintInvoiceView
            print.Height = 300 + 35 * invoice.Items.Count();

            // Set data in the PrintInvoiceView
            print.CustomerID.Text = invoice.PaidCustomer.CustomerId.ToString();
            print.CustomerName.Text = invoice.PaidCustomer.FullName.ToString();
            print.PhoneNumber.Text = invoice.PaidCustomer.PhoneNumber.ToString();
            print.EmployeeName.Text = invoice.PaidEmployee.FullName.ToString();
            print.InvoiceDate.Text = invoice.CreatedDate.ToString();
            print.InvoiceID.Text = invoice.InvoiceId.ToString();

            ListBoughtItems = new ObservableCollection<BoughtItems>();
            foreach (var itemGroup in invoice.Items.GroupBy(item => item.ItemId))
            {
                var totalQuantity = itemGroup.Count(); // Count of items with the same ItemId
                var totalAmount = totalQuantity * itemGroup.First().Price;

                BoughtItems boughtItem = new BoughtItems
                {
                    ITEMID = itemGroup.Key, // Assumes ItemId is a string; adjust accordingly
                    NAME = itemGroup.First().Name, // Assumes ItemName is the same for items with the same ItemId
                    QUANTITY = totalQuantity,
                    UNITPRICE = itemGroup.First().Price, // Assumes UnitPrice is the same for items with the same ItemId
                    TOTALAMOUNT = totalAmount,
                };

                ListBoughtItems.Add(boughtItem);
            }


            print.ListMenuItem.ItemsSource = ListBoughtItems;
            print.TotalAmount.Text = invoice.Amount.ToString();

            try
            {
                // Create a SaveFileDialog to allow the user to choose the location to save the PDF
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = $"Invoice_{invoice.InvoiceId}.pdf",
                    DefaultExt = ".pdf",
                    Filter = "PDF documents (.pdf)|*.pdf"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    // Use iTextSharp to export the PrintInvoiceView content to a PDF file
                    ExportToPdf(print.PrintWindow, saveFileDialog.FileName);

                    MessageBox.Show($"Invoice exported Successfully", "Export Successful");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to PDF: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportToPdf(UIElement element, string filePath)
        {
            try
            {
                // Set the desired size for rendering
                element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                element.Arrange(new Rect(new Point(0, 0), element.DesiredSize));

                var renderTargetBitmap = new RenderTargetBitmap(
                    (int)element.RenderSize.Width,
                    (int)element.RenderSize.Height,
                    80,
                    80,
                    PixelFormats.Pbgra32);

                renderTargetBitmap.Render(element);

                using (MemoryStream stream = new MemoryStream())
                {
                    var encoder = new PngBitmapEncoder(); // Use PngBitmapEncoder for better quality
                    encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                    encoder.Save(stream);
                    byte[] byteArray = stream.ToArray();

                    // Convert the byte array to iTextSharp Image
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(byteArray);
                    image.ScaleToFit(PageSize.A4.Width, PageSize.A4.Height); // Adjust scaling as needed

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        var document = new iTextSharp.text.Document();
                        PdfWriter.GetInstance(document, fileStream);
                        document.Open();

                        // Add the image to the PDF document
                        document.Add(image);

                        document.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to PDF: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
