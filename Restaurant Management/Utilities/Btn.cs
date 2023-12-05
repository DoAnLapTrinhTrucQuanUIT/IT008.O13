using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace Restaurant_Management.Utilities
{
    public class Btn : RadioButton
    {
        static Btn()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Btn), new System.Windows.FrameworkPropertyMetadata(typeof(Btn)));  
        }
    }
}
