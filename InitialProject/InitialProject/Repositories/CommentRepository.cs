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
    public class CommentRepository : ICommentRepository
    {
        private List<Comment> _comments;
        private readonly CommentFileHandler _fileHandler;
        private readonly AccommodationRepository _accommodationRepository;
        private readonly ForumRepository _forumRepository;
        public CommentRepository()
        {
            _fileHandler = new CommentFileHandler();
            _accommodationRepository = new AccommodationRepository();
            _forumRepository = new ForumRepository();
        }
        public List<Comment> GetAll()
        {
            return _comments = _fileHandler.Load();
        }

        public Comment GetById(int commentId)
        {
            GetAll();
            return _comments.Find(c => c.Id == commentId);
        }

        public void IncreaseReportCount(int commentId)
        {
            var comment = GetById(commentId);
            comment.ReportCount++;
            _fileHandler.Save(_comments);
        }

        public void Save(Comment comment)
        {
            GetAll();
            comment.Id = NextId();
            _comments.Add(comment);
            _fileHandler.Save(_comments);
        }
        private int NextId()
        {
            if (_comments == null || _comments.Count == 0)
            {
                return 1;
            }

            return _comments.Max(c => c.Id) + 1;
        }
        public List<Comment> GetCommentsByForumId(int id)
        {
            GetAll();
            List<Comment> comments = new List<Comment>();
            foreach(Comment comment in _comments)
            {
                if(comment.Forum.Id == id)
                {
                    comments.Add(comment);
                }
            }
            return comments;
        }
        public Comment SubmitComment(Forum forum, string text, User author)
        {
            int id = NextId();
            bool credentialAuthor = false;
            List<Accommodation> accommodations = _accommodationRepository.GetAll();
            foreach (Accommodation accommodation in accommodations)
            {
                credentialAuthor = accommodation.Owner.Id == author.Id && accommodation.Location == forum.Location;
                if(credentialAuthor)    
                    break;
            }
            Comment comment = new Comment(forum, text, author, DateTime.Now, credentialAuthor, true, false);
            comment.Id = id;
            if (credentialAuthor)
            {
                _comments.Add(comment);
                _fileHandler.Save(_comments);
                _forumRepository.UpdateCommentCount(forum, author.Id);
            }
            return comment;
        }
    }
}
