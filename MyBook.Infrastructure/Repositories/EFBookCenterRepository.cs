using MyBook.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Infrastructure.Repositories
{
    public class EFBookCenterRepository : EfGenericRepository<BookCenter>
    {
        public EFBookCenterRepository(MyBookContext context) : base(context)
        {

        }
    }
}
