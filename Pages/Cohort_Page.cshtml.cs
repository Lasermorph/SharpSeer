using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpSeer.Models;
using SharpSeer.Services;

namespace SharpSeer.Pages
{
    public class Cohort_PageModel : PageModel
    {
        public IEnumerable<Cohort> Cohorts { get; set; }

        private CohortService m_service;

        public Cohort_PageModel(CohortService service)
        {
            m_service = service; 
        }
        public void OnGet()
        {
            Cohorts = m_service.GetAll();
        }
    }
}
