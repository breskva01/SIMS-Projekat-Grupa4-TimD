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
    public class ForumRepository : IForumRepository
    {
        private List<Forum> _forums;
        private readonly ForumFileHandler _fileHandler;
        public ForumRepository()
        {
            _fileHandler = new ForumFileHandler();
        }

        public Forum GetById(int forumId)
        {
            GetAll();
            return _forums.Find(f => f.Id == forumId);
        }

        public void Save(Forum forum)
        {
            GetAll();
            forum.Id = NextId();
            _forums.Add(forum);
            _fileHandler.Save(_forums);
        }

        public void Close(int forumId)
        {
            var forum = GetById(forumId);
            forum.Status = ForumStatus.Closed;
            _fileHandler.Save(_forums);
        }

        public void MarkAsVeryUseful(int forumId)
        {
            var forum = GetById(forumId);
            forum.VeryUseful = true;
            _fileHandler.Save(_forums);
        }

        public List<Forum> GetAll()
        {
            return _forums = _fileHandler.Load();
        }
        private int NextId()
        {
            return _forums?.Max(f => f.Id) + 1 ?? 0;
        }
    }
}
