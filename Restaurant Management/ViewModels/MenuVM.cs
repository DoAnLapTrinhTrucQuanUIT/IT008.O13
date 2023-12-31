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
        private int _currentPage = 0;
        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
                OnPropertyChanged(nameof(MainCourseDisplayedFoodCards));
            }
        }
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
                // Call a method to update the CustomerName based on the selected ID
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

        private readonly IMongoCollection<MenuItems> _MenuItems;
        private IMongoCollection<MenuItems> GetMenuItems()
        {
            // Set your MongoDB connection string and database name
            string connectionString =
                "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "Restaurant_Management_Application"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<MenuItems>("MenuItems");
        }

        private readonly IMongoCollection<Tables> _Tables;
        private IMongoCollection<Tables> GetTables()
        {
            // Set your MongoDB connection string and database name
            string connectionString =
                "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "Restaurant_Management_Application"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Tables>("Tables");
        }

        private readonly IMongoCollection<Customers> _Customers;
        private IMongoCollection<Customers> GetCustomers()
        {
            // Set your MongoDB connection string and database name
            string connectionString =
                "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "Restaurant_Management_Application"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Customers>("Customers");
        }

        private readonly IMongoCollection<Invoices> _Invoices;
        private IMongoCollection<Invoices> GetInvoices()
        {
            // Set your MongoDB connection string and database name
            string connectionString =
                "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "Restaurant_Management_Application"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Invoices>("_Invoices");
        }


        private readonly IMongoCollection<Employees> _Employees;
        private IMongoCollection<Employees> GetEmployees()
        {
            // Set your MongoDB connection string and database name
            string connectionString =
                "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "Restaurant_Management_Application"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Employees>("Employees");
        }
        private readonly IMongoCollection<InvoiceDetails> _InvoiceDetails;
        private IMongoCollection<InvoiceDetails> GetInvoiceDetails()
        {
            // Set your MongoDB connection string and database name
            string connectionString =
                "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "Restaurant_Management_Application"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<InvoiceDetails>("InvoiceDetails");
        }

        public ICommand AddToTempMenuCommand { get; set; }
        public ICommand NextPageCommand => new RelayCommand(NextPage);
        public ICommand PreviousPageCommand => new RelayCommand(PreviousPage);
        public ICommand DeleteItemCommand { get; set; }
        public ICommand DeleteAllItemCommand { get; set; }
        public ICommand ConfirmItemCommand { get; set; }



        public MenuVM()
        {
            _MenuItems = GetMenuItems();
            _Tables = GetTables();
            _Customers = GetCustomers();
            _Employees = GetEmployees();
            _InvoiceDetails = GetInvoiceDetails();
            _Invoices = GetInvoices();
            LoadMenuItem();

            TempMenuItemsList = new ObservableCollection<TempMenuItems>();
            AddToTempMenuCommand = new RelayCommand<FoodCard>((p) => true, (p) => AddToTempMenu(p));
            DeleteItemCommand = new RelayCommand<TempMenuItems>((p) => true, (p) => DeleteItem(p));
            DeleteAllItemCommand = new RelayCommand<object>((p) => true, (p) => DeleteAllItem());
            ConfirmItemCommand = new RelayCommand<MenuView>((p) => true, (p) => ConfirmItem(p));
        }
        private void AddToTempMenu(FoodCard foodCard)
        {
            var existingItem = TempMenuItemsList.FirstOrDefault(item => item.MenuItem.Name == foodCard.FoodName);

            if (existingItem != null)
            {
                // Nếu mục đã tồn tại, tăng số lượng lên 1
                existingItem.Quantity++;
            }
            else
            {
                // Nếu mục chưa tồn tại, thêm một mục mới với Quantity là 1
                var tempMenuItem = new TempMenuItems
                {
                    MenuItem = new MenuItems
                    {
                        Name = foodCard.FoodName,
                        Price = foodCard.FoodPrice,
                        // Các thuộc tính khác nếu cần
                    },
                    Quantity = 1 // Số lượng mặc định là 1 khi món ăn được thêm vào danh sách tạm thời
                };
                TempMenuItemsList.Add(tempMenuItem);
            }
            OnPropertyChanged(nameof(TempMenuItemsList));
        }
        private void LoadMenuItem()
        {
            var items = _MenuItems.Find(Builders<MenuItems>.Filter.Empty).ToList();
            ItemList = new ObservableCollection<MenuItems>(items);

            MainCourseList = new ObservableCollection<MenuItems>();
            AppetizerList = new ObservableCollection<MenuItems>();
            LightDishList = new ObservableCollection<MenuItems>();
            DessertList = new ObservableCollection<MenuItems>();
            BeverageList = new ObservableCollection<MenuItems>();

            foreach (var item in ItemList)
            {
                switch (item.Category)
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

            var emptyTables = _Tables.Find(tb => tb.Status == false).ToList();
            EmptyTablesList = new ObservableCollection<string>(emptyTables.Select(tb => tb.TableName));

            var customer = _Customers.Find(Builders<Customers>.Filter.Empty).ToList();
            CustomersIdList = new ObservableCollection<string>(customer.Select(cus => cus.CustomerId));
        }
        public IEnumerable<MenuItems> MainCourseDisplayedFoodCards
        {
            get
            {
                int startIndex = CurrentPage * 3;
                return MainCourseList.Skip(startIndex).Take(3);
            }
        }
        public IEnumerable<MenuItems> AppetizerDisplayedFoodCards
        {
            get
            {
                int startIndex = CurrentPage * 3;
                return AppetizerList.Skip(startIndex).Take(3);
            }
        }
        public IEnumerable<MenuItems> LightDishDisplayedFoodCards
        {
            get
            {
                int startIndex = CurrentPage * 3;
                return LightDishList.Skip(startIndex).Take(3);
            }
        }
        public IEnumerable<MenuItems> DessertDisplayedFoodCards
        {
            get
            {
                int startIndex = CurrentPage * 3;
                return DessertList.Skip(startIndex).Take(3);
            }
        }
        public IEnumerable<MenuItems> BeverageDisplayedFoodCards
        {
            get
            {
                int startIndex = CurrentPage * 3;
                return BeverageList.Skip(startIndex).Take(3);
            }
        }
        private void NextPage(object obj)
        {
            int totalPages = (int)Math.Ceiling((double)MainCourseList.Count / 3);
            if (CurrentPage < totalPages - 1)
            {
                CurrentPage++;
            }
        }
        private void PreviousPage(object obj)
        {
            if (CurrentPage > 0)
            {
                CurrentPage--;
            }
        }
        private void DeleteItem(TempMenuItems tempMenuItems)
        {
            if (tempMenuItems != null)
            {
                TempMenuItemsList.Remove(tempMenuItems);
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
            // Use the selected customer ID to retrieve the customer name from your data source
            // Replace this with your actual logic to fetch customer name based on ID
            var customer = _Customers.Find(c => c.CustomerId == SelectedCustomerId).FirstOrDefault();
            CustomerName = customer.FullName.ToString(); // Update CustomerName property
        }

        private void ConfirmItem(MenuView menuView)
        {
            Customers customer = _Customers.Find(c => c.CustomerId == SelectedCustomerId).FirstOrDefault();
            Employees employee = _Employees.Find(em => em.EmployeeId == Const.Instance.UserId).FirstOrDefault();

            string tableIdSelected = menuView.tableIdSelected.ToString();

            Tables table = _Tables.Find(c => c.TableId == tableIdSelected).FirstOrDefault();

            if (table != null)
            {
                // Set the Status to true
                table.Status = true;
                // Update the table status in the MongoDB collection
                var tableFilter = Builders<Tables>.Filter.Eq(t => t.TableId, tableIdSelected);
                var update = Builders<Tables>.Update.Set(t => t.Status, true);
                _Tables.UpdateOne(tableFilter, update);
            }
            string invoiceId = GenerateRandomInvoiceId();
            string invoiceDetailsId = GenerateRandomInvoiceDetailId(invoiceId);

            // Create a list to store InvoiceDetails
            List<InvoiceDetails> invoiceDetailsList = new List<InvoiceDetails>();

            // Calculate the total amount
            double totalAmount = 0;

            foreach (var tempInvoiceDetail in TempMenuItemsList)
            {
                MenuItems tempMenuItem = tempInvoiceDetail.MenuItem;
                int tempQuantity = tempInvoiceDetail.Quantity;

                // Create InvoiceDetail
                InvoiceDetails invoiceDetail = new InvoiceDetails
                {
                    InvoiceDetailId = invoiceDetailsId,
                    Item = tempMenuItem,
                    Quantity = tempQuantity,
                    Amount = tempMenuItem.Price * tempQuantity,
                };

                // Add InvoiceDetail to the list
                invoiceDetailsList.Add(invoiceDetail);

                // Sum up the Amount
                totalAmount += invoiceDetail.Amount;
            }

            // Create the Invoice with the total amount
            Invoices Invoice = new Invoices
            {
                InvoiceId = invoiceId,
                Employee = employee,
                Customer = customer,
                Table = table,
                CreatedDate = DateTime.Now,
                Status = false,
                TotalAmount = totalAmount, // Set the TotalAmount
            };

            // Save the Invoice and InvoiceDetails to MongoDB
            _Invoices.InsertOne(Invoice);
            _InvoiceDetails.InsertMany(invoiceDetailsList);
        }



        string GenerateRandomInvoiceId()
        {
            // Lấy `InvoiceId` lớn nhất hiện có
            var maxInvoiceId = _Invoices.AsQueryable()
                .OrderByDescending(i => i.InvoiceId)
                .FirstOrDefault()?.InvoiceId;

            // Tạo `InvoiceId` mới với số thứ tự kế tiếp
            string newInvoiceId = GenerateNextInvoiceId(maxInvoiceId);

            // Kiểm tra nếu `InvoiceId` mới đã tồn tại
            while (CheckInvoice(newInvoiceId))
            {
                newInvoiceId = GenerateNextInvoiceId(newInvoiceId);
            }

            return newInvoiceId;
        }

        string GenerateNextInvoiceId(string currentMaxInvoiceId)
        {
            if (string.IsNullOrEmpty(currentMaxInvoiceId))
            {
                return "INV1";
            }

            // Trích xuất số từ `InvoiceId` lớn nhất hiện có
            string maxNumberStr = currentMaxInvoiceId.Substring(3);
            if (int.TryParse(maxNumberStr, out int maxNumber))
            {
                // Tạo `InvoiceId` mới với số thứ tự kế tiếp
                return "INV" + (maxNumber + 1).ToString();
            }

            // Trong trường hợp không thành công, trả về `INV1`
            return "INV1";
        }

        bool CheckInvoice(string invoiceId)
        {
            return _Invoices.AsQueryable().Any(temp => temp.InvoiceId == invoiceId);
        }



        string GenerateInvoiceDetailId(string invoiceId, int detailNumber)
        {
            // Tạo `InvoiceDetailId` từ `invoiceId` và `detailNumber`
            return $"{invoiceId}_DET{detailNumber}";
        }

        string GenerateRandomInvoiceDetailId(string invoiceId) // get invoice as a element
        {
            // Lấy `InvoiceDetailId` lớn nhất cho `invoiceId` hiện có
            var maxDetailId = _InvoiceDetails.AsQueryable()
                .Where(d => d.Invoice.InvoiceId == invoiceId)
                .OrderByDescending(d => d.InvoiceDetailId)
                .FirstOrDefault()?.InvoiceDetailId;

            // Trích xuất số từ `InvoiceDetailId` lớn nhất hiện có
            string maxNumberStr = maxDetailId?.Substring(invoiceId.Length + 4);
            if (int.TryParse(maxNumberStr, out int maxNumber))
            {
                // Tạo `InvoiceDetailId` mới với số thứ tự kế tiếp
                string newDetailId = GenerateInvoiceDetailId(invoiceId, maxNumber + 1);

                // Kiểm tra nếu `InvoiceDetailId` mới đã tồn tại
                while (CheckInvoiceDetail(newDetailId))
                {
                    maxNumber++;
                    newDetailId = GenerateInvoiceDetailId(invoiceId, maxNumber + 1);
                }

                return newDetailId;
            }

            // Trong trường hợp không thành công, trả về `InvoiceId_DET1`
            return GenerateInvoiceDetailId(invoiceId, 1);
        }

        bool CheckInvoiceDetail(string invoiceDetailId)
        {
            return _InvoiceDetails.AsQueryable().Any(temp => temp.InvoiceDetailId == invoiceDetailId);
        }

    }
}
