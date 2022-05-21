using MyBook.Core.Interfaces;
using MyBook.Entities;
using MyBook.Infrastructure.Repositories;

namespace MyBook.Services
{
    public class RecommendationsService : IRecommendationsService
    {
        private EFHistoryRepository _historyRepository;
        private EfBookRepository _bookRepository;
        private const int BooksCountInPage = 15;

        public RecommendationsService(EFHistoryRepository historyRepository, EfBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
            _historyRepository = historyRepository;
        }

        public async Task<List<Book>> GetRecommendationsAsync(string userId,int page)
        {
            var history = _historyRepository.GetHistories(userId);
            var topTen = _bookRepository.GetTopBooks().Take(10).ToList();
            if (history == null || !history.Any())
            {
                return topTen;
            }
            var recommends = new List<Book>();
            var genresIds = history.Where(history => history.Book.BookGenres.Any()).Select(history => history.Book.BookGenres.First().GenreId).ToList();
            if (genresIds != null && genresIds.Any())
            {
                genresIds.ForEach(genreId => recommends.AddRange(_bookRepository.GetBookForGenre(genreId)));
            }
            var authorsIds = history.Where(history => history.Book.AuthorBooks.Any()).Select(history => history.Book.AuthorBooks.First().AuthorId).ToList();
            if (authorsIds != null && authorsIds.Any())
            {
                authorsIds.ForEach(authorId => recommends.AddRange(_bookRepository.GetBookForAuthor(authorId)));
            }
            recommends = recommends.DistinctBy(it => it.BookId).Where(it => !history.Any(history => history.BookId == it.BookId)).ToList();
            if (recommends.Count() < 5)
            {
                recommends.AddRange(topTen);
                recommends = recommends.DistinctBy(it => it.BookId).Where(it => !history.Any(history => history.BookId == it.BookId)).ToList();
            }
            return recommends.Take(page*BooksCountInPage).ToList();
        }
    }
}
