using InitialProject.Application.Serializer;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using InitialProject.Repositories.FileHandlers;
using InitialProject.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Input;
using System.Windows.Navigation;

namespace InitialProject.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserFileHandler _fileHandler;
        /*private readonly IAccommodationRatingRepository _accommmodationRatingRepository;
        private List<AccommodationRating> _ratings;*/

        private List<User> _users;

        public UserRepository()
        {
            _fileHandler = new UserFileHandler();
            //_accommmodationRatingRepository = RepositoryStore.GetIAccommodationRatingRepository;
        }

        public List<User> GetAll()
        {
            return _users = _fileHandler.Load();
        }

        public List<Guest1> GetAllGuest1s()
        {
            GetAll();
            return _users.OfType<Guest1>().ToList();
        }

        public User GetById(int id)
        {
            GetAll();
            return _users.Find(u => u.Id == id);
        }
        public User GetByUsername(string username)
        {
            GetAll();
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public bool IsEligibleForFreeVoucher(Guest2 guest)
        {
            if(guest.FreeVoucherProgress == 0)
            {
                guest.FreeVoucherProgressLimit = DateTime.UtcNow.AddYears(1);
            }

            if(DateTime.UtcNow.CompareTo(guest.FreeVoucherProgressLimit) > 0)
            {
                guest.FreeVoucherProgress = 1;
                guest.FreeVoucherProgressLimit = DateTime.UtcNow.AddYears(1);
                Update(guest);
                return false;
            }
            else
            {
                guest.FreeVoucherProgress++;
                if(guest.FreeVoucherProgress == 5)
                {
                    guest.FreeVoucherProgress = 0;
                    Update(guest);
                    return true;
                }
                Update(guest);
                return false;
            }

        }

        public User Update(User user)
        {
            GetAll();
            User updated = _users.Find(t => t.Id == user.Id);
            _users.Remove(updated);
            _users.Add(user);
            _fileHandler.Save(_users);
            return user;
        }
        public string CheckSuperGuide(List<Tour> finishedTours, List<RatingViewModel> ratings, List<TourReservation> reservations, int GuideId)
        {
            int suma = 0;
            int brojac = 0;
            List<Tour> acceptableTours = new List<Tour>();
            foreach (Tour tour in finishedTours)
            {
                foreach(TourReservation reservation in reservations )
                {
                    if(tour.Id == reservation.TourId)
                    {
                        List<RatingViewModel> tourRatings = (List<RatingViewModel>)ratings.Where(r => r.TourId == tour.Id);
                        foreach(RatingViewModel rating in tourRatings)
                        {
                            suma += + Convert.ToInt32(rating.TourContent) + Convert.ToInt32(rating.GuideKnowledge) + Convert.ToInt32(rating.TourInteresting) + Convert.ToInt32(rating.TourInformative) + Convert.ToInt32(rating.GuideLanguage);
                            brojac += 5;
                        }
                        double prosek = (double) suma / brojac;
                        if(prosek > 4.0) 
                        {
                            acceptableTours.Add(tour);
                            suma = 0;
                            brojac = 0;
                        }
                    }
                }
            }
            var tourCountsByLanguage = acceptableTours
            .GroupBy(t => t.Language)
            .Select(g => new { Language = g.Key, Count = g.Count() })
            .Where(t => t.Count > 20)
            .ToList();

            var languageWithMostTours = tourCountsByLanguage
            .OrderByDescending(t => t.Count)
            .FirstOrDefault();

            if(languageWithMostTours != null)
            {
                return "Yes(" + languageWithMostTours.Language + ")";
            }
            return "No";
        }
    }
}
