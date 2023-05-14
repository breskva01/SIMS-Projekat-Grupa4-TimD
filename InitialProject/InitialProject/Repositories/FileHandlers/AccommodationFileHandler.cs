using InitialProject.Application.Serializer;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories.FileHandlers
{
    public class AccommodationFileHandler
    {
        private const string _accommodationsFilePath = "../../../Resources/Data/accommodations.csv";
        private readonly Serializer<Accommodation> _serializer;

        public AccommodationFileHandler()
        {
            _serializer = new Serializer<Accommodation>();
        }
        public List<Accommodation> Load()
        {
            var accommodations = _serializer.FromCSV(_accommodationsFilePath);
            FillInOwners(accommodations);
            return accommodations;
        }
        private void FillInOwners(List<Accommodation> accommodations)
        {
            var users = new UserFileHandler().Load();
            accommodations.ForEach(a =>
                a.Owner = (Owner)users.Find(u => u.Id == a.Owner.Id));
        }
        public void Save(List<Accommodation> accommmodations)
        {
            _serializer.ToCSV(_accommodationsFilePath, accommmodations);
        }
    }
}
