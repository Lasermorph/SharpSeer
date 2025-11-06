using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpSeer.Interfaces;
using SharpSeer.Models;

namespace SharpSeer.Pages.Cohorts
{
    public class UpdateModel : PageModel
    {
        [BindProperty]
        public Cohort Cohort { get; set; }
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        private IService<Cohort> m_service;
        public UpdateModel(IService<Cohort> service)
        {
            m_service = service;
        }
        public void OnGet(int id)
        {
            Id = id;
        }


        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            m_service.Update(Cohort, Id);
            return RedirectToPage("Cohort_Page");
        }
    }
}
