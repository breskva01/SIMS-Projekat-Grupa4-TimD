using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.WPF.NewViews;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class ComplexTourAcceptViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private User _user;

        private TourRequest _selectedRequest;
        public TourRequest SelectedRequest
        {
            get { return _selectedRequest; }
            set
            {
                if (_selectedRequest != value)
                {
                    _selectedRequest = value;
                    OnPropertyChanged(nameof(SelectedRequest));
                }
            }
        }

        private readonly ObservableCollection<ComplexTourRequest> _complexTourRequests;
        public IEnumerable<ComplexTourRequest> ComplexTourRequests => _complexTourRequests;
        
        private ComplexTourRequestService _complexTourRequestService;
        private TourRequestService _tourRequestService;

        public ICommand AcceptTourRequestCommand { get; set; }

        public ComplexTourAcceptViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;

            _complexTourRequestService = new ComplexTourRequestService();
            _tourRequestService = new TourRequestService();

            _complexTourRequests = new ObservableCollection<ComplexTourRequest>(_complexTourRequestService.GetAll());

            AcceptTourRequestCommand = new ExecuteMethodCommand(AcceptTourRequest);


        }
        private void AcceptTourRequest()
        {
            

            if(SelectedRequest != null && SelectedRequest.Status != RequestStatus.Approved)
            {
                TourRequestsAcceptDateListPickerView view = new TourRequestsAcceptDateListPickerView(_navigationStore, _user, SelectedRequest);
                view.Show();
                SelectedRequest = null;
                return;
            }
            return;
            

            /*
            TourCreationViewModel viewModel = new TourCreationViewModel(_navigationStore, _user, SelectedTourRequest);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
            navigate.Execute(null);
            */
        }
    }
}
