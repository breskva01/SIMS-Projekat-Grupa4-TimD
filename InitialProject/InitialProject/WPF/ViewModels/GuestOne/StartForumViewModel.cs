﻿using InitialProject.Application.Commands;
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
    public class StartForumViewModel : ViewModelBase
    {
        private readonly Guest1 _user;
        public string UserName => _user.FirstName;
        private readonly NavigationStore _navigationStore;
        private readonly ForumService _forumService;
        private readonly LocationService _locationService;
        public List<Location> Locations { get; set; }
        private List<string> _countries;
        public List<string> Countries
        {
            get { return _countries; }
            set
            {
                _countries = value;
                OnPropertyChanged();
            }
        }

        private List<string> _cities;
        public List<string> Cities
        {
            get { return _cities; }
            set
            {
                _cities = value;
                OnPropertyChanged();
            }
        }
        private string _selectedCountry;
        public string SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                if (value != _selectedCountry)
                {
                    _selectedCountry = value;
                    OnPropertyChanged();
                    PopulateCitiesComboBox();
                }
            }
        }

        private string _selectedCity;
        public string SelectedCity
        {
            get => _selectedCity;
            set
            {
                if (value != _selectedCity)
                {
                    _selectedCity = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Topic { get; set; }
        public string StartingComment { get; set; }
        public ICommand NavigateBackCommand { get; }
        public ICommand StartForumCommand { get; }
        public StartForumViewModel(NavigationStore navigationStore, Guest1 user)
        {
            _user = user;
            _navigationStore = navigationStore;
            _forumService = new ForumService();
            _locationService = new LocationService();
            Locations = new List<Location>(_locationService.GetAll());
            Countries = Locations.Select(l => l.Country).Distinct().ToList();
            //Cities = Locations.Select(c => c.City).Distinct().ToList();
            NavigateBackCommand = new ExecuteMethodCommand(NavigateForumBrowser);
            StartForumCommand = new ExecuteMethodCommand(StartForum);
            //Topic = "";
        }
        private void StartForum()
        {
            if (CheckIfAllFiedlsAreValid())
            {
                Location location = _locationService.GetByCityAndCountry(SelectedCity, SelectedCountry);
                _forumService.OpenForum(Topic, _user, StartingComment, location);
                NavigateForumBrowser();
            }
        }
        private bool CheckIfAllFiedlsAreValid()
        {
            if (string.IsNullOrEmpty(Topic))
            {
                MessageBox.Show("Morate uneti temu foruma, pre započinjanja istog.");
                return false;
            }
            if (string.IsNullOrEmpty(StartingComment))
            {
                MessageBox.Show("Morate uneti početni komentar/pitanje, pre započinjanja foruma.");
                return false;
            }
            if (string.IsNullOrEmpty(SelectedCountry) || string.IsNullOrEmpty(SelectedCity))
            {
                MessageBox.Show("Morate izabrati lokaciju, pre započinjanja foruma.");
                return false;
            }
            return true;
        }
        private void PopulateCitiesComboBox()
        {
            if (string.IsNullOrEmpty(SelectedCountry))
            {
                Cities = null;
                return;
            }
            Cities = Locations.Where(l => l.Country == SelectedCountry).Select(l => l.City).ToList();
        }
        private void NavigateForumBrowser()
        {
            ViewModelBase contentViewModel = new ForumBrowserViewModel(_navigationStore, _user);
            var navigateBarViewModel = new NavigationBarViewModel(_navigationStore, _user);
            var layoutViewModel = new LayoutViewModel(navigateBarViewModel, contentViewModel);
            new NavigationService(_navigationStore, layoutViewModel).Navigate();
        }
    }
}
