using Microsoft.AspNetCore.Mvc.Rendering;
using MyBook.Entities;
using MyBook.Infrastructure.Repositories;
using Repositories;

namespace MyBook.Infrastructure.Helpers;

public class LanguageFilterGetter: ILanguageFilterGetter
{
    public List<SelectListItem> GetItems(EfBookRepository bookRepository)
    {
        List<SelectListItem> languagesSelectListItems = new List<SelectListItem>();
        List<Book> allBooks = bookRepository.GetAllBooks().ToList();
        languagesSelectListItems.Add(new SelectListItem() {Text = "Все", Value = "Все"});
        List<string> languages = new List<string>();
        foreach (var book in allBooks)
        {
            if (!languages.Contains(book.Language))
            {
                languages.Add(book.Language);
            }
        }

        foreach (var lang in languages)
        {
            var display = "";
            switch (lang)
            {
                case "ru":
                    display = "русский";
                    break;
                case "en":
                    display = "английский";
                    break;
                case "de":
                    display = "немецкий";
                    break;
                case "it":
                    display = "итальянский";
                    break;
                default:
                    display = lang;
                    break;
            }

            var item = new SelectListItem() {Text = $"{display}", Value = $"{lang}"};
            languagesSelectListItems.Add(item);
        }

        return languagesSelectListItems;
    }
}