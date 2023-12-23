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

namespace Restaurant_Management.Views
{
    /// <summary>
    /// Interaction logic for CategoryCard.xaml
    /// </summary>
    public partial class CategoryCard : UserControl
    {
        public CategoryCard()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(CategoryCard), new PropertyMetadata(""));
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty CategoryImageProperty = DependencyProperty.Register("CategoryImage", typeof(ImageSource), typeof(CategoryCard));
        public ImageSource CategoryImage
        {
            get { return (ImageSource)GetValue(CategoryImageProperty); }
            set { SetValue(CategoryImageProperty, value); }
        }
    }
}
