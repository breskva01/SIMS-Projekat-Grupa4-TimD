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
using System.Windows.Shapes;

namespace InitialProject.WPF.NewViews
{
    /// <summary>
    /// Interaction logic for RequestDeniedView.xaml
    /// </summary>
    public partial class RequestDeniedView : Window
    {
        public RequestDeniedView(AccommodationReservationMoveRequest selectedRequest)
        {
            InitializeComponent();
            RequestDeniedViewModel requestDeniedViewModel = new RequestDeniedViewModel(selectedRequest);
            DataContext = requestDeniedViewModel;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
