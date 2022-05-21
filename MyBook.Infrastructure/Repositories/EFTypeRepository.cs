using MyBook.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Infrastructure.Repositories
{
    public class EFTypeRepository : EfGenericRepository<MyBook.Entities.Type>
    {
        public EFTypeRepository(MyBookContext context) : base(context)
        { 

        }
    }
}
