using InitialProject.Application.Serializer;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories.FileHandlers
{
    public class KeyPointFileHandler
    {
        private const string _keyPointsFilePath = "../../../Resources/Data/keyPoints.csv";
        private readonly Serializer<KeyPoint> _serializer;

        public KeyPointFileHandler()
        {
            _serializer = new Serializer<KeyPoint>();
        }
        public List<KeyPoint> Load()
        {
            return _serializer.FromCSV(_keyPointsFilePath);
        }
    }
}
