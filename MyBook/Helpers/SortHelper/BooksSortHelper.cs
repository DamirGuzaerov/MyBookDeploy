using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyBook.Helpers.SortHelper;

public static class BooksSortHelper
{
    public static List<SelectListItem> Orders { get; set; } = new List<SelectListItem>()
    {
        new SelectListItem()
        {
            Text = "По названию",
            Value = "name",
            Selected = true,
        },
        new SelectListItem()
        {
            Text = "По дате выпуска",
            Value = "date",
        }
    };
}