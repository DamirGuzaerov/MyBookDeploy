using Microsoft.AspNetCore.Mvc.Rendering;
using MyBook.Infrastructure.Repositories;

namespace MyBook.Infrastructure.Helpers;

public interface IGenresFilterGetter
{
    List<SelectListItem> GetItems(EFGenreRepository repository);
}