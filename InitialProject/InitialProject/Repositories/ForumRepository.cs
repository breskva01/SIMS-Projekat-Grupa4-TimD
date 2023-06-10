using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories.FileHandlers;
using InitialProject.Repository;
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
        private readonly UserRepository _userRepository;
        public ForumRepository()
        {
            _fileHandler = new ForumFileHandler();
            _userRepository = new UserRepository();
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
        public Forum GetForumByLocationAndTopic(string locationAndTopic)
        {
            foreach(Forum forum in _forums)
            {
                if (locationAndTopic.Contains(forum.Location.City) && locationAndTopic.Contains(forum.Location.Country) && locationAndTopic.Contains(forum.Topic))
                    return forum;
            }
            return null;
        }
        public void UpdateCommentCount(Forum forum, int authorId)
        {
            GetAll();
            Forum newForum = forum;
            List<User> users= _userRepository.GetAll();
            User user = users.Find(u => u.Id == authorId);
            if(user is Owner)
            {
                newForum.OwnerComments += 1;
            }
            else 
            {
                newForum.GuestComments += 1;
            }
            foreach (Forum f in _forums.ToList())
            {
                if (f.Id == forum.Id)
                {
                    _forums.Remove(f);
                    break;
                }
            }

            newForum.VeryUseful = newForum.OwnerComments == 10 || newForum.GuestComments == 20;
            _forums.Add(newForum);
            _fileHandler.Save(_forums);
        }
    }
}
