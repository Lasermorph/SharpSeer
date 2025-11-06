using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpSeer.Interfaces;
using SharpSeer.Models;
using SharpSeer.Services;
using Microsoft.AspNetCore.Http.Extensions;

namespace SharpSeer.Pages.Cohorts
{
    public class Cohort_PageModel : PageModel
    {
        public IEnumerable<Cohort> Cohorts { get; set; }

        private IService<Cohort> m_service;

        public bool ShowCreate { get; set; } = false;

        public Cohort_PageModel(IService<Cohort> service)
        {
            m_service = service;
        }
        public void OnGet()
        {
            Cohorts = m_service.GetAll();
            if (HttpContext.Request.Query.TryGetValue("Action", out var actionValue))
            {
                ShowCreate = true;
            }        
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("Cohort_Page");
        }
    }
}