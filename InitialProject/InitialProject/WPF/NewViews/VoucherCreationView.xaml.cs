using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.WPF.ViewModels;
using System;
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

namespace InitialProject.WPF.NewViews
{
    /// <summary>
    /// Interaction logic for VoucherCreationView.xaml
    /// </summary>
    public partial class VoucherCreationView : Window
    {
        public VoucherCreationView(NavigationStore navigationStore, User user, List<User> guests, int years, bool resign)
        {
            InitializeComponent();
            VoucherCreationViewModel viewModel = new VoucherCreationViewModel(navigationStore, user, guests, years, this, resign);
            DataContext = viewModel;
        }
    }
}
