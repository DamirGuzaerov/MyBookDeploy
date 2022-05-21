using Microsoft.EntityFrameworkCore;
using MyBook.Entities;
using MyBook.Infrastructure.Repositories;
using MyBook.Models;
using Repositories;

namespace MyBook.ViewModels;

public class SearchViewModel
{
    public PageViewModel PageViewModel { get; set; }
    private readonly IGenericRepository<Book>? _bookRepository;
    private readonly IGenericRepository<Author>? _authorRepository;
    private readonly EFUserRepository _userRepository;
    public readonly string SearchString;
    public IQueryable<Book> Books { get; set; }
    public IQueryable<Author> Authors { get; set; }
    public IQueryable<User> Users { get; set; }

    public SearchViewModel(
        IGenericRepository<Book> bookRepository,
        IGenericRepository<Author> authorRepository,
        string searchString)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        SearchString = searchString;
        
        Books = GetSearchBooks()!;
        Authors = GetSearchAuthors()!;

    }
    
    public SearchViewModel(
        IGenericRepository<Author> authorRepository,
        string searchString,
        int pageNumber)
    {
        _authorRepository = authorRepository;
        SearchString = searchString;

        Authors = GetSearchAuthors()!.Take(pageNumber*10);
        PageViewModel = new PageViewModel(Authors.Count(), pageNumber);
    }
    public SearchViewModel(
        EFUserRepository userRepository,
        string searchString,
        int pageNumber)
    {
        _userRepository = userRepository;
        SearchString = searchString;

        Users = GetSearchUsers()!.Take(pageNumber*10);
        PageViewModel = new PageViewModel(Users.Count(), pageNumber);
    }
    
    public SearchViewModel(
        IGenericRepository<Book> bookRepository,
        string searchString,
        int pageNumber)
    {
        _bookRepository = bookRepository;
        SearchString = searchString;
        
        Books = GetSearchBooks()!.Take(pageNumber*10);
        PageViewModel = new PageViewModel(Books.Count(), pageNumber);
    }
    
    private IQueryable<Book>? GetSearchBooks()
    {
        return _bookRepository?.GetWithMultiIncluding(
            book => book,
            book => book.Name.Contains(SearchString),
            books => 
                books.Include(book => book.AuthorBooks)
                    .ThenInclude(authorBook =>  authorBook.Author)
                    .Include(book => book.ImgLinks)
                    .Include(book=>book.Description)
        );
    }
    
    private IQueryable<Author>? GetSearchAuthors()
    {
        return _authorRepository?.GetWithMultiIncluding(
            author => author,
            author => author.Name.Contains(SearchString),
            authors => 
                authors.Include(author => author.AuthorBooks)
                    .ThenInclude(authorBook =>  authorBook.Book)
                    .Include(author => author.ImgLinks)
        );
    }
    
    private IQueryable<User>? GetSearchUsers()
    {
        return _userRepository?.GetUsersWithName(SearchString);
    }
}