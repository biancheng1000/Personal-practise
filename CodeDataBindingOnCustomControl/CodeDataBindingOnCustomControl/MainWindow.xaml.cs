using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace CodeDataBindingOnCustomControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        TypeEnum _SelectMenu = TypeEnum.Two;

        public event PropertyChangedEventHandler PropertyChanged;

        public TypeEnum SelectMenu { get => _SelectMenu; set { _SelectMenu = value;OnPropertyChanged("SelectMenu"); } }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ICommand ChangeSelectMenu
        {
            get
            {
                return new DelegateCommand(() => SelectMenu = TypeEnum.Three);
            }
        }
    }

    public class DelegateCommand : ICommand
    {
        Action _act;
        public DelegateCommand(Action act)
        {
            _act = act;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_act != null)
            {
                _act();
            }
        }
    }

    public enum TypeEnum
    {
        One,
        Two,
        Three
    }
}
