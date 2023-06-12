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

namespace InitialProject.WPF.NewViews.GuestOne
{
    /// <summary>
    /// Interaction logic for ForumCommentsView.xaml
    /// </summary>
    public partial class ForumCommentsView : UserControl
    {
        public ForumCommentsView()
        {
            InitializeComponent();
        }
        private void MyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlaceholderText.Visibility = string.IsNullOrEmpty(MyTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void MyTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PlaceholderText.Visibility = Visibility.Collapsed;
        }

        private void MyTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            PlaceholderText.Visibility = string.IsNullOrEmpty(MyTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
