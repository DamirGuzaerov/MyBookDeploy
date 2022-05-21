using MyBook.Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Infrastructure.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public User GetUserWithSubscr(string userId);
        public IQueryable<User> GetUsersWithSubscr();
        public IQueryable<User> GetUsersWithName(string name);
    }
}
