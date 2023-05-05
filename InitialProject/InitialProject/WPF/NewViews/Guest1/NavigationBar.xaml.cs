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

namespace InitialProject.WPF.NewViews.Guest1
{
    /// <summary>
    /// Interaction logic for NavigationBar.xaml
    /// </summary>
    public partial class NavigationBar : UserControl
    {
        public NavigationBar()
        {
            InitializeComponent();
        }

        private void PersonImageLeftClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                Image image = (Image)sender;
                ContextMenu contextMenu = image.ContextMenu;
                if (contextMenu != null)
                {
                    contextMenu.PlacementTarget = image;
                    contextMenu.IsOpen = true;
                    e.Handled = true;
                }
            }
        }
    }
}
