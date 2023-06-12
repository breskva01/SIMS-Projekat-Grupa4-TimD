using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Threading;
using System.ComponentModel;
using InitialProject.WPF.ViewModels;
using System.Windows.Media.Effects;

namespace InitialProject.WPF.NewViews
{
    /// <summary>
    /// Interaction logic for GuideProfileView.xaml
    /// </summary>
    public partial class GuideProfileView : UserControl
    {
        private DispatcherTimer timer;
        private bool isColorChanged;


        public GuideProfileView()
        {
            InitializeComponent();
            
        }
        private void StartColorAnimation()
        {
            
            string startColorHex = "#FFFFFF"; // White color in hexadecimal
            string endColorHex = "#FFFB00"; // Light purple color in hexadecimal

            // Convert the start and end color hex values to Color objects
            Color startColor = (Color)ColorConverter.ConvertFromString(startColorHex);
            Color endColor = (Color)ColorConverter.ConvertFromString(endColorHex);
            var colorAnimation = new ColorAnimation
            {
                From = Colors.White, // Starting color (white)
                To = endColor,// Ending color (light purple)
                Duration = TimeSpan.FromSeconds(5), // Duration of the animation
                AutoReverse = true, // Reverse the animation back to the starting color
                RepeatBehavior = RepeatBehavior.Forever // Repeat the animation indefinitely
            };

            // Set the target property of the animation
            Storyboard.SetTarget(colorAnimation, label);
            Storyboard.SetTargetProperty(colorAnimation, new PropertyPath("Foreground.Color"));

            // Create a storyboard to contain the animation
            var storyboard = new Storyboard();
            storyboard.Children.Add(colorAnimation);

            // Start the storyboard animation
            storyboard.Begin();
            /*
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // Adjust the interval as desired
            timer.Tick += Timer_Tick;
            timer.Start();
            */
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Toggle the color between two states
            if (isColorChanged)
            {
                label.Foreground = Brushes.Black; // Change to the desired color
            }
            else
            {
                label.Foreground = Brushes.Red; // Change to the desired color
            }

            isColorChanged = !isColorChanged;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            StartColorAnimation();
            ButtonGlow();
        }
        private void ButtonGlow()
        {
            DropShadowEffect glowEffect = new DropShadowEffect()
            {
                Color = Colors.Yellow,
                ShadowDepth = 0,
                BlurRadius = 40,
                Opacity = 1
            };

            // Apply the glowEffect to the button
            resignButton.Effect = glowEffect;
            logOutButton.Effect = glowEffect;
        }
    }
}
