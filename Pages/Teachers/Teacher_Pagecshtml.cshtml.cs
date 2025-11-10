using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpSeer.Interfaces;
using SharpSeer.Models;

namespace SharpSeer.Pages.Teachers
{
    public class Teacher_PagecshtmlModel : PageModel
    {
        public IEnumerable<Teacher> Teachers { get; set; }
        private IService<Teacher> m_service;

        public bool ShowCreate { get; set; } = false;
        public Teacher_PagecshtmlModel(IService<Teacher> service)
        {
            m_service = service;
        }

        public void OnGet()
        {
            Teachers = m_service.GetAll();
            if (HttpContext.Request.Query.TryGetValue("Action", out var actionValue))
            {
                ShowCreate = true;
            }
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("Teacher_Pagecshtml");
        }

    }
}
