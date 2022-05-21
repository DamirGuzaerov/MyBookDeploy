using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBook.Entities;
using MyBook.Infrastructure.Repositories;
using MyBook.Models;
using MyBook.ViewModels;
using Repositories;

namespace MyBook.Controllers;

public class SearchController : Controller
{
    private readonly IGenericRepository<Book> _bookRepository;
    private readonly IGenericRepository<Author> _authorRepository;
    private readonly EFUserRepository _userRepository;
    private SearchViewModel _vIewModel;

    public SearchController(IGenericRepository<Book> bookRepository,
        IGenericRepository<Author> authorRepository,
        EFUserRepository userRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _userRepository = userRepository;
    }
    public async Task<IActionResult> SearchAll(string searchString)
    {
        if (!String.IsNullOrEmpty(searchString))
        {
            _vIewModel = new SearchViewModel(_bookRepository,_authorRepository,searchString);
        }
        else
        {
            _vIewModel = new SearchViewModel(_bookRepository, _authorRepository, "");
        }

        return View(_vIewModel);
    }

    [HttpGet]
    [Route("[controller]/[action]")]
    public IActionResult SearchBooks(int page,string searchString)
    {
        if (page == 0) page = 1;
        if (!String.IsNullOrEmpty(searchString))
        {
            _vIewModel = new SearchViewModel(_bookRepository,searchString,page);
        }
        else
        {
            _vIewModel = new SearchViewModel(_bookRepository,"",page);
        }
        
        return View(_vIewModel);
    }
    
    [HttpGet]
    [Route("[controller]/[action]")]
    public IActionResult SearchAuthors(int page,string searchString)
    {
        if (page == 0) page = 1;
        if (!String.IsNullOrEmpty(searchString))
        {
            _vIewModel = new SearchViewModel(_authorRepository,searchString,page);
        }
        else
        {
            _vIewModel = new SearchViewModel(_authorRepository, "",page);
        }
        
        return View(_vIewModel);
    }

    public PartialViewResult SearchEditBooks(int page, string searchString)
    {
        if (page == 0) page = 1;
        _vIewModel = !String.IsNullOrEmpty(searchString) ? 
            new SearchViewModel(_bookRepository,searchString,page) : 
            new SearchViewModel(_bookRepository,"",page);
        return PartialView("../Partials/_EditBooksList", _vIewModel.Books.ToList());
    }
    
    public PartialViewResult SearchEditAuthors(int page, string searchString)
    {
        if (page == 0) page = 1;
        _vIewModel = !String.IsNullOrEmpty(searchString) ? 
            new SearchViewModel(_authorRepository,searchString,page) : 
            new SearchViewModel(_authorRepository,"",page);
        return PartialView("../Partials/_EditAuthorsList", _vIewModel.Authors.ToList());
    }
    public PartialViewResult SearchEditUsers(int page, string searchString)
    {
        if (page == 0) page = 1;
        _vIewModel = !String.IsNullOrEmpty(searchString) ? 
            new SearchViewModel(_userRepository,searchString,page) : 
            new SearchViewModel(_userRepository,"",page);
        return PartialView("../Partials/_EditUsersList", _vIewModel.Users.ToList());
    }
}