using InitialProject.Application.Serializer;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories.FileHandlers
{
    public class ForumFileHandler
    {
        private const string _forumsFilePath = "../../../Resources/Data/forums.csv";
        private readonly Serializer<Forum> _serializer;

        public ForumFileHandler()
        {
            _serializer = new Serializer<Forum>();
        }
        public List<Forum> Load()
        {
            var forums = _serializer.FromCSV(_forumsFilePath);
            FillInIniators(forums);
            FillInLocations(forums);
            FillInComments(forums);
            SortComments(forums);
            return forums;
        }
        private void FillInIniators(List<Forum> forums)
        {
            var users = new UserFileHandler().Load();
            forums.ForEach(f => f.Initiator = users.Find(u => u.Id == f.Initiator.Id));
        }
        private void FillInLocations(List<Forum> forums)
        {
            var locations = new LocationFileHandler().Load();
            forums.ForEach(f => f.Location = locations.Find(l => l.Id == f.Location.Id));
        }
        private void FillInComments(List<Forum> forums)
        {
            var comments = new CommentFileHandler().Load();
            foreach(Comment comment in comments)
            {
                var forum = forums.Find(f => f.Id == comment.Forum.Id);
                comment.Forum = forum;
                forum.Comments.Add(comment);
            }

        }
        private void SortComments(List<Forum> forums)
        {
            foreach (var forum in forums)
            {
                forum.Comments = forum.Comments.OrderBy(comment => comment.PostTime).ToList();
            }
        }

        public void Save(List<Forum> forums)
        {
            _serializer.ToCSV(_forumsFilePath, forums);
        }
    }
}
