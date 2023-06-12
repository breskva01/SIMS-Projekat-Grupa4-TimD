using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace InitialProject.WPF.ViewModels
{
    public class AllToursViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private User _user;

        public ObservableCollection<Location> _locations { get; set; }
        private readonly ObservableCollection<Tour> _tours;
        private readonly ObservableCollection<Tour> _toursShow;
        public IEnumerable<Tour> ToursShow => _toursShow;

        private List<User> _users;
        private List<int> _guestIds;
        private List<User> _guests;
        private List<TourReservation> _tourReservations;

        private LocationService _locationService;
        private TourService _tourService;
        private TourReservationService _tourReservationService;
        private UserService _userService;
        //private VoucherService _voucherService; 

        private DateTime _today;

        /*public ICommand CreateVoucherNavigateCommand =>
            new NavigateCommand(new NavigationService(_navigationStore, CreateVoucher()));*/
        public ICommand CancelCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand HomeCommand { get; set; }

        public ICommand CreateTourCommand { get; set; }
        public ICommand LiveTrackingCommand { get; set; }
        public ICommand CancelTourCommand { get; set; }
        public ICommand TourStatsCommand { get; set; }
        public ICommand RatingsViewCommand { get; set; }
        public ICommand TourRequestsCommand { get; set; }
        public ICommand TourRequestsStatsCommand { get; set; }
        public ICommand GuideProfileCommand { get; set; }
        public ICommand ComplexTourCommand { get; set; }
        public ICommand GeneratePDFCommand { get; set; }


        public ICommand SignOutCommand { get; set; }


        private Tour _selectedTour;
        public Tour SelectedTour
        {
            get { return _selectedTour; }
            set
            {
                _selectedTour = value;
                OnPropertyChanged(nameof(SelectedTour));

            }
        }

        public AllToursViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;

            //_voucherService = new VoucherService();
            _tourService = new TourService();
            _locationService = new LocationService();
            _userService = new UserService();
            _tourReservationService = new TourReservationService();

            _guestIds = new List<int>();
            _guests = new List<User>();
            _toursShow = new ObservableCollection<Tour>();

            _tourReservations = new List<TourReservation>(_tourReservationService.GetAll());
            _tours = new ObservableCollection<Tour>(_tourService.GetAll());
            _locations = new ObservableCollection<Location>(_locationService.GetAll());
            _users = new List<User>(_userService.GetAll());

            _today = DateTime.Now;

            foreach (Tour t in _tours)
            {
                foreach (Location l in _locations)
                {
                    if (t.LocationId == l.Id)
                        t.Location = l;
                }
            }
            foreach (Tour t in _tours)
            {
                if (t.State == 0)
                {
                    _toursShow.Add(t);
                }
            }

            InitializeCommands();
        }
        private void InitializeCommands()
        {
            CancelCommand = new ExecuteMethodCommand(CancelTour);
            HomeCommand = new ExecuteMethodCommand(ShowGuideMenuView);
            CreateTourCommand = new ExecuteMethodCommand(ShowTourCreationView);
            LiveTrackingCommand = new ExecuteMethodCommand(ShowToursTodayView);
            CancelTourCommand = new ExecuteMethodCommand(ShowTourCancellationView);
            TourStatsCommand = new ExecuteMethodCommand(ShowTourStatsView);
            RatingsViewCommand = new ExecuteMethodCommand(ShowGuideRatingsView);
            TourRequestsCommand = new ExecuteMethodCommand(ShowTourRequestsView);
            TourRequestsStatsCommand = new ExecuteMethodCommand(ShowTourRequestsStatsView);
            GuideProfileCommand = new ExecuteMethodCommand(ShowGuideProfileView);
            ComplexTourCommand = new ExecuteMethodCommand(ShowComplexTourView);
            GeneratePDFCommand = new ExecuteMethodCommand(GeneratePDF);
        }
        private static string OpenFilePicker()
        {
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            saveFileDialog.DefaultExt = "pdf";
            if (saveFileDialog.ShowDialog() == true)
                return saveFileDialog.FileName;
            throw new Exception("Save file dialog returned error!");
        }
        private void GeneratePDF()
        {
            string filePath = OpenFilePicker();

            // Create a new PDF document
            iTextSharp.text.Document document = new iTextSharp.text.Document();

            // Create a new PdfWriter to write the document to a file
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream("output.pdf", FileMode.Create));

            writer.SetPdfVersion(PdfWriter.PDF_VERSION_1_7);
            writer.SetFullCompression();

            document.Open();

            iTextSharp.text.Paragraph heading = new(
                $"Lista gostiju za turu ({SelectedTour.Id}) {SelectedTour.Name}:",
                    new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD))
            {
                SpacingAfter = 15f
            };

            List<User> guests = new List<User>();
            User g1 = new User();
            g1 = _userService.GetById(3);
            User g2 = new User();
            g2 = _userService.GetById(4);

            guests.Add(g1);
            guests.Add(g2);

            PdfPTable table = new(6);


            table.AddCell("Godina");
            table.AddCell("Broj rezervacija");
            table.AddCell("Otkazane rezervacije");
            table.AddCell("Pomerene rezervacije");
            table.AddCell("Preporuke za renoviranje");
            table.AddCell("Najveća zauzetost");

            document.Add(heading);
            document.Add(table);
            document.Close();

            MessageBox.Show("PDF file generated successfully.");
        
            
    /*
    PdfPTable table = statistics.First().Type == Domain.Models.AccommodationStatisticType.Monthly ? new(7) : new(6);
    table.AddCell("Godina");
    if (statistics.First().Type == Domain.Models.AccommodationStatisticType.Monthly) table.AddCell("Mesec");
    table.AddCell("Broj rezervacija");
    table.AddCell("Otkazane rezervacije");
    table.AddCell("Pomerene rezervacije");
    table.AddCell("Preporuke za renoviranje");
    table.AddCell("Najveća zauzetost");
    */
    /*
    try
    {
        string filePath = OpenFilePicker();

        Document document = new();
        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
        writer.SetPdfVersion(PdfWriter.PDF_VERSION_1_7);
        writer.SetFullCompression();

        document.Open();

        Paragraph heading = new(
        $"Statistika za smeštaj ({statistics.First().Accommodation.Id}) {statistics.First().Accommodation.Name}:",
            new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD))
        {
            SpacingAfter = 15f
        };
        document.Add(heading);

        PdfPTable table = statistics.First().Type == Domain.Models.AccommodationStatisticType.Monthly ? new(7) : new(6);
        table.AddCell("Godina");
        if (statistics.First().Type == Domain.Models.AccommodationStatisticType.Monthly) table.AddCell("Mesec");
        table.AddCell("Broj rezervacija");
        table.AddCell("Otkazane rezervacije");
        table.AddCell("Pomerene rezervacije");
        table.AddCell("Preporuke za renoviranje");
        table.AddCell("Najveća zauzetost");

        foreach (var statistic in statistics)
        {
            table.AddCell(statistic.Year.ToString());
            if (statistics.First().Type == Domain.Models.AccommodationStatisticType.Monthly) table.AddCell(statistic.ShortMonth);
            table.AddCell(statistic.TotalReservations.ToString());
            table.AddCell(statistic.CancelledReservations.ToString());
            table.AddCell(statistic.RescheduledReservations.ToString());
            table.AddCell(statistic.RenovationRecommendations.ToString());
            table.AddCell(statistic.Best ? "DA." : string.Empty);
        }

        document.Add(table);
        document.Close();

        MessageBox.Show("PDF file generated successfully.");
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error generating PDF file: " + ex.Message);
    }*/
}
        
        private void CancelTour()
        {
            if (SelectedTour == null)
            {
                MessageBox.Show("Please select a tour first.");

                return;
            }
            if (SelectedTour.Start > _today.AddHours(48))
            {
                if (SelectedTour.State == TourState.None)
                {
                    SelectedTour.State = TourState.Canceled;

                    foreach (TourReservation tourReservation in _tourReservations)
                    {
                        if (tourReservation.TourId == SelectedTour.Id)
                        {
                            _guestIds.Add(tourReservation.GuestId);
                        }
                    }

                    foreach (int id in _guestIds)
                    {
                        foreach (User u in _users)
                        {
                            if (id == u.Id && !_guests.Contains(u))
                            {
                                _guests.Add(u);
                            }

                        }
                    }

                    //CreateVoucherNavigateCommand.Execute(null);
                    _tourService.Update(SelectedTour);
                    _toursShow.Remove(SelectedTour);
                    return;
                }
            }
            MessageBox.Show("It's too late to cancel this tour.");
            return;
        }
        /*
        private VoucherCreationViewModel CreateVoucher()
        {
            return new VoucherCreationViewModel(_navigationStore, _user, _guests, 1, false);

        }
        */
        private void ShowGuideMenuView()
        {
            GuideMenuViewModel viewModel = new GuideMenuViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }
        private void ShowTourCreationView()
        {
            TourCreationViewModel viewModel = new TourCreationViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }

        private void ShowComplexTourView()
        {
            ComplexTourAcceptViewModel viewModel = new ComplexTourAcceptViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }
        private void ShowToursTodayView()
        {
            ToursTodayViewModel viewModel = new ToursTodayViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }
        private void ShowTourCancellationView()
        {
            AllToursViewModel viewModel = new AllToursViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }
        private void ShowTourStatsView()
        {
            TourStatsViewModel viewModel = new TourStatsViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }
        private void ShowGuideRatingsView()
        {
            GuideRatingsViewModel viewModel = new GuideRatingsViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }
        private void ShowTourRequestsView()
        {
            TourRequestsAcceptViewModel viewModel = new TourRequestsAcceptViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }
        private void ShowTourRequestsStatsView()
        {
            TourRequestsStatsViewModel viewModel = new TourRequestsStatsViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }
        private void ShowGuideProfileView()
        {
            GuideProfileViewModel viewModel = new GuideProfileViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }


    }
}
