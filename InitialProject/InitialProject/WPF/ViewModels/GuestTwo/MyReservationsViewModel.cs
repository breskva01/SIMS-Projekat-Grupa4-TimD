using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace InitialProject.WPF.ViewModels.GuestTwo
{
    public class MyReservationsViewModel : ViewModelBase
    {
        public ObservableCollection<TourReservation> MyReservations { get; set; }
        public List<TourReservation> MyReservationsList { get; set; }
        public ObservableCollection<Location> Locations { get; set; }
        public Tour SelectedTourReservation { get; set; }

        private User _user;
        private readonly NavigationStore _navigationStore;
        private readonly TourService _tourService;
        private readonly TourReservationService _tourReservationService;
        private readonly LocationService _locationService;

        public ICommand PDFCommand { get; }

        public ICommand BackCommand { get; }
        public ICommand MenuCommand { get; }
        public ICommand NotificationCommand { get; }


        public MyReservationsViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;
            _tourService = new TourService();
            _tourReservationService = new TourReservationService();
            _locationService = new LocationService();

            MyReservations= new ObservableCollection<TourReservation>();
            MyReservationsList = new List<TourReservation>();
            Locations = new ObservableCollection<Location>(_locationService.GetAll());
            foreach (var t in _tourReservationService.GetByUserId(_user.Id))
            {
                t.Tour.Location = Locations.FirstOrDefault(l => l.Id == t.Tour.LocationId);
                MyReservationsList.Add(t);
                MyReservations.Add(t);
            }

            PDFCommand = new ExecuteMethodCommand(GenerateReservationPDF);
            BackCommand = new ExecuteMethodCommand(ShowGuest2MenuView);
            MenuCommand = new ExecuteMethodCommand(ShowGuest2MenuView);
            NotificationCommand = new ExecuteMethodCommand(ShowNotificationsView);
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

        private void GenerateReservationPDF()
        {
            try
            {
                string filePath = OpenFilePicker();
                iTextSharp.text.Document document = new();
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                writer.SetPdfVersion(PdfWriter.PDF_VERSION_1_7);
                writer.SetFullCompression();

                document.Open();

                iTextSharp.text.Paragraph heading = new(
                $"All reservations from user {_user.FirstName}",
                    new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 16, iTextSharp.text.Font.BOLD))
                {
                    SpacingAfter = 15f
                };
                document.Add((iTextSharp.text.IElement)heading);

                PdfPTable table = new(6);
                table.AddCell("Name");
                table.AddCell("Country");
                table.AddCell("City");
                table.AddCell("Spots reserved");
                table.AddCell("Duration in hours");
                table.AddCell("Date");

                foreach (var reservation in MyReservationsList)
                {
                    table.AddCell(reservation.Tour.Name);
                    table.AddCell(reservation.Tour.Location.Country);
                    table.AddCell(reservation.Tour.Location.City);
                    table.AddCell(reservation.NumberOfGuests.ToString());
                    table.AddCell(reservation.Tour.Duration.ToString());
                    table.AddCell(reservation.Tour.Start.ToString("dd-MM-yyyy"));
                }
                document.Add(table);
                document.Close();

                //MessageBox.Show("PDF file generated successfully.");
                Process.Start("C:/Program Files (x86)/Microsoft/Edge/Application/msedge.exe", filePath);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error generating PDF file: " + ex.Message);
            }
        }

        private void ShowGuest2MenuView()
        {
            Guest2MenuViewModel guest2MenuViewModel = new Guest2MenuViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, guest2MenuViewModel));
            navigate.Execute(null);
        }


        private void ShowNotificationsView()
        {
            NotificationBrowserViewModel notificationBrowserViewModel = new NotificationBrowserViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, notificationBrowserViewModel));

            navigate.Execute(null);
        }
    }
}
