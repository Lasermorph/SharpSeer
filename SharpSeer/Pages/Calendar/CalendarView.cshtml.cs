using System.Collections;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using SharpSeer.Interfaces;
using SharpSeer.Models;
using SharpSeer.Services;
using System.IO;
using Microsoft.IdentityModel.Tokens;
using SharpSeer;
using System.Runtime.CompilerServices;

namespace MyApp.Namespace
{
    public class CalendarViewModel : PageModel
    {
        public LinkedList<Exam> ExamButtons { get; set; }
        [BindProperty (SupportsGet = true)]
        public List<int> CohortsID { get; set; }
        [BindProperty (SupportsGet = true)]
        public List<int> TeachersID { get; set; }
        public IEnumerable<Teacher> Teachers { get; set; }
        public IEnumerable<Cohort> Cohorts { get; set; }
        [BindProperty]
        public int ExamButtonIndex { get; set; } = 0;
        public DateTime CurrentTime { get; set; } = DateTime.Now;
        public DateTime LastDateInMonth { get; set; }
		private DateTime m_selectedDateTime;
        [BindProperty (SupportsGet = true)]
        public CustomDateOnly SelectedDateTime { get; set; }
        public int DaysInMonth { get; set; } = 0;
        public int Year { get; set; } = 0;
        public int Month { get; set; } = 0;
        public string MonthStr { get; set; } = "";
        public int FirstDayOfMonth { get; set; } = 0;
        public List<string> WeekNames { get; set; }
        public string UpdateModal { get; set; } = "";
        [BindProperty (SupportsGet = true)]
        public Exam? SelectedExam { get; set; } = new Exam();
        [BindProperty]
        public Exam.ExamTypeEnum ExamType { get; set; }
        public bool ShowUpdate { get; set; } = false;
        public bool ShowSelected { get; set; } = false;
        private SharpSeerDbContext m_context;

        public IEnumerable<Cohort> CohortsAll { get; set; }
        public IEnumerable<Teacher> TeachersAll { get; set; }
        [BindProperty (SupportsGet = true)]
        public bool? IsGuarded { get; set; } = null;
        [BindProperty (SupportsGet = true)]
        public bool? NeedExternalExaminer { get; set; } = null;
        private IService<Exam> m_service;
        private IService<Teacher> m_teacherService;
        private IService<Cohort> m_cohortService;
        [BindProperty (SupportsGet = true)]
        public int TeacherID { get; set; }
        public Teacher? SelectedTeacher { get; set; }
        public Cohort? SelectedCohort { get; set; }
        public HashSet<int> OverlappingExams { get; set; } = new HashSet<int>();
        public List<Tuple<Exam, Teacher>>? OverlappingTeacher { get; set; } = null;
        public List<Tuple<Exam, Cohort>>? OverlappingCohort { get; set; } = null;
        public string OverlappingStr { get; set; } = "";
        public string ExamColor { get; set; } = "exam";
        public string ExamDisplayName { get; set; } = "";

        public CalendarViewModel(SharpSeerDbContext dbContext, IService<Exam> service, IService<Cohort> cohortService, IService<Teacher> teacherService)
        {
            m_service = service;
            Year = CurrentTime.Year;
            Month = CurrentTime.Month;
            WeekNames = new List<string>{"Mandag", "Tirsdag", "Onsdag", "Torsdag", "Fredag", "Lørdag", "Søndag"};
            m_context = dbContext;
            m_cohortService = cohortService;
            m_teacherService = teacherService;
        }
        public void OnGet()
        {
            ICollection<string> QKeys = HttpContext.Request.Query.Keys;
            foreach (string key in QKeys)
            {
                if (key == "month")
                {
                    HttpContext.Request.Query.TryGetValue(key, out var month);
                    if (!month.IsNullOrEmpty())
                    {
                        Month = int.Parse(month);
                    }
                    break;
                }
                if (key == "year")
                {
                    HttpContext.Request.Query.TryGetValue(key, out var year);
                    if (!year.IsNullOrEmpty())
                    {
                        Year = int.Parse(year);
                    }
                    break;
                }
            }
            GetMonth();
			GetDataFromDatabase();
            SetJunctionTable();
            GetOverlapping();
        }

