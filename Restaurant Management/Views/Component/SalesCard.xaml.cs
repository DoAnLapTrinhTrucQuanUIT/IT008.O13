using System;
using System.Collections.Generic;
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

namespace Restaurant_Management.Views.Component
{
    /// <summary>
    /// Interaction logic for SalesCard.xaml
    /// </summary>
    public partial class SalesCard : UserControl
    {
        public SalesCard()
        {
            InitializeComponent();
            DataContext = this;
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(string), typeof(SalesCard));
        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(SalesCard), new PropertyMetadata(""));
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty QuantityProperty = DependencyProperty.Register("Quantity", typeof(string), typeof(SalesCard), new PropertyMetadata(""));
        public string Quantity
        {
            get { return (string)GetValue(QuantityProperty); }
            set { SetValue(QuantityProperty, value); }
        }

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register("Foreground", typeof(string), typeof(SalesCard), new PropertyMetadata(""));
        public string Foreground
        {
            get { return (string)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }
    }
}
