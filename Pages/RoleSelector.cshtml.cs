using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SharpSeer.Pages
{
    public class RoleSelectorModel : PageModel
    {
        [BindProperty]
        public bool? IsTeacher { get; set; }

        public IActionResult OnPost()
        {
            if (IsTeacher.HasValue)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = false,
                    Expires = DateTimeOffset.UtcNow.AddDays(30)
                };
                Response.Cookies.Append("IsTeacher", IsTeacher.Value ? "true" : "false", cookieOptions);
            }
            var referer = Request.Headers["Referer"].ToString();
            if (string.IsNullOrEmpty(referer))
            {
                return RedirectToPage("/Index");
            }
            return Redirect(referer);
        }
    }
}
