using Microsoft.AspNetCore.Mvc.Rendering;
using MyBook.Entities;
using MyBook.Infrastructure.Helpers;
using MyBook.Infrastructure.Repositories;
using Repositories;

namespace MyBook.Models;

public class FreeBooksViewModel
{
    private readonly EfBookRepository _bookRepository;
    private readonly EFGenreRepository _genreRepository;
    private readonly IGenresFilterGetter _genresFilterGetter;
    private readonly ILanguageFilterGetter _languageFilterGetter;
    public IQueryable<Book> AllBooks { get; set; }
    public IQueryable<Author> AllAuthors { get; set; }
    public readonly string FilterLanguage;
    public readonly string FilterGenre;
    public List<SelectListItem> Languages = new List<SelectListItem>();
    public List<SelectListItem> Genres = new List<SelectListItem>();

    public FreeBooksViewModel(EfBookRepository _bookRepository, EFGenreRepository _genreRepository,
        EfAuthorRepository authorRepository, IGenresFilterGetter genresFilterGetter,
        ILanguageFilterGetter languageFilterGetter)
    {
        this._bookRepository = _bookRepository;
        this._genreRepository = _genreRepository;
        AllBooks = _bookRepository.GetAllFreeBooks();
        AllAuthors = authorRepository.GetAllAuthors();
        _genresFilterGetter = genresFilterGetter;
        _languageFilterGetter = languageFilterGetter;
        Languages = _languageFilterGetter.GetItems(_bookRepository);
        Genres = _genresFilterGetter.GetItems(_genreRepository);
    }

    public FreeBooksViewModel(EfBookRepository _bookRepository, EFGenreRepository _genreRepository,
        EfAuthorRepository authorRepository, string filterLanguage, string filterGenre,
        IGenresFilterGetter genresFilterGetter, ILanguageFilterGetter languageFilterGetter)
    {
        this._bookRepository = _bookRepository;
        this._genreRepository = _genreRepository;
        _genresFilterGetter = genresFilterGetter;
        _languageFilterGetter = languageFilterGetter;
        FilterLanguage = filterLanguage;
        FilterGenre = filterGenre;
        AllBooks = _bookRepository.GetFilterFreeBooksLanguageAndGenre(FilterLanguage, FilterGenre);
        AllAuthors = authorRepository.GetAllAuthors();
        Languages = _languageFilterGetter.GetItems(_bookRepository);
        Genres = _genresFilterGetter.GetItems(_genreRepository);
    }

    public FreeBooksViewModel(EfBookRepository _bookRepository, EFGenreRepository _genreRepository,
        EfAuthorRepository authorRepository, string filterLanguage, IGenresFilterGetter genresFilterGetter,
        ILanguageFilterGetter languageFilterGetter)
    {
        this._bookRepository = _bookRepository;
        this._genreRepository = _genreRepository;
        _genresFilterGetter = genresFilterGetter;
        _languageFilterGetter = languageFilterGetter;
        FilterLanguage = filterLanguage;
        AllBooks = _bookRepository.GetFilterFreeBooksLanguage(FilterLanguage);
        AllAuthors = authorRepository.GetAllAuthors();
        Languages = _languageFilterGetter.GetItems(_bookRepository);
        Genres = _genresFilterGetter.GetItems(_genreRepository);
    }

    public FreeBooksViewModel(EfBookRepository _bookRepository, EFGenreRepository _genreRepository,
        EfAuthorRepository authorRepository, IGenresFilterGetter genresFilterGetter,
        ILanguageFilterGetter languageFilterGetter, string filterGenre)
    {
        this._bookRepository = _bookRepository;
        this._genreRepository = _genreRepository;
        _genresFilterGetter = genresFilterGetter;
        _languageFilterGetter = languageFilterGetter;
        FilterGenre = filterGenre;
        AllBooks = _bookRepository.GetFilterFreeBooksGenre(FilterGenre);
        AllAuthors = authorRepository.GetAllAuthors();
        Languages = _languageFilterGetter.GetItems(_bookRepository);
        Genres = _genresFilterGetter.GetItems(_genreRepository);
    }
}