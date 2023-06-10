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
    public class CommentRepository : ICommentRepository
    {
        private List<Comment> _comments;
        private readonly CommentFileHandler _fileHandler;
        public CommentRepository()
        {
            _fileHandler = new CommentFileHandler();
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
    }
}
