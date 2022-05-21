using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBook.Entities;
using MyBook.Infrastructure.Helpers;
using MyBook.Infrastructure.Repositories;
using Repositories;

namespace MyBook.ViewModels;

public class LibraryViewModel
{
    private readonly EfBookRepository _bookRepository;
    private readonly IGenericRepository<Genre> genreRepository;
    public IQueryable<Book> AllBooks { get; set; }
    public IQueryable<Author> AllAuthors { get; set; }

    private readonly EFGenreRepository _genreRepository;
    private readonly IGenresFilterGetter _genresFilterGetter;
    private readonly ILanguageFilterGetter _languageFilterGetter;
    public readonly string FilterLanguage;
    public readonly string FilterGenre;
    public List<SelectListItem> Languages = new List<SelectListItem>();
    public List<SelectListItem> Genres = new List<SelectListItem>();


    public LibraryViewModel(EfBookRepository _bookRepository, EFGenreRepository _genreRepository,
        EfAuthorRepository authorRepository, IGenresFilterGetter genresFilterGetter,
        ILanguageFilterGetter languageFilterGetter)
    {
        this._bookRepository = _bookRepository;
        this._genreRepository = _genreRepository;
        _genresFilterGetter = genresFilterGetter;
        _languageFilterGetter = languageFilterGetter;
        AllBooks = _bookRepository.GetAllBooks();
        AllAuthors = authorRepository.GetAllAuthors();
        Languages = _languageFilterGetter.GetItems(_bookRepository);
        Genres = _genresFilterGetter.GetItems(_genreRepository);
    }

    public LibraryViewModel(EfBookRepository _bookRepository, EFGenreRepository _genreRepository,
        EfAuthorRepository authorRepository, string filterLanguage, string filterGenre,
        IGenresFilterGetter genresFilterGetter, ILanguageFilterGetter languageFilterGetter)
    {
        this._bookRepository = _bookRepository;
        this._genreRepository = _genreRepository;
        _genresFilterGetter = genresFilterGetter;
        _languageFilterGetter = languageFilterGetter;
        FilterLanguage = filterLanguage;
        FilterGenre = filterGenre;
        AllBooks = _bookRepository.GetFilterBooksLanguageAndGenre(FilterLanguage, FilterGenre);
        AllAuthors = authorRepository.GetAllAuthors();
        Languages = _languageFilterGetter.GetItems(_bookRepository);
        Genres = _genresFilterGetter.GetItems(_genreRepository);
    }

    public LibraryViewModel(EfBookRepository _bookRepository, EFGenreRepository _genreRepository,
        EfAuthorRepository authorRepository, string filterLanguage, IGenresFilterGetter genresFilterGetter,
        ILanguageFilterGetter languageFilterGetter)
    {
        this._bookRepository = _bookRepository;
        this._genreRepository = _genreRepository;
        _genresFilterGetter = genresFilterGetter;
        _languageFilterGetter = languageFilterGetter;
        FilterLanguage = filterLanguage;
        AllBooks = _bookRepository.GetFilterBooksLanguage(FilterLanguage);
        AllAuthors = authorRepository.GetAllAuthors();
        Languages = _languageFilterGetter.GetItems(_bookRepository);
        Genres = _genresFilterGetter.GetItems(_genreRepository);
    }

    public LibraryViewModel(EfBookRepository _bookRepository, EFGenreRepository _genreRepository,
        EfAuthorRepository authorRepository, IGenresFilterGetter genresFilterGetter,
        ILanguageFilterGetter languageFilterGetter, string filterGenre)
    {
        this._bookRepository = _bookRepository;
        this._genreRepository = _genreRepository;
        _genresFilterGetter = genresFilterGetter;
        _languageFilterGetter = languageFilterGetter;
        FilterGenre = filterGenre;
        AllBooks = _bookRepository.GetFilterBooksGenre(FilterGenre);
        AllAuthors = authorRepository.GetAllAuthors();
        Languages = _languageFilterGetter.GetItems(_bookRepository);
        Genres = _genresFilterGetter.GetItems(_genreRepository);
    }
    
}