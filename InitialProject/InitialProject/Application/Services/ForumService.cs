using InitialProject.Application.Injector;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using MahApps.Metro.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Services
{
    public class ForumService
    {
        private readonly IForumRepository _forumRepository;
        private readonly ICommentRepository _commentRepository;
        public ForumService()
        {
            _forumRepository = RepositoryInjector.Get<IForumRepository>();
            _commentRepository = RepositoryInjector.Get<ICommentRepository>();
        }
        public void OpenForum(string topic, User iniator, string startingQuestion, Location location)
        {
            var forum = new Forum(iniator, ForumStatus.Open, location, topic, false);
            bool credentialUser = CheckIfUserHasCredentials(iniator, location);
            var comment = new Comment(forum, startingQuestion, iniator, DateTime.Now, credentialUser);

            _forumRepository.Save(forum);
            _commentRepository.Save(comment);
        }
        private bool CheckIfUserHasCredentials(User user, Location location)
        {
            if (user is Guest1 guest)
            {
                return CheckIfGuestVisited(guest, location);
            }
            //user is an owner
            return CheckIfOwnerOwnsAccommodation((Owner)user, location);
            
        }
        private bool CheckIfGuestVisited(Guest1 guest, Location location)
        {
            return false;
        }
        private bool CheckIfOwnerOwnsAccommodation(Owner owner, Location location)
        {
            return false;
        }
    }
}