        public void OnPostGetTeacher(int teacherID, int month, int year)
        {
            Month = month;
            Year = year;
            SelectedTeacher = m_teacherService.GetById(teacherID);
            Cohorts = m_cohortService.GetAll();
            Teachers = m_teacherService.GetAll();
            List<Teacher> teachers = new List<Teacher>(Teachers);
            int teacherToBeSwaped = teachers.FindIndex(t => t.Id == teacherID);
            teachers.RemoveAt(teacherToBeSwaped);
            Teachers = teachers.Prepend(SelectedTeacher);
            
            GetMonth();
            LastDateInMonth = new DateTime(Year, Month, DaysInMonth);
			IEnumerable<Exam> buttons = m_context.Exams.Include(e => e.Cohorts)
                .Include(e => e.Teachers).Where(e => e.FirstExamDate <= LastDateInMonth && e.LastExamDate >= m_selectedDateTime && e.Teachers.Any<Teacher>(t => t.Id == TeacherID))
                .OrderBy(e => e.FirstExamDate);
			ExamButtons = new LinkedList<Exam>(buttons);
        }

        public void OnPostGetCohort(int cohortID, int month, int year)
        {
            Month = month;
            Year = year;
            SelectedCohort = m_cohortService.GetById(cohortID);
            Cohorts = m_cohortService.GetAll();
            Teachers = m_teacherService.GetAll();
            List<Cohort> cohorts = new List<Cohort>(Cohorts);
            int cohortToBeSwaped = cohorts.FindIndex(c => c.Id == cohortID);
            cohorts.RemoveAt(cohortToBeSwaped);
            Cohorts = cohorts.Prepend(SelectedCohort);

            GetMonth();
            LastDateInMonth = new DateTime(Year, Month, DaysInMonth);
			IEnumerable<Exam> buttons = m_context.Exams.Include(e => e.Cohorts)
                .Include(e => e.Teachers).Where(e => e.FirstExamDate <= LastDateInMonth && e.LastExamDate >= m_selectedDateTime && 
                e.Cohorts.Any<Cohort>(t => t.Id == cohortID)).OrderBy(e => e.FirstExamDate);
			ExamButtons = new LinkedList<Exam>(buttons);
        }
     
        public IActionResult OnPostUpdate(int id, int month, int year)
        {
            Month = month;
            Year = year;
            SelectedExam.Id = id;
            SelectedExam.ExamType = (int)ExamType;

            if (IsGuarded.HasValue)
            {
                SelectedExam.IsGuarded = IsGuarded.Value;
            }
            if (NeedExternalExaminer.HasValue)
            {
                SelectedExam.NeedExternalExaminer = NeedExternalExaminer.Value;
            }
            
            foreach (var cohortId in CohortsID)
            {
                var cohort = m_cohortService.GetById(cohortId);
                SelectedExam.Cohorts.Add(cohort);
            }
            foreach (var teacherId in TeachersID)
            {
                var teacher = m_teacherService.GetById(teacherId);
                SelectedExam.Teachers.Add(teacher);
            }

            GetOverlapping();

            // List<Exam> exams = m_service.GetAll().Where(e => e.LastExamDate >= SelectedExam.FirstExamDate && e.FirstExamDate <= SelectedExam.LastExamDate).ToList();
            m_service.Update(SelectedExam);
            return Redirect($"/Calendar/CalendarView?month={Month}&year={Year}");
        }

        public async Task OnPostNextMonth(int month, int year)
        {
            Month = month + 1;
            Year = year;
            if (Month == 13)
            {
                Month = 1;
                Year = year + 1;
            }

            GetMonth();
			GetDataFromDatabase();
            SetJunctionTable();
            GetOverlapping();
        }

        public async Task OnPostPreviousMonth(int month, int year)
        {
            Month = month - 1;
            Year = year;
            if (Month == 0)
            {
                Month = 12;
                Year = year - 1;
            }

            GetMonth();
			GetDataFromDatabase();
            SetJunctionTable();
            GetOverlapping();
        }

