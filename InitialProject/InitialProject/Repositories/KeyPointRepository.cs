using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    class KeyPointRepository : IKeyPointRepository
    {
        private readonly KeyPointFileHandler _fileHandler;
        private List<KeyPoint> _keyPoints;

        public KeyPointRepository()
        {
            _fileHandler = new KeyPointFileHandler();
            _keyPoints = _fileHandler.Load();
        }

        public List<KeyPoint> GetAll()
        {
            return _fileHandler.Load();
        }
        public KeyPoint GetById(int id)
        {
            _keyPoints = _fileHandler.Load();
            return _keyPoints.Find(k => k.Id == id);
        }
        
    }
}
