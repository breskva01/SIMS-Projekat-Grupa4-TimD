using InitialProject.Domain.Models;
using InitialProject.WPF.ViewModels;
using InitialProject.WPF.ViewModels.GuestOne;
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
    /// Interaction logic for ScheduleRenovationView.xaml
    /// </summary>
    public partial class ScheduleRenovationView : Window
    {
        public ScheduleRenovationView(Accommodation selectedAccommodation)
        {
            InitializeComponent();
            ScheduleRenovationViewModel scheduleRenovationViewModel = new ScheduleRenovationViewModel(selectedAccommodation);
            DataContext = scheduleRenovationViewModel;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (DataContext is ScheduleRenovationViewModel scheduleRenovationVM)
            {
                    HandleScheduleRenovationPanelKeydown(scheduleRenovationVM);
            }
        }
        private void HandleScheduleRenovationPanelKeydown(ScheduleRenovationViewModel viewModel)
        {
            if (Keyboard.IsKeyDown(Key.S) && Keyboard.IsKeyDown(Key.LeftCtrl))
                viewModel.SearchCommand.Execute(null);
            else if (Keyboard.IsKeyDown(Key.R) && Keyboard.IsKeyDown(Key.LeftCtrl))
                viewModel.ScheduleRenovationCommand.Execute(null);
        }
    }
}
