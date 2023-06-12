using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.WPF.ViewModels;
using InitialProject.WPF.ViewModels.GuestOne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Factories
{
    public class ViewModelFactory
    {
        private static readonly ViewModelFactory _instance = new ViewModelFactory();
        public static ViewModelFactory Instance
        {
            get { return _instance; }
        }
        public ViewModelBase CreateUnrateAccommodationsVM(NavigationStore navigationStore, Guest1 user)
        {
            return new UnratedAccommodationsViewModel(navigationStore, user);
        }
        public ViewModelBase CreateStartForumVM(NavigationStore navigationStore, Guest1 user)
        {
            return new StartForumViewModel(navigationStore, user);
        }
        public ViewModelBase CreateReceivedRatingsVM(NavigationStore navigationStore, Guest1 user)
        {
            return new ReceivedRatingsViewModel(navigationStore, user);
        }
        public ViewModelBase CreateRateAccommodationVM(NavigationStore navigationStore, AccommodationReservation reservation)
        {
            return new RateAccommodationViewModel(navigationStore, reservation);
        }
        public ViewModelBase CreateOwnerCommentVM(NavigationStore navigationStore, Guest1 user, AccommodationReservationMoveRequest request)
        {
            return new OwnerCommentViewModel(navigationStore, user, request);
        }
        public ViewModelBase CreateImageBrowserVM(List<string> imageUrls, string imageUrl, Action navigateAction)
        {
            return new ImageBrowserViewModel(imageUrls, imageUrl, navigateAction);
        }
        public ViewModelBase CreateGuestRatingDetailsVM(NavigationStore navigationStore, Guest1 user, GuestRating rating)
        {
            return new GuestRatingDetailsViewModel(navigationStore, user, rating);
        }
        public ViewModelBase CreateForumCommentsVM(NavigationStore navigationStore, Guest1 user, Forum forum)
        {
            return new WPF.ViewModels.GuestOne.ForumCommentsViewModel(navigationStore, user, forum);
        }
        public ViewModelBase CreateReservationFormVM(NavigationStore navigationStore, Guest1 user, Accommodation accommodation)
        {
            return new AccommodationReservationViewModel(navigationStore, user, accommodation);
        }
        public ViewModelBase CreateReservationMoveRequestVM(NavigationStore navigationStore, AccommodationReservation reservation)
        {
            return new AccommodationReservationMoveRequestViewModel(navigationStore, reservation);
        }
        public ViewModelBase CreateReservationDetailsVM(NavigationStore navigationStore, Guest1 user, AccommodationReservation reservation, bool isReadOnlyMode = true)
        {
            return new AccommodationReservationDetailsViewModel(navigationStore, user, reservation, isReadOnlyMode);
        }
        public ViewModelBase CreateReservationDatePickerVM(NavigationStore navigationStore, List<AccommodationReservation> reservations)
        {
            return new AccommodationReservationDatePickerViewModel(navigationStore, reservations);
        }
        public ViewModelBase CreateSignInVM(NavigationStore navigationStore)
        {
            return new SignInViewModel(navigationStore);
        }
        public ViewModelBase CreateAccommodationBrowserVM(NavigationStore navigationStore, Guest1 user) 
        {
            ViewModelBase viewModel = new AccommodationBrowserViewModel(navigationStore, user);
            return CreateLayoutViewModel(viewModel, navigationStore, user);
        }
        public ViewModelBase CreateAnywhereAnytimeVM(NavigationStore navigationStore, Guest1 user)
        {
            ViewModelBase viewModel = new AnywhereAnytimeViewModel(navigationStore, user);
            return CreateLayoutViewModel(viewModel, navigationStore, user);
        }
        public ViewModelBase CreateMyReservationsVM(NavigationStore navigationStore, Guest1 user)
        {
            ViewModelBase viewModel = new MyAccommodationReservationsViewModel(navigationStore, user);
            return CreateLayoutViewModel(viewModel, navigationStore, user);
        }
        public ViewModelBase CreateMyRequestsVM(NavigationStore navigationStore, Guest1 user)
        {
            ViewModelBase viewModel = new MyAccommodationReservationRequestsViewModel(navigationStore, user);
            return CreateLayoutViewModel(viewModel, navigationStore, user);
        }
        public ViewModelBase CreateRatingsVM(NavigationStore navigationStore, Guest1 user, int selectedTab = 0)
        {
            ViewModelBase viewModel = new AccommodationRatingViewModel(navigationStore, user, selectedTab);
            return CreateLayoutViewModel(viewModel, navigationStore, user);
        }
        public ViewModelBase CreateForumBrowserVM(NavigationStore navigationStore, Guest1 user)
        {
            ViewModelBase viewModel = new ForumBrowserViewModel(navigationStore, user);
            return CreateLayoutViewModel(viewModel, navigationStore, user);
        }
        private ViewModelBase CreateLayoutViewModel(ViewModelBase contentViewModel, NavigationStore navigationStore, Guest1 user)
        {
            var navigateBarViewModel = new NavigationBarViewModel(navigationStore, user);
            var layoutViewModel = new LayoutViewModel(navigateBarViewModel, contentViewModel);
            return layoutViewModel;
        }
    }
}
