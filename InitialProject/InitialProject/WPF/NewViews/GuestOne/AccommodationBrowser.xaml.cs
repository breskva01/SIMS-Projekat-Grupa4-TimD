using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InitialProject.WPF.NewViews.GuestOne
{
    /// <summary>
    /// Interaction logic for AccommodationBrowserView.xaml
    /// </summary>
    public partial class AccommodationBrowser : UserControl
    {
        public AccommodationBrowser()
        {
            InitializeComponent();
        }

        private void ScrollEvent(object sender, ScrollEventArgs e)
        {
            if (sender == scrollBar)
            {
                scrollViewer.ScrollToVerticalOffset(e.NewValue);
            }
        }

        private void ScrollViewerEvent(object sender, ScrollChangedEventArgs e)
        {
            if (sender == scrollViewer)
            {
                scrollBar.Value = scrollViewer.VerticalOffset;
            }
        }

        private void ResetEvent(object sender, RoutedEventArgs e)
        {
            scrollViewer.ScrollToVerticalOffset(0);
        }
    }
}
