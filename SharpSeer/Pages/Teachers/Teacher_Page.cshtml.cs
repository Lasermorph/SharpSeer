using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SharpSeer.Interfaces;
using SharpSeer.Models;

namespace SharpSeer.Pages.Teachers
{
    public class Teacher_PageModel : PageModel
    {
        public bool ShowDelete { get; set; } = false;
        public bool ShowUpdate { get; set; } = false;
        public bool ShowCreate { get; set; } = false;
        [BindProperty(SupportsGet = true)]
        public string Email { get; set; }
        [BindProperty(SupportsGet = true)]
        public string PhoneNumber { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Name { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IsTeacher { get; set; }

        [BindProperty(SupportsGet = true)]
        public string NameId { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool? IsExternal { get; set; }

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
                        if (!string.IsNullOrEmpty(Name))
                        { 
                            Teachers = Teachers.Where(t => t.Name.Contains(Name, StringComparison.OrdinalIgnoreCase));
                        }
                        break;
                    case "Email":
                        if (!string.IsNullOrEmpty(Email))
                        {
                            Teachers = Teachers.Where(t => t.Email.Contains(Email, StringComparison.OrdinalIgnoreCase));
                        }
                       break;
                    case "PhoneNumber":
                        if (!string.IsNullOrEmpty(PhoneNumber))
                        {
                            Teachers = Teachers.Where(t => t.PhoneNumber.Contains(PhoneNumber, StringComparison.OrdinalIgnoreCase));
                        }
                        break;
                    case "NameId":
                        if (!string.IsNullOrEmpty(NameId))
                        {
                            Teachers = Teachers.Where(t => t.NameId.Contains(NameId, StringComparison.OrdinalIgnoreCase));
                        }
                        break;
                    case "IsExternal":
                        if (IsExternal.HasValue)
                        {
                            Teachers = Teachers.Where(t => t.IsExternal == IsExternal.Value);
                        }
                        break;
                }
            }
            EndOfLoop:;
        }
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
    

