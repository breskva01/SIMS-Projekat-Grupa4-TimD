using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using System.Windows.Input;
using InitialProject.Application.Commands;
using iTextSharp.text.pdf;
using System.IO;
using System.Diagnostics;

namespace InitialProject.WPF.ViewModels.GuestOne
{
    public class AccommodationReservationViewModel : ViewModelBase
    {
        private readonly AccommodationReservationService _reservationService;
        private readonly NavigationStore _navigationStore;
        public Accommodation Accommodation { get; set; }
        public double AccommodationAverageRating { get; set; }
        public double OwnerAverageRating { get; set; }
        public string SecondImagePath => Accommodation.PictureURLs.Count > 1 ? Accommodation.PictureURLs[1] : null;
        public string ThirdImagePath => Accommodation.PictureURLs.Count > 2 ? Accommodation.PictureURLs[2] : null;
        public string FourthImagePath => Accommodation.PictureURLs.Count > 3 ? Accommodation.PictureURLs[3] : null;
        public string FifthImagePath => Accommodation.PictureURLs.Count > 4 ? Accommodation.PictureURLs[4] : null;

        public Guest1 Guest { get; set; }
        public int Days { get; set; }
        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime EndDate { get; set; }
        public ICommand NavigateImageBrowserCommand { get; }
        public ICommand FindAvailableReservationsCommand { get; }
        public ICommand NavigateAccommodationBrowserCommand { get; }
        public ICommand GeneratePDFCommand { get; }
        public AccommodationReservationViewModel(NavigationStore navigationStore ,Guest1 user, Accommodation accommodation)
        {
            StartDate = DateTime.Now;
            _navigationStore = navigationStore;
            Guest = user;
            Accommodation = accommodation;
            var ratingService = new AccommodationRatingService();
            AccommodationAverageRating = ratingService.CalculateAccommodationAverageRating(accommodation.Id);
            OwnerAverageRating = ratingService.CalculateOwnerAverageRating(accommodation.Owner.Id);
            _reservationService = new AccommodationReservationService();
            FindAvailableReservationsCommand = new ExecuteMethodCommand(GetAvailableReservations);
            NavigateAccommodationBrowserCommand = new ExecuteMethodCommand(NavigateAcoommodationBrowser);
            NavigateImageBrowserCommand = new ImageClickCommand(NavigateImageBrowser);
            GeneratePDFCommand = new ExecuteMethodCommand(PreparePDF);
        }

        private void GetAvailableReservations()
        {
            if (Days == 0)
            {
                if (TranslationSource.Instance.CurrentCulture.Name == "sr-Latn")
                    MessageBox.Show("Unesite željeni broj dana.");
                else
                    MessageBox.Show("Please input the desired number of days.");
            }
            else if (Days < Accommodation.MinimumDays)
            {
                if (TranslationSource.Instance.CurrentCulture.Name == "sr-Latn")
                    MessageBox.Show($"Minimalani broj dana: {Accommodation.MinimumDays}");
                else
                    MessageBox.Show($"Minimum number of days: {Accommodation.MinimumDays}");

            }
            else if (StartDate != DateTime.MinValue && EndDate != DateTime.MinValue)
            {
                DateOnly startDate = DateOnly.FromDateTime(StartDate);
                DateOnly endDate = DateOnly.FromDateTime(EndDate);
                List<AccommodationReservation> reservations = _reservationService.GetAvailable(startDate, endDate, Days, Accommodation, Guest);
                ShowDatePicker(reservations);
                
            }
            else if (TranslationSource.Instance.CurrentCulture.Name == "sr-Latn")
                MessageBox.Show("Izaberite željeni opseg datuma.");
            else
                MessageBox.Show("Please select the desired date range.");
        }
        private void PreparePDF()
        {
            string imagePath1 = "../../../Resources/Images/ReservationStep1.png";
            string imagePath2 = "../../../Resources/Images/ReservationStep2.png";
            string outputFilePath = "C:/Users/vukma/Documents/GitHub/SIMS-Projekat-Grupa4-TimD/InitialProject/InitialProject/Resources/PDF/output.pdf";

            GeneratePDF(imagePath1, imagePath2, outputFilePath);

            // Open the generated PDF file
            Process.Start("C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe", outputFilePath);
        }
        public void GeneratePDF(string imagePath1, string imagePath2, string outputFilePath)
        {
            using (iTextSharp.text.Document doc = new iTextSharp.text.Document())
            {
                // Calculate the dimensions of the images
                iTextSharp.text.Image img1 = iTextSharp.text.Image.GetInstance(imagePath1);
                iTextSharp.text.Image img2 = iTextSharp.text.Image.GetInstance(imagePath2);

                // Set the page size based on the larger image dimensions
                doc.SetPageSize(new iTextSharp.text.Rectangle(0, 0, Math.Max(img1.Width, img2.Width), Math.Max(img1.Height, img2.Height)));

                using (FileStream fs = new FileStream(outputFilePath, FileMode.Create))
                {
                    PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                    doc.Open();

                    // Add the first image to the PDF
                    img1.SetAbsolutePosition(0, 0);
                    doc.Add(img1);

                    // Add a new page
                    doc.NewPage();

                    // Add the second image to the PDF
                    img2.SetAbsolutePosition(0, 0);
                    doc.Add(img2);

                    doc.Close();
                }
            }
        }


        private void ShowDatePicker(List<AccommodationReservation> reservations)
        {
            var viewModel = new AccommodationReservationDatePickerViewModel(_navigationStore, reservations);
            var navigateCommand = new NavigateCommand
                (new NavigationService(_navigationStore, viewModel));

            navigateCommand.Execute(null);
        }
        private void NavigateAcoommodationBrowser()
        {
            var contentViewModel = new AccommodationBrowserViewModel(_navigationStore, Guest);
            var navigationBarViewModel = new NavigationBarViewModel(_navigationStore, Guest);
            var layoutViewModel = new LayoutViewModel(navigationBarViewModel, contentViewModel);
            var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, layoutViewModel));
            navigateCommand.Execute(null);
        }
        private void NavigateImageBrowser(string imageURL)
        {
            var viewModel = new ImageBrowserViewModel(Accommodation.PictureURLs, imageURL, RecreateSelf);
            new NavigationService(_navigationStore, viewModel).Navigate();
        }
        private void RecreateSelf()
        {
            var viewModel = new AccommodationReservationViewModel(_navigationStore, Guest, Accommodation);
            new NavigationService(_navigationStore, viewModel).Navigate();
        }
    }
}
