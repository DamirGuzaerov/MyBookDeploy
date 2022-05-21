using MyBook.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Core.Interfaces
{
    public interface IRecommendationsService
    {
        public Task<List<Book>> GetRecommendationsAsync(string userId,int page);
    }
}
