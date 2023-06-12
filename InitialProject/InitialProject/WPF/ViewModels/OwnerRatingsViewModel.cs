using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.WPF.NewViews;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Windows;
using OxyPlot.Axes;
using System.Diagnostics;

namespace InitialProject.WPF.ViewModels
{
    public class OwnerRatingsViewModel : ViewModelBase
    {
        public ObservableCollection<AccommodationRating> OwnerRatings { get; set; }
        private AccommodationRatingService _accommodatonRatingService;
        private readonly User _user;
        private readonly NavigationStore _navigationStore;
        public OwnerRatingsViewModel(NavigationStore navigationStore, User user)
        {
            _user = user;
            _navigationStore = navigationStore;
            _accommodatonRatingService = new AccommodationRatingService();
            OwnerRatings = new ObservableCollection<AccommodationRating>(_accommodatonRatingService.GetEligibleForDisplay(_user.Id));
            GeneratePDFCommand = new ExecuteMethodCommand(GeneratePDF);
        }
        public ICommand GeneratePDFCommand { get; }
        private static string OpenFilePicker()
        {
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            saveFileDialog.DefaultExt = "pdf";
            if (saveFileDialog.ShowDialog() == true)
                return saveFileDialog.FileName;
            throw new Exception("Save file dialog returned error!");
        }

        public static void GenerateAccommodationStatsPDF(ObservableCollection<AccommodationRating> ratings)
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
                    $"Ocene za moje smeštaje:",
                    new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD))
                {
                    SpacingAfter = 15f
                };
                document.Add(heading);

                PdfPTable table = new(6);
                table.AddCell("Name");
                table.AddCell("Location");
                table.AddCell("Hygiene");
                table.AddCell("Pleasant");
                table.AddCell("Fairness");
                table.AddCell("Parking");
                List<AverageAccommodationRatings> averageRatings = new List<AverageAccommodationRatings>();
                foreach (var rating1 in ratings.ToList())
                {
                    if (ratings.Count == 0)
                    {
                        break;
                    }
                    else
                    {
                        int count = 1;
                        foreach (var rating2 in ratings.ToList())
                        {
                            if (rating1.Reservation.Id != rating2.Reservation.Id && rating1.Reservation.Accommodation.Id == rating2.Reservation.Accommodation.Id)
                            {
                                rating1.Location += rating2.Location;
                                rating1.Hygiene += rating2.Hygiene;
                                rating1.Pleasantness += rating2.Pleasantness;
                                rating1.Fairness += rating2.Fairness;
                                rating1.Parking += rating2.Parking;
                                ratings.Remove(rating2);
                                count++;
                            }
                        }
                        if (count != 1)
                        {
                            double location = (Double)rating1.Location / count;
                            double hygiene = (Double)rating1.Hygiene / count;
                            double pleasantness = (Double)rating1.Pleasantness / count;
                            double fairness = (Double)rating1.Fairness / count;
                            double parking = (Double)rating1.Parking / count;
                            AverageAccommodationRatings averageAccommodationRatings = new AverageAccommodationRatings(rating1.Reservation,
                                Math.Round(location, 2), Math.Round(hygiene, 2), Math.Round(pleasantness, 2), Math.Round(fairness, 2), Math.Round(parking, 2));
                            averageRatings.Add(averageAccommodationRatings);
                            ratings.Remove(rating1);
                        }
                        else
                        {
                            double location = (Double)rating1.Location;
                            double hygiene = (Double)rating1.Hygiene;
                            double pleasantness = (Double)rating1.Pleasantness;
                            double fairness = (Double)rating1.Fairness;
                            double parking = (Double)rating1.Parking;
                            AverageAccommodationRatings averageAccommodationRatings = new AverageAccommodationRatings(rating1.Reservation,
                                Math.Round(location, 2), Math.Round(hygiene, 2), Math.Round(pleasantness, 2), Math.Round(fairness, 2), Math.Round(parking, 2));
                            averageRatings.Add(averageAccommodationRatings);
                            ratings.Remove(rating1);
                        }
                    }
                    
                }
                foreach(AverageAccommodationRatings averageAccommodationRating in averageRatings)
                {
                    table.AddCell(averageAccommodationRating.Reservation.Accommodation.Name);
                    table.AddCell(averageAccommodationRating.Location.ToString());
                    table.AddCell(averageAccommodationRating.Hygiene.ToString());
                    table.AddCell(averageAccommodationRating.Pleasantness.ToString());
                    table.AddCell(averageAccommodationRating.Fairness.ToString());
                    table.AddCell(averageAccommodationRating.Parking.ToString());
                }

                document.Add(table);
                document.Close();
                Process.Start("C:/Program Files (x86)/Microsoft/Edge/Application/msedge.exe", filePath);
                MessageBox.Show("PDF file generated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating PDF file: " + ex.Message);
            }
        }
        private void GeneratePDF()
        {
            GenerateAccommodationStatsPDF(OwnerRatings);
            OwnerRatings.Clear();
            foreach (var rating in _accommodatonRatingService.GetEligibleForDisplay(_user.Id))
            {
                OwnerRatings.Add(rating);
            }
        }
    }

}
