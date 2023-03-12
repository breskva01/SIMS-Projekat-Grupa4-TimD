using InitialProject.Controller;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Xml.Linq;
using Xceed.Wpf.Toolkit;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for AccommodationBrowser.xaml
    /// </summary>
    public partial class AccommodationBrowser : Window, INotifyPropertyChanged
    {
        public User LoggedInUser { get; set; }
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        private readonly AccommodationController _controller;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public AccommodationBrowser(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            _controller = new AccommodationController();
            Accommodations = new ObservableCollection<Accommodation>(_controller.GetAll());

            Height = SystemParameters.PrimaryScreenHeight * 0.75;
            Width = SystemParameters.PrimaryScreenWidth * 0.75;
        }

        private void ApplyFiltersClick(object sender, RoutedEventArgs e)
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
        private AccommodationType GetType()
        {
            switch (TypeComboBox.SelectedIndex)
            {
                case 0:
                    return AccommodationType.Everything;
                case 1:
                    return AccommodationType.House;
                case 2:
                    return AccommodationType.Apartment;
                default:
                    return AccommodationType.Cottage;
            }
        }
        private int GetGuestNumber()
        {
            int guestNumber = 0;
            try
            {
                guestNumber = int.Parse(GuestNumberTextBox.Text);
            } catch { };
            return guestNumber;
        }
        private int GetNumberOfDays()
        {
            int numberOfDays = 0;
            try
            {
                numberOfDays = int.Parse(NumberOfDaysTextBox.Text);
            } catch { };
            return numberOfDays;
        }

        private void ResetFiltersClick(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Clear();
            GuestNumberTextBox.Clear();
            NumberOfDaysTextBox.Clear();
            TypeComboBox.SelectedIndex = 0;
            Accommodations.Clear();
            foreach (var accommodation in _controller.GetAll())
            {
                Accommodations.Add(accommodation);
            }
        }
    }
}
