using Microsoft.EntityFrameworkCore;
using MyBook.Entities;

namespace MyBook.Infrastructure.Repositories
{
    public class EFUserRepository : EfGenericRepository<User>, IUserRepository
    {
        public EFUserRepository(MyBookContext context) : base(context)
        { }

        public User GetUserWithSubscr(string userId)
        {
            return DbSet
                .Where(user => user.Id == userId)
                .Include(user => user.UserSubscrs)
                .ThenInclude(userSubscr => userSubscr.Subscription)
                .ThenInclude(subscr => subscr.Type)
                .FirstOrDefault()!;
        }

        public IQueryable<User> GetUsersWithSubscr()
        {
            return DbSet
                .Include(user => user.UserSubscrs)
                .ThenInclude(userSubscr => userSubscr.Subscription)
                .ThenInclude(subscr => subscr.Type);
        }

        public IQueryable<User> GetUsersWithName(string nameString)
        {
            return DbSet
                .Include(user => user.UserSubscrs)
                .ThenInclude(subscr => subscr.Subscription)
                .ThenInclude(subscr => subscr.Type)
                .Where(user => (user.FirstName + " " + user.LastName).Contains(nameString));
        }
    }
}
