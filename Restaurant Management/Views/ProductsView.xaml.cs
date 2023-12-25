using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Restaurant_Management.Views.StaffView;

namespace Restaurant_Management.Views
{
    /// <summary>
    /// Interaction logic for ProductsView.xaml
    /// </summary>
    public partial class ProductsView : UserControl
    {
        public ProductsView()
        {
            InitializeComponent();
            var converter = new BrushConverter();

            ObservableCollection<TestProduct> tests = new ObservableCollection<TestProduct>();

            tests.Add(new TestProduct { Number = "1", ID = "NV01", Name = "Tai", BgColor = (Brush)converter.ConvertFromString("#f9f9f9"), Barcode = "owner", Category = "0328329908"});
            productDataGrid.ItemsSource = tests;
        }
        public class TestProduct
        {
            public string Number { get; set; }
            public string ID { get; set; }
            public string Name { get; set; }
            public string Barcode { get; set; }
            public string Category { get; set; }
            public Brush BgColor { get; set; }
        }
    }
}
