using InitialProject.Domain.Models;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IComplexTourRequestRepository : IRepository<ComplexTourRequest>
    {
        public void Delete(ComplexTourRequest complexTourRequest);
        public List<ComplexTourRequest> GetAll();
        public List<ComplexTourRequest> GetApproved(List<ComplexTourRequest> userRequests);
        public List<ComplexTourRequest> GetOnHold();
        public ComplexTourRequest GetById(int complexTourRequestId);
        public List<ComplexTourRequest> GetByUser(int userId, List<ComplexTourRequest> complexTourRequests);
        public List<ComplexTourRequest> CheckIfInvalid(List<ComplexTourRequest> complexTourRequests);
        public List<ComplexTourRequest> CheckIfApproved(List<ComplexTourRequest> complexTourRequests);
        public bool AreRequestsPartiallyApproved(ComplexTourRequest complexTourRequest);
        public bool AreAllRequestsApproved(ComplexTourRequest complexTourRequest);
        public bool IsAnyRequestApproved(ComplexTourRequest complexTourRequest);
        public DateTime GetEarliestDate(ComplexTourRequest complexTourRequest);
        public int NextId();
        public ComplexTourRequest Save(ComplexTourRequest complexTourRequest);
        public ComplexTourRequest Update(ComplexTourRequest complexTourRequest);
    }
}
