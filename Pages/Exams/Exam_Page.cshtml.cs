using Microsoft.AspNetCore.Mvc;
using SharpSeer.Models;
using SharpSeer.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.CompilerServices;
using static SharpSeer.Models.Exam;
using static SharpSeer.Services.Extensions;
using Microsoft.VisualBasic;


namespace SharpSeer.Pages.Exams
{
    public class Exam_PageModel : PageModel
    {
        public bool ShowDelete { get; set; } = false;
        public bool ShowCreate { get; set; } = false;
        public bool ShowUpdate { get; set; } = false;
        public bool AreYouAnIdiot { get; set;} = false;
        public bool SetOverflow { get; set; } = false;
        public bool ShowCalendar { get; set; } = false;
        public string ExamComment { get; set; } = string.Empty;
        public List<Exam> TestExams { get; set; } = new List<Exam>();
        
        [BindProperty(SupportsGet = true)]
        public bool? IsTeacher { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Name { get; set; } = null!;

        [BindProperty(SupportsGet = true)]
        public string NameId { get; set; } = null!;

        [BindProperty(SupportsGet =true)]
        public string CohortName { get; set; } = null!;

        [BindProperty(SupportsGet = true)]
        public ExamTypeEnum? ExamType { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool? IsGuarded { get; set; } = null; 
        [BindProperty(SupportsGet = true)]
        public bool? NeedExternalExaminer { get; set; } = null;
        [BindProperty(SupportsGet = true)]
        public DateTime FirstExamDate { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime LastExamDate { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime? HandInDeadline { get; set; }
        [BindProperty(SupportsGet = true)]
        public int DurationInMinutes { get; set; }
        public int EstimatedStudentCount { get; set; }  
        [BindProperty(SupportsGet = true)]
        public string QueryString { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public Exam? Exam { get; set; }

        [BindProperty(SupportsGet = true)]
        public ICollection<int> Cohorts { get; set; }
        [BindProperty(SupportsGet = true)]
        public ICollection<int> Teachers { get; set; }
        private IService<Exam> m_examService;
        private IService<Cohort> m_cohortService;
        private IService<Teacher> m_teacherService;
        public IEnumerable<Exam> Exams { get; set; }
        public IEnumerable<Cohort> CohortsAll { get; set; }
        public IEnumerable<Teacher> TeachersAll { get; set; }
        public Exam_PageModel(IService<Exam> examService, IService<Cohort> cohortService, IService<Teacher> teacherService)
        {
            m_examService = examService;
            m_cohortService = cohortService;
            m_teacherService = teacherService;
            Exams = m_examService.GetAll();
            CohortsAll = m_cohortService.GetAll();
            TeachersAll = m_teacherService.GetAll();
            Exam = new Exam();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void GetQueryValues(in string q) 
        {
            HttpContext.Request.Query.TryGetValue(q, out var value);
            Exam = m_examService.GetById(int.Parse(value));
        }
        public void GetTime(DateTime start, DateTime end)
        {
            foreach(Exam e in Exams)
            {
                if (e.FirstExamDate >= start || e.FirstExamDate <= e.LastExamDate || e.LastExamDate <= end)
                {
                    TestExams.Add(e);
                }
            }
            Exams = TestExams;
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
                        SetOverflow = true;
                        GetQueryValues(q);
                        goto EndOfLoop;
                    case "Create":
                        ShowCreate = true;
                        SetOverflow = true;
                        HttpContext.Request.Query.TryGetValue(q, out var value);
                        QueryString = q;
                        goto EndOfLoop;
                    case "Update":
                        ShowUpdate = true;
                        SetOverflow = true;
                        GetQueryValues(q);
                        goto EndOfLoop;
                    case "Name":
                        if (!string.IsNullOrEmpty(Name))
                        {
                            Exams = Exams.Where(t => t.Name.Contains(Name, StringComparison.OrdinalIgnoreCase));
                        }
                        break;
                    case "NameId":
                        if (!string.IsNullOrEmpty(NameId))
                        {
                            Exams = Exams.Where(t => t.Teachers.Any(teacher => teacher.NameId.Contains(NameId, StringComparison.OrdinalIgnoreCase)));
                        }
                        break;
                    case "CohortName":
                        if (!string.IsNullOrEmpty(CohortName))
                        {
                            Exams = Exams.Where(t => t.Cohorts.Any(cohort => cohort.Name.Contains(CohortName, StringComparison.OrdinalIgnoreCase)));
                        }
                        break;
                    case "ExamType":
                        if (ExamType.HasValue)
                            Exams = Exams.Where(t => t.ExamType == (int)ExamType.Value);
                        break;
                    case "IsGuarded":
                        if (IsGuarded.HasValue)
                        {
                            Exams = Exams.Where(t => t.IsGuarded == IsGuarded);
                        }
                       break;
                    case "NeedExternalExaminer":
                        if (NeedExternalExaminer.HasValue)
                        {
                            Exams = Exams.Where(t => t.NeedExternalExaminer == NeedExternalExaminer);
                        }
                        break;
                    case "FirstExamDate":
                        if (FirstExamDate > LastExamDate)
                        {
                            AreYouAnIdiot = true;                            
                        }
                        else
                        {
                            if (FirstExamDate != DateTime.MinValue)
                            {
                                Exams = Exams.Where(t => t.FirstExamDate.Date <= LastExamDate.Date && t.LastExamDate.Date >= FirstExamDate.Date);
                            }
                        }
                        break;
                    case "LastExamDate":
                        if (LastExamDate != DateTime.MinValue)
                        {
                        }
                        break;
                    case "HandInDeadline":
                        if (HandInDeadline != null)
                        {
                            Exams = Exams.Where(t => t.HandInDeadline != null && t.HandInDeadline.Value.Date == HandInDeadline.Value.Date);
                        }
                        break;
                    case "DurationInMinutes":
                        if (DurationInMinutes != 0)
                        {
                            Exams = Exams.Where(t => t.DurationInMinutes == DurationInMinutes);
                        }
                        break;
                    case "EstimatedStudentCount":
                        if (EstimatedStudentCount != 0)
                        {
                            Exams = Exams.Where(t => t.EstimatedStudentCount != null && t.EstimatedStudentCount == EstimatedStudentCount);
                        }
                        break;  
                }
            }
            EndOfLoop:;

            // If updating, populate bound id lists so checkboxes render checked
            if (ShowUpdate && Exam != null)
            {
                Cohorts = Exam.Cohorts?.Select(c => c.Id).ToList() ?? new List<int>();
                Teachers = Exam.Teachers?.Select(t => t.Id).ToList() ?? new List<int>();
            }

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
                        m_examService.Delete(Exam);
                        goto EndOfLoop;
                    case "Create":
                        HttpContext.Request.Query.TryGetValue(q, out var value);
                        m_examService.Create(Exam);
                        isDone = true;
                        break;
                    case "Update":
                        GetQueryValues(q);
                        m_examService.Update(Exam);
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

        public IActionResult OnPostDelete(int id)
        {
            Exam.Id = id;
            foreach (var cohortId in Cohorts)
            {
                var cohort = m_cohortService.GetById(cohortId);
                Exam.Cohorts.Add(cohort);
            }
            foreach (var teacherId in Teachers)
            {
                var teacher = m_teacherService.GetById(teacherId);
                Exam.Teachers.Add(teacher);
            }
            m_examService.Delete(Exam);
            return RedirectToPage("Exam_Page");
        }

        public IActionResult OnPostUpdate(int id)
        {
            Exam.Id = id;
            Exam.ExamType = (int)ExamType.Value;

            if (IsGuarded.HasValue)
            {
                Exam.IsGuarded = IsGuarded.Value;
            }
            if (NeedExternalExaminer.HasValue)
            {
                Exam.NeedExternalExaminer = NeedExternalExaminer.Value;
            }

            foreach (var cohortId in Cohorts)
            {
                var cohort = m_cohortService.GetById(cohortId);
                Exam.Cohorts.Add(cohort);
            }
            foreach (var teacherId in Teachers)
            {
                var teacher = m_teacherService.GetById(teacherId);
                Exam.Teachers.Add(teacher);
            }
            m_examService.Update(Exam);
            return RedirectToPage("Exam_Page");
        }

        public IActionResult OnPostCreate()
        {

            CohortsAll = m_cohortService.GetAll();
            TeachersAll = m_teacherService.GetAll();

            if (Cohorts == null || Cohorts.Count == 0)
            {
                ModelState.AddModelError(nameof(Cohorts), "Vælg mindst ét hold.");
                ShowCreate = true;
                return Page();
            }
            if (Teachers == null || Teachers.Count == 0)
            {
                ModelState.AddModelError(nameof(Teachers), "Vælg mindst én underviser.");
                ShowCreate = true;
                return Page();
            }
            Exam.ExamType = (int)ExamType;
            foreach (var cohortId in Cohorts)
            {
                var cohort = m_cohortService.GetById(cohortId);
                Exam.Cohorts.Add(cohort);
            }
            foreach (var teacherId in Teachers)
            {
                var teacher = m_teacherService.GetById(teacherId);
                Exam.Teachers.Add(teacher);
            }

            m_examService.Create(Exam);
            return RedirectToPage("Exam_Page");
        }
        public IActionResult OnPostTeacher()
        {
            return RedirectToPage("Exam_Page", new { IsTeacher = IsTeacher });
        }
    }
}
