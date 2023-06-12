﻿using InitialProject.Application.Commands;
using InitialProject.Application.Factories;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.GuestOne
{
    public class MyAccommodationReservationRequestsViewModel : ViewModelBase
    {
        private readonly Guest1 _user;
        public List<AccommodationReservationMoveRequest> Requests { get; set; }
        private readonly AccommodationReservationRequestService _requestService;
        private readonly NavigationStore _navigationStore;
        public ICommand ShowCommentCommand { get; }
        public MyAccommodationReservationRequestsViewModel(NavigationStore navigationStore, Guest1 user)
        {
            _navigationStore = navigationStore;
            _user = user;
            _requestService = new AccommodationReservationRequestService();
            Requests = _requestService.GetByGuestId(_user.Id);
            ShowCommentCommand = new AccommodationReservationRequestClickCommand(ShowComment);
        }
        private void ShowComment(AccommodationReservationMoveRequest request)
        {
            if (string.IsNullOrEmpty(request.Comment))
            {
                if (TranslationSource.Instance.CurrentCulture.Name == "sr-Latn")
                    MessageBox.Show("Vlasnik nije ostavio komentar.");
                else
                    MessageBox.Show("The owner did not leave a comment.");
            }
            else
            {
                ViewModelBase viewModel = ViewModelFactory.Instance.CreateOwnerCommentVM(_navigationStore, _user, request);
                NavigationService.Instance.Navigate(viewModel);
            }
        }
    }
}
