﻿using System;
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
    /// Interaction logic for Food_Card.xaml
    /// </summary>
    public partial class FoodCard : UserControl
    {
        public FoodCard()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty FoodImageProperty =
            DependencyProperty.Register("FoodImage", typeof(ImageSource), typeof(FoodCard));

        public ImageSource FoodImage
        {
            get { return (ImageSource)GetValue(FoodImageProperty); }
            set { SetValue(FoodImageProperty, value); }
        }

        public static readonly DependencyProperty FoodNameProperty =
            DependencyProperty.Register("FoodName", typeof(string), typeof(FoodCard));

        public string FoodName
        {
            get { return (string)GetValue(FoodNameProperty); }
            set { SetValue(FoodNameProperty, value); }
        }

        public static readonly DependencyProperty FoodPriceProperty =
            DependencyProperty.Register("FoodPrice", typeof(string), typeof(FoodCard));

        public string FoodPrice
        {
            get { return (string)GetValue(FoodPriceProperty); }
            set { SetValue(FoodPriceProperty, value); }
        }

    }
}
