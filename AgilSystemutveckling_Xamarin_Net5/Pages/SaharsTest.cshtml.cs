using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AgilSystemutveckling_Xamarin_Net5.Models;
using AgilSystemutveckling_Xamarin_Net5.Pages.UpdateService;

namespace AgilSystemutveckling_Xamarin_Net5.Pages
{
    public class SaharsTestModel : PageModel
    {
        public void OnGet()
        {
          
            Update.UpdateUnitsInStock(1,25);
        }

    }
}
