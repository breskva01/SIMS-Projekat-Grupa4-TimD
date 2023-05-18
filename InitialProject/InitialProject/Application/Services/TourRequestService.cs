using InitialProject.Application.Observer;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Services
{
    public class TourRequestService
    {
        private readonly List<IObserver> _observers;
        private readonly ITourRequestRepository _repository;
    }
}
