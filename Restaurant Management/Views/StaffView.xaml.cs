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

namespace Restaurant_Management.Views
{
    /// <summary>
    /// Interaction logic for StaffView.xaml
    /// </summary>
    public partial class StaffView : UserControl
    {
        public StaffView()
        {
            InitializeComponent();

            var converter = new BrushConverter();

            ObservableCollection<TestStaff> tests = new ObservableCollection<TestStaff>();

            tests.Add(new TestStaff { Number = "1", ID = "NV01", Name = "Tai", BgColor = (Brush)converter.ConvertFromString("#f9f9f9"), Department = "owner", PhoneNumber = "0328329908", Email = "example@gmail.com", HireDate="1-1-2023" });
            staffDataGrid.ItemsSource = tests;
        }
        public class TestStaff
        {
            public string ID { get; set; }
            public string Number { get; set; }
            public string Name { get; set; }
            public string Department { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }
            public string HireDate { get; set; }
            public Brush BgColor { get; set; }

        }
    }
}
