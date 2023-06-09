using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Comment GetById(int commentId);
        void IncreaseReportCount(int commentId);
        void Save(Comment comment);
    }
}
