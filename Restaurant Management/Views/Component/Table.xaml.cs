using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Table.xaml
    /// </summary>
    public partial class Table : UserControl, INotifyPropertyChanged
    {
        public Table()
        {
            InitializeComponent();
            DataContext = this;
        }
        public static readonly DependencyProperty TextProperty = 
            DependencyProperty.Register("Text", typeof(string), typeof(Table), new PropertyMetadata(""));
        public string Text
        {
            get { return (string)GetValue(TextProperty);}
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(Table), new PropertyMetadata(false));

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(bool), typeof(Table), new PropertyMetadata(false));

        public bool Status
        {
            get { return (bool)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            TableView tableView = FindVisualParent<TableView>(this);

            if (tableView != null)
            {
                foreach (Table table in FindVisualChildren<Table>(tableView))
                {
                    if (table != this)
                    {
                        table.IsSelected = false;
                    }
                }
            }
        }

        public static readonly DependencyProperty TableStatusProperty = DependencyProperty.Register("TableStatus", typeof(bool), typeof(Table), new PropertyMetadata(false));

        private bool _tableStatus;

        public bool TableStatus
        {
            get { return (bool)GetValue(TableStatusProperty); }
            set
            {
                SetValue(TableStatusProperty, value);   
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
