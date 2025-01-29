using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SampleAuthentication.Pages
{
    [Authorize(Policy = "PageII")]
    public class PageIIModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
