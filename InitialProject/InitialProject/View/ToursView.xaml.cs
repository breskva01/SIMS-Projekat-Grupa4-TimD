using InitialProject.Controller;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for ToursView.xaml
    /// </summary>
    public partial class ToursView : Window, INotifyPropertyChanged
    {
        public User LoggedInUser { get; set; }
        public ObservableCollection<Tour> Tours { get; set; }
        public Tour SelectedTour { get; set; }
        private readonly TourController _controller;
        

        public ToursView(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            _controller = new TourController();
            Tours = new ObservableCollection<Tour>(_controller.GetAll());

            Height = SystemParameters.PrimaryScreenHeight * 0.75;
            Width = SystemParameters.PrimaryScreenWidth * 0.75;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void ApplyClick(object sender, RoutedEventArgs e)
        {
            string keyWords = SearchTextBox.Text;
            AccommodationType type = GetType();
            int guestNumber = GetGuestNumber();
            int numberOfDays = GetNumberOfDays();

            Accommodations.Clear();
            foreach (var accommodation in _controller.GetFiltered(keyWords, type, guestNumber, numberOfDays))
            {
                Accommodations.Add(accommodation);
            }
        }
    }
}
