using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Management.Models
{
    public class TempMenuItems : Utilities.ViewModelBase
    {
        private MenuItems _menuItem;
        public MenuItems MenuItem
        {
            get { return _menuItem; }
            set
            {
                _menuItem = value;
                OnPropertyChanged(nameof(MenuItem));
            }
        }

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }
    }
}
