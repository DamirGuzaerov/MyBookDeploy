using System.Text;
using Microsoft.AspNetCore.Identity;
using MyBook.Controllers;
using MyBook.Entities;
using MyBook.Infrastructure.Repositories;
using Repositories;

namespace MyBook.Models;

public class BookViewModel
{
    private readonly EfBookRepository _bookRepository;
    private readonly IGenericRepository<Author> _authorRepository;
    private readonly EFHistoryRepository _historyRepository;
    private readonly IGenericRepository<MyBook.Entities.Type> _typeRepository;
    private readonly IGenericRepository<Genre> _genreRepository;
    public readonly Book? _resultBook;
    public User? User;
    private readonly ILogger<BookController> _logger;
    private readonly UserManager<User> _userManager;
    private string userId;
    public List<DownloadLink> downloadLink;
    public IGenericRepository<DownloadLink> _linksRepository;

    public BookViewModel(EFHistoryRepository historyRepository, IGenericRepository<DownloadLink> linksRepository,
        IGenericRepository<MyBook.Entities.Type> typeRepository, EfBookRepository bookRepository,
        int bookId, User user,
        ILogger<BookController> logger)
    {
        _historyRepository = historyRepository;
        _linksRepository = linksRepository;
        _typeRepository = typeRepository;
        _bookRepository = bookRepository;
        _resultBook = _bookRepository.GetFullBook(bookId);
        User = user;
        _logger = logger;
        downloadLink = linksRepository.GetWithInclude(link =>
            _resultBook != null && _resultBook.BookDescId == link.BookDescId).ToList();
    }
    
    public bool HasPremiumSubscription()
    {
        var type = GetTypes().FirstOrDefault(it => it.TypeName == "Премиум");
        if (type != null)
        {
            var user = HasSubscription(type.TypeId, null, null);
            return user.Result != null;
        }

        return false;
    }

    public bool HasGenreSubsciption()
    {
        int genreId;
        genreId = _resultBook!.BookGenres.First().Genre.GenreId;
        var type = GetTypes().FirstOrDefault(it => it.TypeName == "Подписка на жанр");
        Task<User?> user = null;
        if (type != null)
            user = HasSubscription(type.TypeId, genreId: genreId, authorId: null);
        
        if(user!=null)
         return user.Result != null;
        return false;
    }


    public bool HasAuthorSubscription()
    {
        int authorId;
        if (_resultBook!.AuthorBooks.Count == 0) return false;
        authorId = _resultBook.AuthorBooks.FirstOrDefault()!.Author.AuthorId;
        var type = GetTypes().FirstOrDefault(it => it.TypeName == "Подписка на автора");
        if (type != null)
        {
            var user = HasSubscription(type.TypeId, genreId: null, authorId: authorId);
            return user.Result != null;
        }

        return false;
    }
    
    private async Task<User?> HasSubscription(int typeId, int? genreId, int? authorId)
    {
        if (User == null)
            return null;
        if (User.UserSubscrs != null && (User.UserSubscrs.Any(it =>
                it.Subscription.TypeId == typeId &&
                ((genreId != null && it.Subscription.GenreId == genreId) ||
                 (authorId != null && it.Subscription.AuthorId == authorId)
                 || it.Subscription.Type.TypeName == "Премиум"))))
            return User;
        return null;
    }


    private List<MyBook.Entities.Type> GetTypes()
    {
        return _typeRepository.Get().ToList();
    }

    public bool CheckAge()
    {
        if (User == null) return true;
        var today = DateTime.Today;
        string[] date = User.BirthDate.Split('.');
        int yearBirth;
        _logger.LogInformation(User.BirthDate);
        bool success = int.TryParse(date[2], out yearBirth);
        var age = today.Year - yearBirth;
        return _resultBook != null && (_resultBook.IsForAdult && age >= 18 || !_resultBook.IsForAdult);
    }

    public bool CheckHistory()
    {
        if (User == null) return true;
        return (!_historyRepository.GetWithInclude(h =>
            _resultBook != null && _resultBook.BookId == h.BookId && User.Id == h.UserId).Any());
    }

    public bool IsPermitted()
    {
        return _resultBook != null && (_resultBook.IsPaid == false || HasGenreSubsciption() || HasAuthorSubscription()
                                       || HasPremiumSubscription());
    }
}