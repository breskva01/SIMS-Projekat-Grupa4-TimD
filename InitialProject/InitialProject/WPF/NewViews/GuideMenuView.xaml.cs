using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Vlc.DotNet.Wpf;


namespace InitialProject.WPF.NewViews
{
    /// <summary>
    /// Interaction logic for GuideMenuView.xaml
    /// </summary>
    public partial class GuideMenuView : UserControl
    {
        public GuideMenuView()
        {
            InitializeComponent();
            
        }
        
        private void StartTextAnimation()
        {
            // Create the animation for increasing and decreasing the font size
            var animation = new DoubleAnimation
            {
                From = 60, // Initial font size
                To = 63, // Maximum font size
                AutoReverse = true, // Reverse the animation to shrink the text back
                Duration = TimeSpan.FromSeconds(3), // Total duration of the animation
                RepeatBehavior = RepeatBehavior.Forever // Repeat the animation indefinitely
            };

            // Set the target property of the animation
            Storyboard.SetTarget(animation, label);
            Storyboard.SetTargetProperty(animation, new PropertyPath(System.Windows.Controls.Label.FontSizeProperty));

            // Create a storyboard to contain the animation
            var storyboard = new Storyboard();
            storyboard.Children.Add(animation);

            // Start the storyboard animation
            storyboard.Begin();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            StartTextAnimation();
        }
    }
}
