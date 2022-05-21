using MyBook.Entities;
using Repositories;

namespace MyBook.Infrastructure.Repositories;

public interface IAuthorRepository: IGenericRepository<Author>
{
    public IQueryable<Author> GetAllAuthors();
    public Author GetFullAuthor(int authorId);
}