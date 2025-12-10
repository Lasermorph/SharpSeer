using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Storage;
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
        public bool IsTeacher { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Name { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Major { get; set; }
        [BindProperty(SupportsGet = true)]
        public int Term { get; set; } = 0;

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
            if (Request.Cookies.ContainsKey("IsTeacher"))
            {
                var cookie = Request.Cookies["IsTeacher"];
                if (!string.IsNullOrEmpty(cookie))
                {
                    IsTeacher = cookie == "true";
                }
            }

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
                    case "Name":
                        if (!string.IsNullOrEmpty(Cohort?.Name))
                        {
                            Cohorts = Cohorts.Where(c => c.Name.Contains(Cohort.Name, StringComparison.OrdinalIgnoreCase));
                        }
                        break;
                    case "Major":
                        if (!string.IsNullOrEmpty(Cohort?.Major))
                        {
                            Cohorts = Cohorts.Where(c => c.Major.Contains(Cohort.Major, StringComparison.OrdinalIgnoreCase));
                        }
                        break;
                    case "Term":
                        if (Cohort?.Term != 0)
                        {
                            Cohorts = Cohorts.Where(c => c.Term == Cohort.Term);
                        }
                        break;
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

        public IActionResult OnPostDelete(int id)
        {
            Cohort.Id = id;
            m_service.Delete(Cohort);
            return RedirectToPage("Cohort_Page");
        }
        public IActionResult OnPostUpdate(int id)
        {
            Cohort.Id = id;
            m_service.Update(Cohort);
            return RedirectToPage("Cohort_Page");
        }
        public IActionResult OnPostCreate()
        {
            m_service.Create(Cohort);
            return RedirectToPage("Cohort_Page");
        }
    }
}