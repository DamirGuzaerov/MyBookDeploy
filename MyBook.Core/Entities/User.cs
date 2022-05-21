using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBook.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string BirthDate { get; set; }
        public virtual ICollection<History> Histories { get; set; }= new List<History>();
        public virtual ICollection<Rating> Ratings { get; set; }=new List<Rating>();
        public virtual ICollection<UserSubscr> UserSubscrs { get; set; } = new List<UserSubscr>();
    }
}