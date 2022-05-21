using Microsoft.EntityFrameworkCore;
using MyBook.Entities;
using Repositories;

namespace MyBook.Infrastructure.Repositories;

public class EFGenreRepository : EfGenericRepository<Genre>, IGenreRepository
{
    public EFGenreRepository(MyBookContext context) : base(context)
    {
    }

    public IQueryable<Genre> GetAllGenres()
    {
        return DbSet.Include(genre => genre.Name);
    }
}