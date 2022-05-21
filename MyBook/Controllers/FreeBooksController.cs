using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBook.Entities;
using MyBook.Infrastructure.Helpers;
using MyBook.Models;
using MyBook.Infrastructure.Repositories;
using Repositories;
using MyBook.Infrastructure.Helpers;

namespace MyBook.Controllers;

public class FreeBooksController : Controller
{
    private FreeBooksViewModel _viewModel;
    private readonly EfBookRepository _bookRepository;
    private readonly EFGenreRepository _genreRepository;
    private readonly EfAuthorRepository _authorRepository;
    private readonly IGenresFilterGetter _genresFilterGetter;
    private readonly ILanguageFilterGetter _languageFilterGetter;

    public FreeBooksController(EfBookRepository _bookRepository, EFGenreRepository _genreRepository,
        EfAuthorRepository _authorRepository, IGenresFilterGetter genresFilterGetter,
        ILanguageFilterGetter languageFilterGetter)
    {
        this._bookRepository = _bookRepository;
        this._genreRepository = _genreRepository;
        this._authorRepository = _authorRepository;
        _genresFilterGetter = genresFilterGetter;
        _languageFilterGetter = languageFilterGetter;
    }

    [Route("[controller]/[action]/{pageCount:int?}")]
    public async Task<IActionResult> FreeBooks(string filterLanguage, string filterGenre, string sortOrder)
    {
        _viewModel = InitializeViewModel(filterLanguage, filterGenre);
        switch (sortOrder)
        {
            case "name":
            {
                _viewModel.AllBooks = _viewModel.AllBooks.OrderBy(book => book.Name);
                break;
            }
            case "date":
            {
                _viewModel.AllBooks = _viewModel.AllBooks.OrderBy(book => book.PublishedDate);
                break;
            }
        }
        return View(_viewModel);
    }

    public bool IsInputEmpty(string filterLanguage, string filterGenre)
    {
        return (!String.IsNullOrEmpty(filterLanguage) || filterLanguage == "Все") &&
               (!String.IsNullOrEmpty(filterGenre) || filterGenre == "Все");
    }

    public bool IsAllSelected(string filterLanguage, string filterGenre)
    {
        return (String.IsNullOrEmpty(filterGenre) || filterGenre == "Все") && (!String.IsNullOrEmpty(filterLanguage));
    }

    public bool IsGenreSelected(string filterLanguage, string filterGenre)
    {
        return (String.IsNullOrEmpty(filterLanguage) || filterLanguage == "Все") &&
               (!String.IsNullOrEmpty(filterGenre));
    }

    public bool IsLanguageSelected(string filterLanguage, string filterGenre)
    {
        return (String.IsNullOrEmpty(filterLanguage) || filterLanguage == "Все") &&
               (String.IsNullOrEmpty(filterGenre) || filterGenre == "Все");
    }

    public FreeBooksViewModel InitializeViewModel(string filterLanguage, string filterGenre)
    {
        if (IsInputEmpty(filterLanguage, filterGenre))
        {
            _viewModel = new FreeBooksViewModel(_bookRepository, _genreRepository, _authorRepository, filterLanguage,
                filterGenre, _genresFilterGetter, _languageFilterGetter);
        }

        if (IsAllSelected(filterLanguage, filterGenre))
        {
            _viewModel = new FreeBooksViewModel(_bookRepository, _genreRepository, _authorRepository, filterLanguage,
                _genresFilterGetter, _languageFilterGetter);
        }

        if (IsGenreSelected(filterLanguage, filterGenre))
        {
            _viewModel = new FreeBooksViewModel(_bookRepository, _genreRepository, _authorRepository,
                _genresFilterGetter, _languageFilterGetter, filterGenre);
        }

        if (IsLanguageSelected(filterLanguage, filterGenre))
        {
            _viewModel = new FreeBooksViewModel(_bookRepository, _genreRepository, _authorRepository,
                _genresFilterGetter, _languageFilterGetter);
        }

        return _viewModel;
    }
}