using MaterialDesignThemes.Wpf;
using MongoDB.Driver;
using Restaurant_Management.Models;
using Restaurant_Management.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class GreetingDataProvider
    {
        private static readonly string[] GreetingMessages =
        {
            "Greetings and salutations!",
            "A warm welcome to you!",
            "May your day be filled with joy and positivity!",
            "Delighted to have you join us.",
            "Welcome, and may your journey here be delightful!",
            "Wishing you a splendid experience in our app.",
            "Good day! Your presence is a pleasure.",
            "Step into a world of possibilities. Welcome!",
            "Greetings! Here's to a day filled with success and happiness.",
            "A hearty welcome to our valued guest!"
        };

        public string GetRandomGreeting()
        {
            Random random = new Random();
            int index = random.Next(GreetingMessages.Length);
            return GreetingMessages[index];
        }
    }
    public class AnimatedGradientOffsetConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double width = (double)values[0];
            double height = (double)values[1];

            // You can customize the logic to create an animated effect
            double offset = Math.Sqrt(width * width + height * height) / 2;

            return offset;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
