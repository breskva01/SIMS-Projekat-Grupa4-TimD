using InitialProject.Controller;
using InitialProject.Forms;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.View;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace InitialProject
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {
        
       
        private readonly UserRepository _repository;

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
            _repository = new UserRepository();
            //Only to speed up testing
            /*User user = _repository.GetByUsername("Zika");
            AccommodationBrowser accommodationBrowser = new AccommodationBrowser(user);
            accommodationBrowser.Show();
            Close();*/
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _repository.GetByUsername(Username);
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
                // TO DO: otvoriti odgovarajuce prozore za svaki tip korisnika
                case UserType.Owner:
                    {
                        AccommodationRegistrationView accommodationRegistrationView = new AccommodationRegistrationView(user);
                        accommodationRegistrationView.Show();
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
                        GuideUserIntefaceView guideInteface = new GuideUserIntefaceView(user);
                        guideInteface.Show();
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
