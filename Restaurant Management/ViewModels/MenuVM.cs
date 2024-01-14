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
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace Restaurant_Management.ViewModels
{
    public class MenuVM : Utilities.ViewModelBase
    {
        public ObservableCollection<MenuItems> MainCourseList { get; set; }

        public ObservableCollection<MenuItems> AppetizerList { get; set; }

        public ObservableCollection<MenuItems> LightDishList { get; set; }

        public ObservableCollection<MenuItems> DessertList { get; set; }

        public ObservableCollection<MenuItems> BeverageList { get; set; }

        private ObservableCollection<String> _emptyTablesList;
      
        public ObservableCollection<String> EmptyTablesList
        {
            get { return _emptyTablesList; }
            set
            {
                _emptyTablesList = value;
                OnPropertyChanged(nameof(EmptyTablesList));
            }
        }

        private ObservableCollection<String> _customersList;
        
        public ObservableCollection<String> CustomersIdList
        {
            get { return _customersList; }
            set
            {
                _customersList = value;
                OnPropertyChanged(nameof(CustomersIdList));
            }
        }

        private ObservableCollection<MenuItems> _itemlist;
        
        public ObservableCollection<MenuItems> ItemList
        {
            get { return _itemlist; }
            set
            {
                _itemlist = value;
                OnPropertyChanged(nameof(ItemList));
            }
        }

        private ObservableCollection<TempMenuItems> _tempMenuItemsList;

        public ObservableCollection<TempMenuItems> TempMenuItemsList
        {
            get { return _tempMenuItemsList; }
            set
            {
                _tempMenuItemsList = value;
                OnPropertyChanged(nameof(TempMenuItemsList));
            }
        }

        private string _selectedCustomerId;
        
        public string SelectedCustomerId
        {
            get { return _selectedCustomerId; }
            set
            {
                _selectedCustomerId = value;
                OnPropertyChanged(nameof(SelectedCustomerId));
                UpdateCustomerName();
            }
        }
        
        private string _customerName;
        
        public string CustomerName
        {
            get { return _customerName; }
            set
            {
                _customerName = value;
                OnPropertyChanged(nameof(CustomerName));
            }
        }

        private string _selectedTableItem;
        
        public string SelectedTableItem
        {
            get { return _selectedTableItem; }
            set
            {
                _selectedTableItem = value;
                OnPropertyChanged(nameof(SelectedTableItem));
            }
        }

        public IEnumerable<MenuItems> MainCourseDisplayedFoodCards
        {
            get
            {
                return MainCourseList;
            }
        }

        public IEnumerable<MenuItems> AppetizerDisplayedFoodCards
        {
            get
            {
                return AppetizerList;
            }
        }

        public IEnumerable<MenuItems> LightDishDisplayedFoodCards
        {
            get
            {
                return LightDishList;
            }
        }

        public IEnumerable<MenuItems> DessertDisplayedFoodCards
        {
            get
            {
                return DessertList;
            }
        }

        public IEnumerable<MenuItems> BeverageDisplayedFoodCards
        {
            get
            {
                return BeverageList;
            }
        }

        private readonly IMongoCollection<MenuItems> _MenuItems;

        private readonly IMongoCollection<Tables> _Tables;

        private readonly IMongoCollection<Customers> _Customers;

        private readonly IMongoCollection<Invoices> _Invoices;

        private readonly IMongoCollection<Employees> _Employees;

        private readonly IMongoCollection<InvoiceDetails> _InvoiceDetails;

        private IMongoCollection<MenuItems> GetMenuItems()
        {
            string connectionString = "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/"; 
            
            string databaseName = "Restaurant_Management_Application";

            var client = new MongoClient(connectionString);
            
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<MenuItems>("MenuItems");

        }
        
        private IMongoCollection<Tables> GetTables()
        {
            string connectionString = "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/";

            string databaseName = "Restaurant_Management_Application";

            var client = new MongoClient(connectionString);

            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Tables>("Tables");
        }
        
        private IMongoCollection<Customers> GetCustomers()
        {
            string connectionString = "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/";
            
            string databaseName = "Restaurant_Management_Application"; 

            var client = new MongoClient(connectionString);
            
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Customers>("Customers");
        }

        private IMongoCollection<Invoices> GetInvoices()
        {
            string connectionString = "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/";

            string databaseName = "Restaurant_Management_Application";

            var client = new MongoClient(connectionString);

            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Invoices>("Invoices");
        }

        private IMongoCollection<Employees> GetEmployees()
        {
            string connectionString = "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/";

            string databaseName = "Restaurant_Management_Application";

            var client = new MongoClient(connectionString);

            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Employees>("Employees");
        }

        private IMongoCollection<InvoiceDetails> GetInvoiceDetails()
        {
            string connectionString = "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/";
            
            string databaseName = "Restaurant_Management_Application"; 

            var client = new MongoClient(connectionString);
            
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<InvoiceDetails>("InvoiceDetails");
        }
        
        public ICommand DeleteItemCommand { get; set; }
   
        public ICommand DeleteAllItemCommand { get; set; }
        
        public ICommand ConfirmItemCommand { get; set; }

        public ICommand AddToTempMenuCommand { get; set; }

        public MenuVM()
        {
            _MenuItems = GetMenuItems();

            _Tables = GetTables();

            _Customers = GetCustomers();

            _Employees = GetEmployees();

            _InvoiceDetails = GetInvoiceDetails();

            _Invoices = GetInvoices();

            LoadMenuItem();

            InitializeCommand();
        }

        public void LoadCustomer()
        {
            var customer = _Customers.Find(Builders<Customers>.Filter.Empty).ToList();
            
            CustomersIdList = new ObservableCollection<string>(customer.Select(cus => cus.CustomerId));
        }
        
        public void LoadTable()
        {
            var emptyTables = _Tables.Find(tb => tb.Status == false).ToList();
            
            EmptyTablesList = new ObservableCollection<string>(emptyTables.Select(tb => tb.TableName));
        }

        private void InitializeCommand()
        {
            TempMenuItemsList = new ObservableCollection<TempMenuItems>();

            AddToTempMenuCommand = new RelayCommand<FoodCard>((p) => true, (p) => AddToTempMenu(p));

            DeleteItemCommand = new RelayCommand<TempMenuItems>((p) => true, (p) => DeleteItem(p));

            DeleteAllItemCommand = new RelayCommand<object>((p) => true, (p) => DeleteAllItem());

            ConfirmItemCommand = new RelayCommand<MenuView>((p) => true, (p) => ConfirmItem());
        }

        private void AddToTempMenu(FoodCard foodCard)
        {
            var existingItem = TempMenuItemsList.FirstOrDefault(item => item.MenuItem.Name == foodCard.FoodName);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                var tempMenuItem = new TempMenuItems
                {
                    MenuItem = new MenuItems
                    {
                        ItemId = foodCard.FoodId,
                        Name = foodCard.FoodName,
                        Price = foodCard.FoodPrice,
                    },
                    Quantity = 1
                };
                TempMenuItemsList.Add(tempMenuItem);
            }
            OnPropertyChanged(nameof(TempMenuItemsList));
        }

        private void LoadMenuItem()
        {
            var items = _MenuItems.Find(Builders<MenuItems>.Filter.Empty).ToList();

            ItemList = new ObservableCollection<MenuItems>(items);

            var groupedItems = items.GroupBy(item => item.Category);

            MainCourseList = new ObservableCollection<MenuItems>();

            AppetizerList = new ObservableCollection<MenuItems>();

            LightDishList = new ObservableCollection<MenuItems>();

            DessertList = new ObservableCollection<MenuItems>();

            BeverageList = new ObservableCollection<MenuItems>();

            foreach (var group in groupedItems)
            {
                foreach (var item in group)
                {
                    switch (group.Key)
                    {
                        case "Main Course":
                            MainCourseList.Add(item);
                            break;
                        case "Appetizer":
                            AppetizerList.Add(item);
                            break;
                        case "Light Dish":
                            LightDishList.Add(item);
                            break;
                        case "Dessert":
                            DessertList.Add(item);
                            break;
                        case "Beverage":
                            BeverageList.Add(item);
                            break;
                    }
                }
            }

            LoadCustomer();

            LoadTable();
        }

        private void DeleteItem(TempMenuItems tempMenuItems)
        {
            if (tempMenuItems != null)
            {
                if (tempMenuItems.Quantity > 1)
                {
                    tempMenuItems.Quantity--;
                }
                else
                {
                    TempMenuItemsList.Remove(tempMenuItems);
                }

                OnPropertyChanged(nameof(TempMenuItemsList));
            }
        }

        private void DeleteAllItem()
        {
            TempMenuItemsList.Clear();
            OnPropertyChanged(nameof(TempMenuItemsList));
        }
        
        private void UpdateCustomerName()
        {
            var customer = _Customers.Find(c => c.CustomerId == SelectedCustomerId).FirstOrDefault();

            if (customer!=null)
            {
                CustomerName = customer.FullName.ToString();
            }
            else
            {
                CustomerName=null;
            }
        }

        private void ConfirmItem()
        {
            if (SelectedTableItem == null)
            {
                System.Windows.MessageBox.Show("Please enter complete information!", "Notification");
            }
            else
            {
                if (CustomerName != null && SelectedCustomerId == null)
                {
                    var customer = new Customers
                    {
                        CustomerId = GenerateRandomCustomerId(),
                        FullName = CustomerName,
                        PhoneNumber = "",
                        Email = "",
                        Address = "",
                        Gender = null,
                        RegistrationDate = DateTime.Now,
                        Sales = 0
                    };

                    _Customers.InsertOne(customer);

                    Employees employee = _Employees.Find(em => em.EmployeeId == Const.Instance.UserId).FirstOrDefault();

                    string tableIdSelected = SelectedTableItem.ToString();

                    Tables table = _Tables.Find(c => c.TableName == tableIdSelected).FirstOrDefault();

                    // Set the Status to true
                    table.Status = true;

                    // Update the table status in the MongoDB collection
                    var tableFilter = Builders<Tables>.Filter.Eq(t => t.TableName, tableIdSelected);
                    var update = Builders<Tables>.Update.Set(t => t.Status, true);
                    _Tables.UpdateOne(tableFilter, update);


                    string invoiceId = GenerateRandomInvoiceId();
                    // Create the Invoice with the total amount
                    Invoices Invoice = new Invoices
                    {
                        InvoiceId = invoiceId,
                        Employee = employee,
                        Customer = customer,
                        Table = table,
                        CreatedDate = DateTime.Now,
                        Status = false
                    };
                    _Invoices.InsertOne(Invoice);

                    // Calculate the total amount
                    double totalAmount = 0;

                    foreach (var tempInvoiceDetail in TempMenuItemsList)
                    {
                        string invoiceDetailsId = GenerateRandomInvoiceDetailId(invoiceId);
                        MenuItems tempMenuItem = tempInvoiceDetail.MenuItem;
                        int tempQuantity = tempInvoiceDetail.Quantity;

                        // Create InvoiceDetail
                        InvoiceDetails invoiceDetail = new InvoiceDetails
                        {
                            InvoiceDetailId = invoiceDetailsId,
                            Invoice = Invoice,
                            Item = tempMenuItem,
                            Quantity = tempQuantity,
                            Amount = tempMenuItem.Price * tempQuantity,
                        };
                        _InvoiceDetails.InsertOne(invoiceDetail);

                        // Sum up the Amount
                        totalAmount += invoiceDetail.Amount;
                    }

                    var customerFilter = Builders<Customers>.Filter.Eq(c => c.CustomerId, customer.CustomerId);
                    var updateCustomerSales = Builders<Customers>.Update.Inc(c => c.Sales, totalAmount);
                    _Customers.UpdateOne(customerFilter, updateCustomerSales);


                    var filter = Builders<Invoices>.Filter.Eq(i => i.InvoiceId, invoiceId);
                    var updateTotalAmount = Builders<Invoices>.Update.Set(i => i.TotalAmount, totalAmount);
                    _Invoices.UpdateOne(filter, updateTotalAmount);

                    SelectedTableItem = null;
                    CustomerName = null;
                    SelectedCustomerId = null;
                    LoadTable();
                    DeleteAllItem();
                    System.Windows.MessageBox.Show("Invoice created successfully", "Notification");
                }
                else
                {
                    Customers customer = _Customers.Find(c => c.CustomerId == SelectedCustomerId).FirstOrDefault();

                    Employees employee = _Employees.Find(em => em.EmployeeId == Const.Instance.UserId).FirstOrDefault();

                    string tableIdSelected = SelectedTableItem.ToString();

                    Tables table = _Tables.Find(c => c.TableName == tableIdSelected).FirstOrDefault();

                    // Set the Status to true
                    table.Status = true;

                    // Update the table status in the MongoDB collection
                    var tableFilter = Builders<Tables>.Filter.Eq(t => t.TableName, tableIdSelected);
                    var update = Builders<Tables>.Update.Set(t => t.Status, true);
                    _Tables.UpdateOne(tableFilter, update);


                    string invoiceId = GenerateRandomInvoiceId();
                    // Create the Invoice with the total amount
                    Invoices Invoice = new Invoices
                    {
                        InvoiceId = invoiceId,
                        Employee = employee,
                        Customer = customer,
                        Table = table,
                        CreatedDate = DateTime.Now,
                        Status = false
                    };
                    _Invoices.InsertOne(Invoice);

                    // Calculate the total amount
                    double totalAmount = 0;

                    foreach (var tempInvoiceDetail in TempMenuItemsList)
                    {
                        string invoiceDetailsId = GenerateRandomInvoiceDetailId(invoiceId);
                        MenuItems tempMenuItem = tempInvoiceDetail.MenuItem;
                        int tempQuantity = tempInvoiceDetail.Quantity;

                        // Create InvoiceDetail
                        InvoiceDetails invoiceDetail = new InvoiceDetails
                        {
                            InvoiceDetailId = invoiceDetailsId,
                            Invoice = Invoice,
                            Item = tempMenuItem,
                            Quantity = tempQuantity,
                            Amount = tempMenuItem.Price * tempQuantity,
                        };
                        _InvoiceDetails.InsertOne(invoiceDetail);

                        // Sum up the Amount
                        totalAmount += invoiceDetail.Amount;
                    }

                    var customerFilter = Builders<Customers>.Filter.Eq(c => c.CustomerId, SelectedCustomerId);
                    var updateCustomerSales = Builders<Customers>.Update.Inc(c => c.Sales, totalAmount);
                    _Customers.UpdateOne(customerFilter, updateCustomerSales);

                    var filter = Builders<Invoices>.Filter.Eq(i => i.InvoiceId, invoiceId);
                    var updateTotalAmount = Builders<Invoices>.Update.Set(i => i.TotalAmount, totalAmount);
                    _Invoices.UpdateOne(filter, updateTotalAmount);

                    SelectedTableItem = null;
                    CustomerName = null;
                    SelectedCustomerId = null;
                    LoadTable();
                    DeleteAllItem();
                    System.Windows.MessageBox.Show("Invoice created successfully", "Notification");
                }
            }
        }

        private string GenerateRandomInvoiceId()
        {
            var maxInvoiceId = _Invoices.AsQueryable()
                .OrderByDescending(i => i.InvoiceId)
                .FirstOrDefault()?.InvoiceId;

            string newInvoiceId = GenerateNextInvoiceId(maxInvoiceId);

            while (CheckInvoice(newInvoiceId))
            {
                newInvoiceId = GenerateNextInvoiceId(newInvoiceId);
            }

            return newInvoiceId;
        }

        private string GenerateNextInvoiceId(string currentMaxInvoiceId)
        {
            if (string.IsNullOrEmpty(currentMaxInvoiceId))
            {
                return "INV1";
            }

            string maxNumberStr = currentMaxInvoiceId.Substring(3);

            if (int.TryParse(maxNumberStr, out int maxNumber))
            {
                return "INV" + (maxNumber + 1).ToString();
            }

            return "INV1";
        }

        private bool CheckInvoice(string invoiceId)
        {
            return _Invoices.AsQueryable().Any(temp => temp.InvoiceId == invoiceId);
        }

        private string GenerateInvoiceDetailId(string invoiceId, int detailNumber)
        {
            return $"{invoiceId}_DET{detailNumber}";
        }

        private string GenerateRandomInvoiceDetailId(string invoiceId)
        {
            var maxDetailId = _InvoiceDetails.AsQueryable()
                .Where(d => d.Invoice.InvoiceId == invoiceId)
                .OrderByDescending(d => d.InvoiceDetailId)
                .FirstOrDefault()?.InvoiceDetailId;

            string maxNumberStr = maxDetailId?.Substring(invoiceId.Length + 4);

            if (int.TryParse(maxNumberStr, out int maxNumber))
            {
                int newDetailNumber = maxNumber + 1;

                string newDetailId = GenerateInvoiceDetailId(invoiceId, newDetailNumber);

                while (CheckInvoiceDetail(newDetailId))
                {
                    newDetailNumber++;

                    newDetailId = GenerateInvoiceDetailId(invoiceId, newDetailNumber);
                }

                return newDetailId;
            }

            return GenerateInvoiceDetailId(invoiceId, 1);
        }

        private bool CheckInvoiceDetail(string invoiceDetailId)
        {
            return _InvoiceDetails.AsQueryable().Any(temp => temp.InvoiceDetailId == invoiceDetailId);
        }


        string GenerateRandomCustomerId()
        {
            // Lấy `CustomerId` lớn nhất hiện có
            var maxCustomerId = _Customers.AsQueryable()
                .OrderByDescending(c => c.CustomerId)
                .FirstOrDefault()?.CustomerId;

            // Tạo `CustomerId` mới với số thứ tự kế tiếp
            string newCustomerId = GenerateNextCustomerId(maxCustomerId);

            // Kiểm tra nếu `CustomerId` mới đã tồn tại
            while (Check(newCustomerId))
            {
                newCustomerId = GenerateNextCustomerId(newCustomerId);
            }

            return newCustomerId;
        }

        string GenerateNextCustomerId(string currentMaxCustomerId)
        {
            if (string.IsNullOrEmpty(currentMaxCustomerId))
            {
                return "CUS1";
            }

            // Trích xuất số từ `CustomerId` lớn nhất hiện có
            string maxNumberStr = currentMaxCustomerId.Substring(3);
            if (int.TryParse(maxNumberStr, out int maxNumber))
            {
                // Tạo `CustomerId` mới với số thứ tự kế tiếp
                return "CUS" + (maxNumber + 1).ToString();
            }

            // Trong trường hợp không thành công, trả về `CUS1`
            return "CUS1";
        }

        bool Check(string customerId)
        {
            return _Customers.AsQueryable().Any(temp => temp.CustomerId == customerId);
        }

    }
}
