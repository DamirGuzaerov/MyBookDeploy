using System.Linq;
using MyBook.Entities;

namespace Repositories;

public interface IBookRepository
{
    public IQueryable<Book> GetAllBooks();
    public IQueryable<Book> GetFreeBooks();
    public Book? GetBookWithImgLinks(int bookId);
    public Book? GetFullBook(int bookId);
}