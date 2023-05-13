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

namespace InitialProject.WPF.NewViews.Guest1
{
    /// <summary>
    /// Interaction logic for StatusBar.xaml
    /// </summary>
    public partial class StatusBar : UserControl
    {
        public StatusBar()
        {
            InitializeComponent();
            Timer_Tick(null, null);
            DispatcherTimer LiveTime = new DispatcherTimer();
            LiveTime.Interval = TimeSpan.FromSeconds(60);
            LiveTime.Tick += Timer_Tick;
            LiveTime.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTextBox.Text = DateTime.Now.ToString("dd.MM.yyyy.");
        }
    }
}
