using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpSeer.Interfaces;
using SharpSeer.Models;

namespace SharpSeer.Pages.Cohorts
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public Cohort Cohort { get; set; }
        private IService<Cohort> m_service;
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public DeleteModel(IService<Cohort> service)
        {
            m_service = service;
        }
        public void OnGet(int id)
        {
            Cohort = m_service.GetById(id);
        }
        public IActionResult OnPost()
        {
            m_service.Delete(Cohort);
            return RedirectToPage("Cohort_Page");
        }
    }
}
