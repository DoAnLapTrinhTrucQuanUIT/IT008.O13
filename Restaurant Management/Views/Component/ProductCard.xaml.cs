using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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

namespace Restaurant_Management.Views.Component
{
    /// <summary>
    /// Interaction logic for ProductCard.xaml
    /// </summary>
    public partial class ProductCard : UserControl
    {
        public ProductCard()
        {
            InitializeComponent();
        }
      
        public static readonly DependencyProperty FoodNameProperty =
            DependencyProperty.Register("FoodName", typeof(string), typeof(ProductCard));

        public string FoodName
        {
            get { return (string)GetValue(FoodNameProperty); }
            set { SetValue(FoodNameProperty, value); }
        }

        public static readonly DependencyProperty FoodPriceProperty =
            DependencyProperty.Register("FoodPrice", typeof(double), typeof(ProductCard));

        public double FoodPrice
        {
            get { return (double)GetValue(FoodPriceProperty); }
            set { SetValue(FoodPriceProperty, value); }
        }

        public static readonly DependencyProperty FoodDescriptionProperty =
       DependencyProperty.Register("FoodDescription", typeof(string), typeof(ProductCard));

        public string FoodDescription
        {
            get { return (string)GetValue(FoodDescriptionProperty); }
            set { SetValue(FoodDescriptionProperty, value); }
        }
    }
}
