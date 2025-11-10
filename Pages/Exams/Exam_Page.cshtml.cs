using Microsoft.AspNetCore.Mvc;
using SharpSeer.Models;
using SharpSeer.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;

namespace SharpSeer.Pages.Exams
{
    public class Exam_PageModel : PageModel
    {
        public bool ShowDelete { get; set; } = false;
        public bool ShowCreate { get; set; } = false;
        public bool ShowUpdate { get; set; } = false;
        [BindProperty(SupportsGet = true)]
        public string QueryString { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public Exam? Exam { get; set; }
        private IService<Exam> m_service;
        public IEnumerable<Exam> Exams { get; set; }
        public Exam_PageModel(IService<Exam> service)
        {
            m_service = service;
            Exams = m_service.GetAll();
            Exam = new Exam();
        
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void GetQueryValues(in string q) 
        {
            HttpContext.Request.Query.TryGetValue(q, out var value);
            Exam = m_service.GetById(int.Parse(value));
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

            //if (HttpContext.Request.Query.TryGetValue("Delete", out var actionValue))
            //{
            //    ShowDelete = true;
            //    Exam = m_service.GetById(int.Parse(actionValue));
            //}
            //else if (HttpContext.Request.Query.TryGetValue("Create", out var createValue))
            //{
            //    ShowUpdate = true;
            //    Exam = m_service.GetById(int.Parse(createValue));
            //}
            //else if (HttpContext.Request.Query.TryGetValue("Update", out var updateValue))
            //{
            //    ShowCreate = true;
            //}

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
                        m_service.Delete(Exam);
                        goto EndOfLoop;
                    case "Create":
                        HttpContext.Request.Query.TryGetValue(q, out var value);
                        m_service.Create(Exam);
                        isDone = true;
                        break;
                    case "Update":
                        GetQueryValues(q);
                        m_service.Update(Exam);
                        isDone = true;
                        break;
                }
                if (isDone)
                {
                    break;
                }
            }
        EndOfLoop:
            //HttpContext.Request.Query.TryGetValue("Delete", out var actionValue);
            //m_service.Delete(m_service.GetById(int.Parse(actionValue)));
            return RedirectToPage("Exam_Page");

        }
        
        public IActionResult OnPostUpdate(int id)
        {
            Exam.Id = id;
            m_service.Update(Exam);
            return RedirectToPage("Exam_Page");
        }

        public IActionResult OnPostCreate()
        {
            m_service.Create(Exam);
            return RedirectToPage("Exam_Page");
        }
    }
}
