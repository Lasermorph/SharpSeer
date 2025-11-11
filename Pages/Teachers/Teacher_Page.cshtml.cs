using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpSeer.Interfaces;
using SharpSeer.Models;
using System.Runtime.CompilerServices;

namespace SharpSeer.Pages.Teachers
{
    public class Teacher_PageModel : PageModel
    {

        public bool ShowDelete { get; set; } = false;
        public bool ShowUpdate { get; set; } = false;
        public bool ShowCreate { get; set; } = false;

        [BindProperty(SupportsGet =true)]
        public string QueryString { get; set; } = string.Empty;

        [BindProperty(SupportsGet =true)] 
        public Teacher? Teacher { get; set; }

        public IEnumerable<Teacher> Teachers { get; set; }
        private IService<Teacher> m_service;
        public Teacher_PageModel(IService<Teacher> service)
        {
            m_service = service;
            Teachers = m_service.GetAll();
            Teacher = new Teacher();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void GetQueryValues(in string q)
        {
            HttpContext.Request.Query.TryGetValue(q, out var value);
            Teacher = m_service.GetById(int.Parse(value));
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
        //public IActionResult OnPost()
        //{
        //    ICollection<string> QKeys = HttpContext.Request.Query.Keys;
        //    bool isDone = false;
        //    foreach (var q in QKeys)
        //    {
        //        switch (q)
        //        {
        //            case "Delete":

        //                GetQueryValues(q);
        //                m_service.Delete(Teacher);
        //                isDone = true;
        //                break;
        //            case "Create":
        //                HttpContext.Request.Query.TryGetValue(q, out var value);
        //                m_service.Create(Teacher);
        //                isDone = true;
        //                break;
        //            case "Update":
        //                GetQueryValues(q);
        //                m_service.Update(Teacher);
        //                isDone = true;
        //                break;
        //        }
        //        if (isDone)
        //        {
        //            break;
        //        }
        //    }
        //    return RedirectToPage("Teacher_Page");
        //}
        public IActionResult OnPostDelete(int id)
        {
            Teacher.Id = id;
            m_service.Delete(Teacher);
            return RedirectToPage("Teacher_Page");
        }
        public IActionResult OnPostUpdate(int id)
        {
            Teacher.Id = id;
            m_service.Update(Teacher);
            return RedirectToPage("Teacher_Page");
        }

        public IActionResult OnPostCreate()
        {
            m_service.Create(Teacher);
            return RedirectToPage("Teacher_Page");
        }

    }
}
    

