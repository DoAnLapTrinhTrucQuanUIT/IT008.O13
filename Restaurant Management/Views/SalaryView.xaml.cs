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
    /// Interaction logic for SalaryView.xaml
    /// </summary>
    public partial class SalaryView : UserControl
    {
        public SalaryView()
        {
            InitializeComponent();

            var converter = new BrushConverter();

            ObservableCollection<TestSalary> tests = new ObservableCollection<TestSalary>();

            tests.Add(new TestSalary { Number = "1", ID = "NV01", Name = "Tai", BgColor = (Brush)converter.ConvertFromString("#f9f9f9"), Email="example@gmail.com", Salary="10000000" });
            salaryDataGrid.ItemsSource = tests;
        }
        public class TestSalary
        {
            public string ID { get; set; }
            public string Number { get; set; }
            public string Email { get; set; }
            public string Name { get; set; }
            public string Salary { get; set; }
            public Brush BgColor { get; set; }

        }
    }
}
