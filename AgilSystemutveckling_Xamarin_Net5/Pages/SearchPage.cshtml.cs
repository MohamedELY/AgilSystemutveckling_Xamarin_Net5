using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AgilSystemutveckling_Xamarin_Net5.Pages
{
    public class SearchPageModel : PageModel
    {
        public static List<TestModels.Product> BookName;

        public void OnGet()
        {
            BookName = TestService.GetService.Get.GetAllProducts();
        }

        public void OnPost()
        {
            ViewData["Books"] = BookName;
            // git test
        }
    }


}
