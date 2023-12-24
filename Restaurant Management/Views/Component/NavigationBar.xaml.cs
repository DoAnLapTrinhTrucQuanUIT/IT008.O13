using MaterialDesignThemes.Wpf;
using MongoDB.Driver;
using Restaurant_Management.Models;
using Restaurant_Management.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Restaurant_Management.Views.Component
{
    public partial class NavigationBar : UserControl
    {
        public NavigationBar()
        {
            InitializeComponent();
        }
       
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            // Minimize the main window
            if (Window.GetWindow(this) is Window mainWindow)
            {
                mainWindow.WindowState = WindowState.Minimized;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the main window
            if (Window.GetWindow(this) is Window mainWindow)
            {
                mainWindow.Close();
            }
        }
    }
}
