using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpSeer.Interfaces;
using SharpSeer.Models;
using SharpSeer.Services;
using Microsoft.AspNetCore.Http.Extensions;

namespace SharpSeer.Pages
{
    public class Cohort_PageModel : PageModel
    {
        public IEnumerable<Cohort> Cohorts { get; set; }

        private IService<Cohort> m_service;

        public Cohort_PageModel(IService<Cohort> service)
        {
            m_service = service;
        }
        public void OnGet()
        {
            Cohorts = m_service.GetAll();
            if (HttpContext.Request.Query.TryGetValue("Action", out var actionValue))
            {
                Console.WriteLine("It works :)");
            }        
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("Cohort_Page");
        }
    }
}