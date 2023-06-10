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
        public List<Forum> GetAll() 
        { 
            return _forumRepository.GetAll(); 
        }
        public List<Forum> GetByIniatorId(int id)
        {
            return _forumRepository.GetByIniatorId(id);
        }
        public Forum GetById(int forumId)
        {
            return _forumRepository.GetById(forumId);
        }
        public void OpenForum(string topic, User iniator, string startingQuestion, Location location)
        {
            var forum = new Forum(iniator, ForumStatus.Open, location, topic, false);
            bool credentialUser = CheckIfUserHasCredentials(iniator, location);
            var comment = new Comment(forum, startingQuestion, iniator, DateTime.Now, credentialUser);

            _forumRepository.Save(forum);
            _commentRepository.Save(comment);
            //TO DO: Napravite obavestenje za vlasnike
        }
        public void Close(Forum forum)
        {
            _forumRepository.Close(forum.Id);
        }
        public void PostComment(Forum forum, User user, string commentText)
        {
            bool credentialUser = CheckIfUserHasCredentials(user, forum.Location);
            var comment = new Comment(forum, commentText, user, DateTime.Now, credentialUser);
            _commentRepository.Save(comment);
            UpdateVeryUsefulStatus(forum.Id);
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
            var reservationService = new AccommodationReservationService();
            return reservationService.CheckIfGuestVisited(guest, location);
        }
        private bool CheckIfOwnerOwnsAccommodation(Owner owner, Location location)
        {
            var accommodationService = new AccommodationService();
            return accommodationService.CheckIfOwnerOwnsAccommodation(owner, location);
        }
        private void UpdateVeryUsefulStatus(int forumId)
        {
            int credentialGuestComments = 0;
            int credentialOwnerComments = 0;
            int guestCommentsNeeded = 20;
            int ownerCommentsNeeded = 10;
            var forum = _forumRepository.GetById(forumId);
            //minimum possible number of comments for a forum to become very useful
            if (forum.Comments.Count < guestCommentsNeeded + ownerCommentsNeeded) 
                return;

            CountComments(ref credentialGuestComments, ref credentialOwnerComments, forum);
            if (credentialGuestComments >= guestCommentsNeeded && credentialOwnerComments >= ownerCommentsNeeded)
                _forumRepository.MarkAsVeryUseful(forum.Id);
        }
        private void CountComments(ref int credentialGuestComments, ref int credentialOwnerComments, Forum forum)
        {
            foreach (Comment comment in forum.Comments)
            {
                if (!comment.CredentialAuthor)
                    continue;
                if (comment.Author is Guest1)
                    credentialGuestComments++;
                else
                    credentialOwnerComments++;
            }
        }
    }
}
