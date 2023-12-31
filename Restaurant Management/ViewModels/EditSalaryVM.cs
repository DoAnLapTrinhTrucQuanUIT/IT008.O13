using MongoDB.Driver;
using Restaurant_Management.Models;
using Restaurant_Management.Utilities;
using Restaurant_Management.Views.Component;
using System;
using System.Windows.Input;

namespace Restaurant_Management.ViewModels
{
    public class EditSalaryVM : Utilities.ViewModelBase
    {
        public ICommand CloseWDCM { get; set; }
        public ICommand MinimizeWDCM { get; set; }
        public ICommand MoveWDCM { get; set; }
        public ICommand ConfirmCM { get; set; }

        public event EventHandler<decimal> BasicSalaryUpdated;

        private decimal _editedBasicSalary;

        public decimal EditedBasicSalary
        {
            get { return _editedBasicSalary; }
            set
            {
                if (_editedBasicSalary != value)
                {
                    _editedBasicSalary = value;
                    OnPropertyChanged(nameof(EditedBasicSalary));

                    BasicSalaryUpdated?.Invoke(this, _editedBasicSalary);
                }
            }
        }

        private SalaryInformation _selectedSalary;

        public SalaryInformation SelectedSalary
        {
            get { return _selectedSalary; }
            set
            {
                if (_selectedSalary != value)
                {
                    _selectedSalary = value;
                    OnPropertyChanged(nameof(SelectedSalary));
                }
            }
        }

        public EditSalaryVM()
        {
            CloseWDCM = new RelayCommand<EditSalary>((p) => true, (p) => _CloseWD(p));
            MinimizeWDCM = new RelayCommand<EditSalary>((p) => true, (p) => _MinimizeWD(p));
            MoveWDCM = new RelayCommand<EditSalary>((p) => true, (p) => _MoveWD(p));
            ConfirmCM = new RelayCommand<EditSalary>((p) => true, (p) => ConfirmCommand(p));
        }

        private void ConfirmCommand(EditSalary parameter)
        {
            if (SelectedSalary != null)
            {
                // Gán giá trị mới cho BasicSalary của SelectedSalary
                SelectedSalary.BasicSalary = EditedBasicSalary;

                // Gọi sự kiện để thông báo việc cập nhật lương
                BasicSalaryUpdated?.Invoke(this, SelectedSalary.BasicSalary);

            }
            else
            {
                return;
            }
            var window = System.Windows.Window.GetWindow(parameter as EditSalary);
            if (window != null)
            {
                window.Close();
            }
        }

        private void _CloseWD(EditSalary paramater)
        {
            var window = System.Windows.Window.GetWindow(paramater);
            if (window != null)
            {
                window.Close();
            }
        }

        private void _MinimizeWD(EditSalary paramater)
        {
            var window = System.Windows.Window.GetWindow(paramater);
            if (window != null)
            {
                System.Windows.WindowState originalWindowState = window.WindowState;
                window.WindowState = System.Windows.WindowState.Minimized;
            }
        }

        private void _MoveWD(EditSalary paramater)
        {
            var window = System.Windows.Window.GetWindow(paramater);
            if (window != null)
            {
                window.DragMove();
            }
        }
    }
}
