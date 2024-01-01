using MongoDB.Driver;
using Restaurant_Management.Models;
using Restaurant_Management.Utilities;
using Restaurant_Management.Views;
using Restaurant_Management.Views.Component;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        public ICommand DeleteItemCommand { get; set; }
        
        public ICommand DeleteAllItemCommand { get; set; }

        public ICommand AddToTempMenuCommand { get; set; }

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
        
        

        public MenuVM()
        {
            _MenuItems = GetMenuItems();
            LoadMenuItem();

            TempMenuItemsList = new ObservableCollection<TempMenuItems>();
            AddToTempMenuCommand = new RelayCommand<FoodCard>((p) => true, (p) => AddToTempMenu(p));
            DeleteItemCommand = new RelayCommand<TempMenuItems>((p) => true, (p) => DeleteItem(p));
            DeleteAllItemCommand = new RelayCommand<object>((p) => true, (p) => DeleteAllItem());
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
            var projection = Builders<MenuItems>.Projection
           .Exclude(item => item.Image);

            var items = _MenuItems.Find(Builders<MenuItems>.Filter.Empty).Project<MenuItems>(projection).ToList();

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
                        // Các trường hợp khác nếu có
                }
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
    }
}
