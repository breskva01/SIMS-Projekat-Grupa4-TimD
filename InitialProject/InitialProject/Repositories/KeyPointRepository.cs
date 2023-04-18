using InitialProject.Domain.Models;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    class KeyPointRepository
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
        /*
        public KeyPoint Update(KeyPoint keyPoint)
        {
            _keyPoints = _fileHandler.Load();
            KeyPoint updated = _keyPoints.Find(k => k.Id == keyPoint.Id);
            _keyPoints.Remove(updated);
            _keyPoints.Add(keyPoint);
            _fileHandler.Save(_keyPoints);
            return keyPoint;
        }
        public KeyPoint Save(KeyPoint keyPoint)
        {
            keyPoint.Id = NextId();
            _keyPoints = _fileHandler.Load();
            _keyPoints.Add(keyPoint);
            _fileHandler.Save(_keyPoints);
            return keyPoint;
        }
        public int NextId()
        {
            _keyPoints = _fileHandler.Load();
            if (_keyPoints.Count < 1)
            {
                return 1;
            }
            return _keyPoints.Max(t => t.Id) + 1;
        }
        */
        
    }
}
