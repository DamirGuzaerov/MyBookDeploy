using MyBook.Entities;
using System.ComponentModel.DataAnnotations;

namespace MyBook.Models
{
    public class PayViewModel
    {
        #region Subscr
        const string date_regex = @"^((0[1-9])|(1[0-2]))\/([2-9][3-9])|((0[6-9])|(1[0-2]))\/22$";
        const string name_regex = @"^((?:[A-Za-z]+ ?){1,3})$";
        const string cvc_regex = @"^[0-9]{3}$";
        public bool isGift { get; set; } = false;
        public string? UserId { get; set; }
        public int? Period { get; set; }
        public int? Price { get; set; }
        public int? TypeId { get; set; }
        public string? TypeName { get; set; }

        public string? SpecsName { get; set; }
        public int? SpecsId { get; set; }
        #endregion

        #region Card

        [CreditCard(ErrorMessage ="Введите корректный номер карты")]
        [DataType(DataType.CreditCard,ErrorMessage ="Введите корректный номер карты")]
        public string? CardNum { get; set; }
        [RegularExpression(date_regex, ErrorMessage = "Введите корректную дату для карты, которая не является истекшей")]
        public string? CardDate { get; set; }
        [RegularExpression(name_regex, ErrorMessage = "Введите корректное имя владельца карты")]
        public string? CardName { get; set; }
        [RegularExpression(cvc_regex, ErrorMessage = "Введите корректный код")]
        public string? CardCode { get; set; }
        #endregion

        #region Book
        public string? BookPrice { get; set; }
        public string? BookName { get; set; }
        public int? BookId { get; set; } = null;
        #endregion
    }
}