        private void GetMonth()
        {
            DaysInMonth = DateTime.DaysInMonth(Year, Month);
            m_selectedDateTime = new DateTime(Year, Month, 1);
            SelectedDateTime = new CustomDateOnly(m_selectedDateTime);
            DayOfWeek dayOfWeek = m_selectedDateTime.DayOfWeek;
            FirstDayOfMonth = GetDayOfWeekAsNumber(dayOfWeek.ToString());
            MonthStr = GetMonthName(Month);
        }

		public void GetDataFromDatabase()
		{
			LastDateInMonth = new DateTime(Year, Month, DaysInMonth);
			IEnumerable<Exam> buttons = m_context.Exams.Include(e => e.Cohorts)
                .Include(e => e.Teachers).Where(e => e.FirstExamDate <= LastDateInMonth && e.LastExamDate >= m_selectedDateTime)
                .OrderBy(e => e.FirstExamDate);
			ExamButtons = new LinkedList<Exam>(buttons);
		}

        public void OnPostSelectedExam(int examID, int month, int year)
        {
            Year = year;
            Month = month;

            GetMonth();
            SelectedExam = m_service.GetById(examID);
            ExamType = (Exam.ExamTypeEnum)SelectedExam.ExamType;
            CohortsAll = m_cohortService.GetAll();
            TeachersAll = m_teacherService.GetAll();
            ShowUpdate = true;
            GetDataFromDatabase();
            SetJunctionTable();
            GetOverlapping();

            if (OverlappingExams.Contains(SelectedExam.Id) && OverlappingCohort != null && OverlappingTeacher != null)
            {
                string overStr = "";
                HashSet<string> overHash = new HashSet<string>();
                
                foreach (Tuple<Exam, Cohort> tuple in OverlappingCohort)
                {
                    if (tuple.Item1 == SelectedExam)
                    {
                        overStr = tuple.Item2.Name + ", ";
                        overHash.Add(overStr);
                    }
                }

                foreach (Tuple<Exam, Teacher> tuple in OverlappingTeacher)
                {
                    if (tuple.Item1 == SelectedExam)
                    {
                        overStr = tuple.Item2.Name + ", ";
                        overHash.Add(overStr);
                    }
                }
                
                foreach (string str in overHash)
                {
                    OverlappingStr += str;
                }
                OverlappingStr = OverlappingStr.Substring(0, OverlappingStr.Length - 2);
            }
        }

        public void UpdateDateTime(int day)
        {
            // Made a custom DateOnly for this to avoid unnecessary heap allocations
            SelectedDateTime.Day = day;
        }

        public void GetOverlapping()
        {
            List<Exam> exams = m_service.GetAll().ToList();
            // DateTime lastDayInMonth = new DateTime(Year, Month, DaysInMonth);
            // List<Exam> exams = m_service.GetAll().Where(e => e.FirstExamDate <= m_selectedDateTime && e.LastExamDate >= m_selectedDateTime ||
            // e.FirstExamDate >= m_selectedDateTime && e.FirstExamDate <= lastDayInMonth).ToList();

            for (int i = 0; i < exams.Count; i++)
            {
                for (int j = i + 1; j < exams.Count; j++)
                {
                    Exam examA = exams[i];
                    Exam examB = exams[j];

                    // Check if date ranges overlap
                    if (examA.LastExamDate >= examB.FirstExamDate && examA.FirstExamDate <= examB.LastExamDate)
                    {
                        // Check overlapping teachers
                        foreach (var teacher in examA.Teachers.Intersect(examB.Teachers))
                        {
                            if (examA.ExamType < 4 && examB.ExamType < 4)
                            {
                                OverlappingExams.Add(examA.Id);
                                OverlappingExams.Add(examB.Id);
                                OverlappingTeacher ??= new List<Tuple<Exam, Teacher>>();
                                OverlappingTeacher.Add(new Tuple<Exam, Teacher>(examA, teacher));
                                OverlappingTeacher.Add(new Tuple<Exam, Teacher>(examB, teacher));
                            }
                        }

                        // Check overlapping cohorts
                        foreach (var cohort in examA.Cohorts.Intersect(examB.Cohorts))
                        {
                            OverlappingExams.Add(examA.Id);
                            OverlappingExams.Add(examB.Id);
                            OverlappingCohort ??= new List<Tuple<Exam, Cohort>>();
                            OverlappingCohort.Add(new Tuple<Exam, Cohort>(examA, cohort));
                            OverlappingCohort.Add(new Tuple<Exam, Cohort>(examB, cohort));
                        }
                    }
                }
            }
        }

