using MyBook.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyBook.Parser
{
    public static class Parcer
    {
        public static List<BookJson> ParceJSONBooks()
        {
            var result = new List<BookJson>();

            foreach (var path in Directory.GetFiles(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent + "/files"))
            {
                var root = new Root();
                using (StreamReader reader = new StreamReader(path))
                {
                    var json = reader.ReadToEnd();
                    root = JsonSerializer.Deserialize<Root>(json);
                }
                foreach (var item in root.items)
                {
                    var imgLinkSm = new ImgLink { Resolution = "smallThumbnail", Url = item.volumeInfo.imageLinks != null && item.volumeInfo.imageLinks.smallThumbnail != null ? item.volumeInfo.imageLinks.smallThumbnail : "" };
                    var imgLink = new ImgLink { Resolution = "thumbnail", Url = item.volumeInfo.imageLinks != null && item.volumeInfo.imageLinks.thumbnail != null ? item.volumeInfo.imageLinks.thumbnail : "" };
                    //var epub = new DownloadLink { Format = "epub", Url = item.accessInfo.epub.acsTokenLink != null ? item.accessInfo.epub.acsTokenLink : "" };
                    //var pdf = new DownloadLink { Format = "pdf", Url = item.accessInfo.pdf.acsTokenLink != null ? item.accessInfo.pdf.acsTokenLink : "" };
                    var epub = new DownloadLink { Format = "epub", Url = @"https://drive.google.com/uc?export=download&id=1Bsr_A-6yrTsdYYYFAnuvurCmhYTRfEH6" };
                    var pdf = new DownloadLink { Format = "pdf", Url = @"https://drive.google.com/uc?export=download&id=1tMtHBHvnJof0gJd_H3UCBrLseIDWtntB" };
                    DateOnly date;
                    DateOnly.TryParse(item.volumeInfo.publishedDate, out date);
                    var book = new BookJson
                    {
                        Name = item.volumeInfo.title,
                        Language = item.volumeInfo.language,
                        PublishedDate = date,
                        IsForAdult = item.volumeInfo.maturityRating == "MATURE",
                        IsPaid = !item.volumeInfo.allowAnonLogging,
                        Description = item.volumeInfo.description,
                        Price = item.saleInfo.listPrice?.amount + " " + item.saleInfo.listPrice?.currencyCode,
                        PagesCount = item.volumeInfo.pageCount,
                        Genres = item.volumeInfo.categories?.Select(catName => new Genre { Name = catName != null ? catName : "" }).ToList(),
                        Authors = item.volumeInfo.authors?.Select(authName => new Author { Name = authName != null ? authName : "" }).ToList(),
                        Images = new List<ImgLink> { imgLink, imgLinkSm },
                        DownloadLinks = new List<DownloadLink> { epub, pdf }
                    };
                    result.Add(book);
                }
            }
            return result;
        }
    }
}
