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
    /// Interaction logic for InvoicesView.xaml
    /// </summary>
    public partial class InvoicesView : UserControl
    {
        public InvoicesView()
        {
            InitializeComponent();
            var converter = new BrushConverter();

            ObservableCollection<TestInvoice> tests = new ObservableCollection<TestInvoice>();

            tests.Add(new TestInvoice { Number = "1", ID = "IV01", StaffID = "EMP001", CustomerID = "CUS001", CreatedDate = "30-02-2024", Status = "Paid", Amount = "100", BgColor = (Brush)converter.ConvertFromString("#f9f9f9")});
            invoicesDataGrid.ItemsSource = tests;
        }

        public class TestInvoice
        {
            public string Number { get; set; }
            public string ID { get; set; }
            public string StaffID { get; set; }
            public string CustomerID { get; set; }
            public string CreatedDate { get; set; }
            public string Status { get; set; }
            public string Amount { get; set; }
            public Brush BgColor { get; set; }

        }
    }
}
