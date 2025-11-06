using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpSeer.Interfaces;
using SharpSeer.Models;

namespace SharpSeer.Pages.Cohorts
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Cohort Cohort { get; set; }
        private IService<Cohort> m_service;
        public CreateModel(IService<Cohort> service)
        {
            m_service = service;
            Cohort = new Cohort();
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
            m_service.Create(Cohort);
            return RedirectToPage("Cohort_Page");
        }
    }
}
