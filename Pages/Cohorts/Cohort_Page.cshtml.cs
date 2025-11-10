using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpSeer.Interfaces;
using SharpSeer.Models;
using System.Runtime.CompilerServices;

namespace SharpSeer.Pages.Cohorts
{
    public class Cohort_PageModel : PageModel
    {
        public bool ShowDelete { get; set; } = false;
        public bool ShowCreate { get; set; } = false;
        public bool ShowUpdate { get; set; } = false;
        [BindProperty(SupportsGet = true)]
        public string QueryString { get; set; } = string.Empty;
        [BindProperty(SupportsGet = true)]
        public Cohort? Cohort { get; set; }
        public IEnumerable<Cohort> Cohorts { get; set; }
        private IService<Cohort> m_service;
        public Cohort_PageModel(IService<Cohort> service)
        {
            m_service = service;
            Cohorts = m_service.GetAll();
            Cohort = new Cohort();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void GetQueryValues(in string q)
        {
            HttpContext.Request.Query.TryGetValue(q, out var value);
            Cohort = m_service.GetById(int.Parse(value));
        }
        public void OnGet()
        {
            ICollection<string> QKeys = HttpContext.Request.Query.Keys;
            foreach (var q in QKeys)
            {
                switch (q)
                {
                    case "Delete":
                        ShowDelete = true;
                        GetQueryValues(q);
                        goto EndOfLoop;
                    case "Create":
                        ShowCreate = true;
                        HttpContext.Request.Query.TryGetValue(q, out var value);
                        QueryString = q;
                        goto EndOfLoop;
                    case "Update":
                        ShowUpdate = true;
                        GetQueryValues(q);
                        goto EndOfLoop;
                }
            }
        EndOfLoop:;
        }

        public IActionResult OnPost()
        {
            ICollection<string> QKeys = HttpContext.Request.Query.Keys;
            bool isDone = false;
            foreach (var q in QKeys)
            {
                switch (q)
                {
                    case "Delete":

                        GetQueryValues(q);
                        m_service.Delete(Cohort);
                        isDone = true;
                        break;
                    case "Create":
                        HttpContext.Request.Query.TryGetValue(q, out var value);
                        m_service.Create(Cohort);
                        isDone = true;
                        break;
                    case "Update":
                        GetQueryValues(q);
                        m_service.Update(Cohort);
                        isDone = true;
                        break;
                }
                if (isDone)
                {
                    break;
                }
            }
            return RedirectToPage("Cohort_Page");
        }
    }
}