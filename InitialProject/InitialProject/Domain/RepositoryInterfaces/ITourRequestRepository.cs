using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ITourRequestRepository : IRepository<TourRequest>
    {
        public TourRequest GetById(int tourRequestId);
        public TourRequest Update(TourRequest tourRequest);
        public TourRequest Save(TourRequest tourRequest);
        public int NextId();
        public void Delete(TourRequest tourRequest);

    }
}