        public void GetExamType(Exam exam)
        {
            switch ((Exam.ExamTypeEnum)exam.ExamType)
            {
                case Exam.ExamTypeEnum.Skriftlig:
                    ExamColor = "exam";
                    ExamDisplayName = "📝 " + exam.Name;
                    break;
                case Exam.ExamTypeEnum.Mundtlig:
                    ExamColor = "exam";
                    ExamDisplayName = "🗣️ " + exam.Name;
                    break;
                case Exam.ExamTypeEnum.Projekt:
                    ExamColor = "exam";
                    ExamDisplayName = "💻 " + exam.Name;
                    break;
                case Exam.ExamTypeEnum.Skriftlig_Re_Examen:
                    ExamColor = "re-exam";
                    ExamDisplayName = "🔁📝 " + exam.Name;
                    break;
                case Exam.ExamTypeEnum.Mundtlig_Re_Examen:
                    ExamColor = "re-exam";
                    ExamDisplayName = "🔁🗣️ " + exam.Name;
                    break;
                case Exam.ExamTypeEnum.Projekt_Re_Examen:
                    ExamColor = "re-exam";
                    ExamDisplayName = "🔁💻 " + exam.Name;
                    break;
                case Exam.ExamTypeEnum.Afsluttende:
                    ExamColor = "finalExam";
                    ExamDisplayName = "🎓 " + exam.Name;
                    break;
                case Exam.ExamTypeEnum.Afsluttende_Re_Examen:
                    ExamColor = "re-finalExam";
                    ExamDisplayName = "🔁🎓 " + exam.Name;
                    break;
            }
        }

        public void SetJunctionTable()
        {
            CohortsID = SelectedExam.Cohorts?.Select(c => c.Id).ToList() ?? new List<int>();
            TeachersID = SelectedExam.Teachers?.Select(t => t.Id).ToList() ?? new List<int>();
            Teachers = m_teacherService.GetAll();
            Cohorts = m_cohortService.GetAll();
        }

        public string GetMonthName(int month)
        {
            string monthStr = "";
            switch (Month)
            {
                case 1:
                    monthStr = "Januar";
                    break;
                case 2:
                    monthStr = "Februar";
                    break;
                case 3:
                    monthStr = "Marts";
                    break;
                case 4:
                    monthStr = "April";
                    break;
                case 5:
                    monthStr = "Maj";
                    break;
                case 6:
                    monthStr = "Juni";
                    break;
                case 7:
                    monthStr = "Juli";
                    break;
                case 8:
                    monthStr = "August";
                    break;
                case 9:
                    monthStr = "September";
                    break;
                case 10:
                    monthStr = "Oktober";
                    break;
                case 11:
                    monthStr = "November";
                    break;
                case 12:
                    monthStr = "December";
                    break;
            }
            return monthStr;
        }

        public int GetDayOfWeekAsNumber(string dayOfWeek)
        {
            int day = 0;
            switch (dayOfWeek)
            {
                case "Monday":
                    day = 1;
                    break;
                case "Tuesday":
                    day = 2;
                    break;
                case "Wednesday":
                    day = 3;
                    break;
                case "Thursday":
                    day = 4;
                    break;
                case "Friday":
                    day = 5;
                    break;
                case  "Saturday":
                    day = 6;
                    break;
                case "Sunday":
                    day = 7;
                    break;
            }
            return day;
        }
    }
}
