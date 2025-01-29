using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SampleAuthentication.Pages
{
    [Authorize(Policy ="PageI")]
    public class PageIModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
