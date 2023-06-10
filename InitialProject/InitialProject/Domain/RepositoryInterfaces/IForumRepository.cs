using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IForumRepository : IRepository<Forum>
    {
        List<Forum> GetByIniatorId(int id);
        Forum GetById(int forumId);
        void Save(Forum forum);
        void Close(int forumId);
        void MarkAsVeryUseful(int forumId);
    }
}
