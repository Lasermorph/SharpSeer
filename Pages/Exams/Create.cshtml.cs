using Microsoft.AspNetCore.Mvc;
using SharpSeer.Models;
using SharpSeer.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SharpSeer.Pages.Exams
{
    public class CreateModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public Exam Exam { get; set; }
        IService<Exam> m_service;
        public CreateModel(IService<Exam> service)
        {
            m_service = service;
            Exam = new Exam();
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            m_service.Create(Exam);
            return RedirectToPage("Exam_Page");
        }

    }
}
