using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBook.Infrastructure.Repositories;
using Repositories;

namespace MyBook.Infrastructure.Helpers;

public interface ILanguageFilterGetter
{
    List<SelectListItem> GetItems(EfBookRepository repository);
}