using MyBook.Entities;
using Microsoft.EntityFrameworkCore;
namespace MyBook.Infrastructure.Repositories
{
    public class EFHistoryRepository : EfGenericRepository<History>, IHistoryRepository
    {
        public EFHistoryRepository(MyBookContext context) : base(context)
        { }

        public IQueryable<History> GetHistories(string userId)
        {
            return DbSet.Where(x => x.UserId == userId)
                .Include(history => history.Book)
            .ThenInclude(book => book.Description).Include(history => history.Book)
            .ThenInclude(book => book.BookGenres)
            .Include(history => history.Book)
            .ThenInclude(book => book.AuthorBooks);
        }

        public bool CheckHistory(string userId, int bookId)
        {
            return DbSet.Any(x => x.UserId == userId && x.BookId==bookId);
        }
    }
}
