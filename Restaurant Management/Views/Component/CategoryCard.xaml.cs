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
        public static readonly DependencyProperty IsSelectedProperty =
        DependencyProperty.Register("IsSelected", typeof(bool), typeof(CategoryCard), new PropertyMetadata(false));

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            // Lấy parent của CategoryCard là MenuView
            MenuView menuView = FindVisualParent<MenuView>(this);

            if (menuView != null)
            {
                foreach (CategoryCard categoryCard in FindVisualChildren<CategoryCard>(menuView))
                {
                    if (categoryCard != this)
                    {
                        categoryCard.IsSelected = false;
                    }
                }
            }
        }

        private static T FindVisualParent<T>(DependencyObject obj) where T : DependencyObject
        {
            while (obj != null && !(obj is T))
            {
                obj = VisualTreeHelper.GetParent(obj);
            }
            return obj as T;
        }

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject obj) where T : DependencyObject
        {
            if (obj != null)
            {
                int childrenCount = VisualTreeHelper.GetChildrenCount(obj);
                for (int i = 0; i < childrenCount; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }


    }
}
