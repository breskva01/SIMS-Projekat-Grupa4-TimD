using InitialProject.Controller;
using InitialProject.FileHandler;
using InitialProject.Forms;
using InitialProject.Model;
using InitialProject.Model.DAO;
using InitialProject.Repository;
using InitialProject.View;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace InitialProject
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {

        private readonly UserController _userController;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SignInForm()
        {
            InitializeComponent();
            DataContext = this;
            _userController = new UserController();
            //Only to speed up testing
            User user = _userController.GetByUsername("Zika");
            AccommodationBrowser accommodationBrowser = new AccommodationBrowser(user);
            accommodationBrowser.Show();
            Close();
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _userController.GetByUsername(Username);
            if (user != null)
            {
                if(user.Password == txtPassword.Password)
                {
                    OpenAppropriateWindow(user);
                    Close();
                } 
                else
                {
                    MessageBox.Show("Wrong password!");
                }
            }
            else
            {
                MessageBox.Show("Wrong username!");
            }
            
        }
        private void OpenAppropriateWindow(User user)
        {
            switch (user.UserType)
            {
                // TO DO: otrvoriti odgovarajuce prozore za svaki tip korisnika
                case UserType.Owner:
                    {
                        CommentsOverview commentsOverview = new CommentsOverview(user);
                        commentsOverview.Show();
                        break;
                    }
                case UserType.Guest1:
                    {
                        AccommodationBrowser accommodationBrowser = new AccommodationBrowser(user);
                        accommodationBrowser.Show();
                        break;
                    }
                case UserType.TourGuide:
                    {
                        CommentsOverview commentsOverview = new CommentsOverview(user);
                        commentsOverview.Show();
                        break;
                    }
                case UserType.Guest2:
                    {
                        CommentsOverview commentsOverview = new CommentsOverview(user);
                        commentsOverview.Show();
                        break;
                    }
            }
        }
    }
}
