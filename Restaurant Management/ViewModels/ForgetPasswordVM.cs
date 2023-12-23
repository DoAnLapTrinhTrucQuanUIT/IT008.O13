using Restaurant_Management.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Restaurant_Management.ViewModels.ComponentVM
{
    public class ForgetPasswordVM : ViewModelBase
    {
        private string _email;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public ICommand SendPassCommand { get; set; }

        public ForgetPasswordVM()
        {
            SendPassCommand = new RelayCommand<object>((p) => true, (p) => SendPassword());
        }

        private void SendPassword()
        {
            System.Windows.MessageBox.Show($"Send password for email: {Email}");
        }
    }
}