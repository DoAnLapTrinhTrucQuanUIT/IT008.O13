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
    public partial class Customer : UserControl
    {
        public Customer()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty TextHeaderProperty = DependencyProperty.Register("TextHeader", typeof(string), typeof(Customer), new PropertyMetadata(""));
        public string TextHeader
        {
            get { return (string)GetValue(TextHeaderProperty); }
            set { SetValue(TextHeaderProperty, value); }
        }

        public static readonly DependencyProperty TextofNumberProperty = DependencyProperty.Register("TextofNumber", typeof(string), typeof(Customer), new PropertyMetadata(""));
        public string TextofNumber
        {
            get { return (string)GetValue(TextofNumberProperty); }
            set { SetValue(TextofNumberProperty, value); }
        }

        public static readonly DependencyProperty TextofPercentProperty = DependencyProperty.Register("TextofPercent", typeof(string), typeof(Customer), new PropertyMetadata(""));
        public string TextofPercent
        {
            get { return (string)GetValue(TextofPercentProperty); }
            set { SetValue(TextofPercentProperty, value); }
        }

        public static readonly DependencyProperty ImagePathStatusProperty =
            DependencyProperty.Register("ImagePathStatus", typeof(string), typeof(Customer));

        public string ImagePathStatus
        {
            get { return (string)GetValue(ImagePathStatusProperty); }
            set { SetValue(ImagePathStatusProperty, value); }
        }

        public static readonly DependencyProperty ImagePathSymbolProperty =
            DependencyProperty.Register("ImagePathSymbol", typeof(string), typeof(Customer));

        public string ImagePathSymbol
        {
            get { return (string)GetValue(ImagePathSymbolProperty); }
            set { SetValue(ImagePathSymbolProperty, value); }
        }

    }
}
