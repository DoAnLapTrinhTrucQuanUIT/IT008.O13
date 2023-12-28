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
    public class ProductsVM : Utilities.ViewModelBase
    {
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
        private readonly IMongoCollection<MenuItems> _MenuItems;

        public ICommand SearchCM { get; set; }
        public ICommand AddItemCM { get; set; }
        public ICommand RemoveItemCommand { get; set; }
        public ICommand EditItemCommand { get; set; }
        public ProductsVM()
        {
            _MenuItems = GetMenuItems();
            LoadMenuItem();
            SearchCM = new RelayCommand<ProductsView>((p) => true, (p) => _Search(p));
            AddItemCM = new RelayCommand<ProductsView>((p) => true, (p) => _AddCustomer(p));
            RemoveItemCommand = new RelayCommand<MenuItems>((item) => true, (item) => _RemoveItem(item));
            EditItemCommand = new RelayCommand<MenuItems>((item) => true, (item) => _EditItem(item));
        }
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
        private void LoadMenuItem()
        {
            var items = _MenuItems.Find(Builders<MenuItems>.Filter.Empty).ToList();
            ItemList = new ObservableCollection<MenuItems>(items);
        }
        private void _Search(ProductsView productsView)
        {
            ObservableCollection<MenuItems> temp = new ObservableCollection<MenuItems>();
            if (!string.IsNullOrEmpty(productsView.txtSearch.Text))
            {
                var filterBuilder = Builders<MenuItems>.Filter;
                FilterDefinition<MenuItems> filter;

                var keyword = productsView.txtSearch.Text;

                filter = filterBuilder.Or(
                    filterBuilder.Regex("itemId", new BsonRegularExpression(keyword, "i")),
                    filterBuilder.Regex("Name", new BsonRegularExpression(keyword, "i")),
                    filterBuilder.Regex("category", new BsonRegularExpression(keyword, "i"))
                );
                var result = _MenuItems.Find(filter).ToList();
                temp = new ObservableCollection<MenuItems>(result);
            }
            else
            {
                var result = _MenuItems.Find(Builders<MenuItems>.Filter.Empty).ToList();
                temp = new ObservableCollection<MenuItems>(result);
            }
            productsView.productDataGrid.ItemsSource = temp;
        }
        private void _AddCustomer(ProductsView productsView)
        {
            AddItem addStaff = new AddItem();
            var window = new Window
            {
                Content = addStaff,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStyle = WindowStyle.None,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();
            LoadMenuItem();
        }
        private void _EditItem(MenuItems menuItem)
        {
            if (menuItem != null)
            {
                EditItemVM editItemViewModel = new EditItemVM(menuItem);
                EditItem editItem = new EditItem();
                editItem.DataContext = editItemViewModel;
                var window = new Window
                {
                    Content = editItem,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    WindowStyle = WindowStyle.None,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                window.Closed += (sender, args) => LoadMenuItem();
            }
        }
        private void _RemoveItem(MenuItems menuItem)
        {
            if (menuItem != null)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {menuItem.Name}?",
                                                          "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _MenuItems.DeleteOne(Builders<MenuItems>.Filter.Eq("itemId", menuItem.ItemId));
                    LoadMenuItem();
                }
            }
        }
    }
}
