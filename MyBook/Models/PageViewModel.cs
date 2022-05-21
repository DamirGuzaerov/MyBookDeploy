using Microsoft.AspNetCore.Mvc;

namespace MyBook.Models;

public class PageViewModel
{
    public int PageNumber { get;  set; }
    public int TotalPages { get; private set; }
 
    public PageViewModel(int count, int pageNumber, int pageSize = 10)
    {
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    }
 
    public bool HasNextPage
    {
        get
        {
            return (PageNumber < TotalPages);
        }
    }
}