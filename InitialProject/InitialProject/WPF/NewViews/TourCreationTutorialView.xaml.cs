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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace InitialProject.WPF.NewViews
{
    /// <summary>
    /// Interaction logic for TourCreationTutorialView.xaml
    /// </summary>
    public partial class TourCreationTutorialView : UserControl
    {
        private DispatcherTimer progressTimer;
        private bool isVideoPlaying = false;

        public TourCreationTutorialView()
        {
            InitializeComponent();
            tutorialPlayer.Source = new Uri("C:/Users/lukaz/OneDrive/Desktop/Videos/tutorial.mp4");

            tutorialPlayer.MediaOpened += VideoPlayer_MediaOpened;

            // Set up timer for updating progress bar
            progressTimer = new DispatcherTimer();
            progressTimer.Interval = TimeSpan.FromSeconds(1); // Update progress every 1 second
            progressTimer.Tick += ProgressTimer_Tick;


        }
        private void ProgressTimer_Tick(object sender, EventArgs e)
        {
            double videoDuration = tutorialPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            double currentPosition = tutorialPlayer.Position.TotalSeconds;
            double progressPercentage = (currentPosition / videoDuration) * 100;

            progressBar.Value = progressPercentage;
        }
        
        private void VideoPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            // Pause the video when it's opened
            //tutorialPlayer.Pause(); // Pause the video initially
            progressTimer.Start(); // Start the progress timer
        }
        
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update the progress bar's value to reflect the current position of the video
            progressBar.Value = tutorialPlayer.Position.TotalSeconds;
        }

        private void ResumeClick(object sender, RoutedEventArgs e)
        {
            tutorialPlayer.Play();
            playButton.Visibility = Visibility.Hidden;
            pauseButton.Visibility = Visibility.Visible;
        }

        private void PauseClick(object sender, RoutedEventArgs e)
        {
            if (tutorialPlayer.CanPause)
            {
                tutorialPlayer.Pause();
                progressTimer.Stop();
            }
            playButton.Visibility = Visibility.Visible;
            pauseButton.Visibility = Visibility.Hidden;
        }
        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan newPosition = tutorialPlayer.Position + TimeSpan.FromSeconds(15);
            if (newPosition < tutorialPlayer.NaturalDuration)
            {
                tutorialPlayer.Position = newPosition;
            }
            else
            {
                tutorialPlayer.Position = tutorialPlayer.NaturalDuration.TimeSpan;
            }
        }

        private void RewindButton_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan newPosition = tutorialPlayer.Position.Subtract(TimeSpan.FromSeconds(15));
            tutorialPlayer.Position = newPosition;
        }
    }
}
