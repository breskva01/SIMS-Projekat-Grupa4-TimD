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

namespace InitialProject.WPF.NewViews.Owner
{
    /// <summary>
    /// Interaction logic for MyRenovationAppointmentsView.xaml
    /// </summary>
    public partial class MyRenovationAppointmentsView : Window
    {
        public MyRenovationAppointmentsView(int id)
        {
            InitializeComponent();
            MyRenovationAppointmentsViewModel myRenovationAppointmentsViewModel = new MyRenovationAppointmentsViewModel(id);
            DataContext = myRenovationAppointmentsViewModel;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (DataContext is MyRenovationAppointmentsViewModel myRenovationAppointmentsVM)
            {
                HandleScheduleRenovationPanelKeydown(myRenovationAppointmentsVM);
            }
        }
        private void HandleScheduleRenovationPanelKeydown(MyRenovationAppointmentsViewModel viewModel)
        {
            if (Keyboard.IsKeyDown(Key.C) && Keyboard.IsKeyDown(Key.LeftCtrl))
                viewModel.CancelAppointmentCommand.Execute(null);
        }
    }
}
