using InitialProject.Domain.Models;
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
using System.Windows.Shapes;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for Probavanje.xaml
    /// </summary>
    public partial class Probavanje : Window
    {
        User user { get; set; }
        public Probavanje(User user)
        {
            InitializeComponent();
            DataContext = this;
        }

        private void CancelTourClick(object sender, RoutedEventArgs e)
        {
            AllToursView allToursView = new AllToursView(user);
            this.Close();
            allToursView.Show();
        }
    }
}
