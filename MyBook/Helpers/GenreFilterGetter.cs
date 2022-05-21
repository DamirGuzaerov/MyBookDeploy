using Microsoft.AspNetCore.Mvc.Rendering;
using MyBook.Entities;
using MyBook.Infrastructure.Repositories;
using Repositories;

namespace MyBook.Infrastructure.Helpers;

public class GenreFilterGetter: IGenresFilterGetter
{
    public List<SelectListItem> GetItems(EFGenreRepository genreRepository)
    {
        List<SelectListItem> genres = new List<SelectListItem>();
        List<Genre> allGenres = genreRepository.Get().ToList();
        genres.Add(new SelectListItem() {Text = "Все", Value = "Все"});
        foreach (var genre in allGenres)
        {
            var genreName = genre.Name;
            var item = new SelectListItem() {Text = $"{genreName}", Value = $"{genreName}"};
            genres.Add(item);
        }

        return genres;
    }
}