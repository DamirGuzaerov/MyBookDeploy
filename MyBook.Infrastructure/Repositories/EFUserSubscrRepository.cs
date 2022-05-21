using Microsoft.EntityFrameworkCore;
using MyBook.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Infrastructure.Repositories
{
    public class EFUserSubscrRepository : EfGenericRepository<UserSubscr>, IUserSubscrRepository
    {
        public EFUserSubscrRepository(MyBookContext context) : base(context)
        { }

        public UserSubscr? GetUserSubscr(string userId, int subscrId)
        {
            return DbSet.Where(it => it.UserId == userId && it.SubscriptionId == subscrId)
                .Include(it => it.Subscription)
                .Include(it => it.User)
                .FirstOrDefault();
        }

        public IEnumerable<UserSubscr> GetUserWithAllSubscr(string userId)
        {
            return DbSet.Where(it => it.UserId == userId)
                .Include(it => it.Subscription)
                .ThenInclude(it => it.Genre)
                .Include(it => it.Subscription)
                .ThenInclude(it => it.Author)
                .Include(it => it.Subscription)
                .ThenInclude(it => it.Type);
        }

        public IEnumerable<UserSubscr>? GetExpiredUserSubscrs(string userId, int expireIn = 0)
        {
            var subscrs = DbSet.Where(it => it.UserId == userId)
                .Include(it => it.Subscription).ThenInclude(it => it.Type)
                .Include(it => it.Subscription).ThenInclude(it => it.Author)
                .Include(it => it.Subscription).ThenInclude(it => it.Genre).ToList();
            return subscrs.Where(it => DateTime.Parse(it.Subscription.EndDate).CompareTo(DateTime.Now.AddDays(expireIn)) < 0);
        }

        public async Task DeleteExpiredUserSubscrs(IEnumerable<UserSubscr> subscrs)
        {
            if (subscrs != null && subscrs.Any())
                await RemoveAll(subscrs.ToList());
        }
    }
}
