using InitialProject.Application.Serializer;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories.FileHandlers
{
    public class CommentFileHandler
    {
        private const string _commentsFilePath = "../../../Resources/Data/comments.csv";
        private readonly Serializer<Comment> _serializer;

        public CommentFileHandler()
        {
            _serializer = new Serializer<Comment>();
        }
        public List<Comment> Load()
        {
            var comments = _serializer.FromCSV(_commentsFilePath);
            FillInAuthors(comments);
            return comments;
        }
        private void FillInAuthors(List<Comment> comments)
        {
            var users = new UserFileHandler().Load();
            comments.ForEach(c =>
                c.Author = users.Find(u => u.Id == c.Author.Id));
        }
        public void Save(List<Comment> comments)
        {
            _serializer.ToCSV(_commentsFilePath, comments);
        }
    }
}
