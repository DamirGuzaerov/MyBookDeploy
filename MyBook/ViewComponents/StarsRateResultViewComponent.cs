using Microsoft.AspNetCore.Mvc;

namespace MyBook.ViewComponents;

public class StarsRateResultViewComponent :ViewComponent
{
        public async Task<IViewComponentResult> InvokeAsync(int rate) //пока что заглушка
        {
            return View(rate);
        }
}