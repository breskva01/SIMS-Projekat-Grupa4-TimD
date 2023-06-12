using InitialProject.Domain.Models;
using OxyPlot.Series;
using OxyPlot;
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
using InitialProject.WPF.ViewModels;

namespace InitialProject.WPF.NewViews.Owner
{
    /// <summary>
    /// Interaction logic for AccommodationStatisticsView.xaml
    /// </summary>
    public partial class AccommodationStatisticsView : Window
    {
        public AccommodationStatisticsView(Accommodation selectedAccommodation, User owner)
        {
            InitializeComponent();
            AccommodationStatisticsViewModel accommodationStatisticsViewModel= new AccommodationStatisticsViewModel(selectedAccommodation, owner);
            populateComboBox();
            DataContext = accommodationStatisticsViewModel;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void populateComboBox()
        {
            DateOnly start = new DateOnly(2020, 1, 1);
            for (int i = start.Year; i <= DateTime.Now.Year; i++)
            {
                yearsComboBox.Items.Add(i.ToString());
            }
        }
    }
}
