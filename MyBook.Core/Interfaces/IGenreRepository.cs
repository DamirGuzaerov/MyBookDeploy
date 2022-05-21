using MyBook.Entities;

namespace MyBook.Infrastructure.Repositories;

public interface IGenreRepository
{
    public IQueryable<Genre> GetAllGenres();
    
}