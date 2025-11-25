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

namespace MyApp.Namespace
{
    public class CalendarViewModel : PageModel
    {
        public LinkedList<Exam> ExamButtons { get; set; }
		public List<Exam> ExamToBeDeleted { get; set; }
        [BindProperty]
        public int ExamButtonIndex { get; set; } = 0;
        public DateTime CurrentTime { get; set; } = DateTime.Now;
        public DateTime LastDateInMonth { get; set; }
		private DateTime m_selectedDateTime;
        public int DaysInMonth { get; set; } = 0;
        public int Year { get; set; } = 0;
        public int Month { get; set; } = 0;
        public string MonthStr { get; set; } = "";
        public int FirstDayOfMonth { get; set; } = 0;
        public List<string> WeekNames { get; set; }
        public string UpdateModal { get; set; } = "";
        public Exam SelectedExam { get; set; } = new Exam();
        public Exam.ExamTypeEnum ExamType { get; set; }
        public bool ShowUpdate { get; set; } = false;
        public bool ShowSelected { get; set; } = false;
        private SharpSeerDbContext m_context;
        public IEnumerable<Cohort> CohortsAll { get; set; }
        public IEnumerable<Teacher> TeachersAll { get; set; }
        public bool IsGuarded { get; set; } = true;
        public bool NeedExternalExaminer { get; set; } = true;
        private IService<Exam> m_service;
        private IService<Teacher> m_teacherService;
        private IService<Cohort> m_cohortService;

        public CalendarViewModel(SharpSeerDbContext dbContext, IService<Exam> service, IService<Cohort> cohortService, IService<Teacher> teacherService)
        {
            m_service = service;
            DaysInMonth = DateTime.DaysInMonth(CurrentTime.Year, CurrentTime.Month);
            Year = CurrentTime.Year;
            Month = CurrentTime.Month;
            WeekNames = new List<string>{"Mandag", "Tirsdag", "Onsdag", "Torsdag", "Fredag", "Lørdag", "Søndag"};
            m_context = dbContext;
            m_cohortService = cohortService;
            m_teacherService = teacherService;
        }
        public void OnGet()
        {
            MonthStr = GetMonthName(Month);

            m_selectedDateTime = new DateTime(Year, Month, 1);
            DayOfWeek dayOfWeek = m_selectedDateTime.DayOfWeek;
            FirstDayOfMonth = GetDayOfWeekAsNumber(dayOfWeek.ToString());
			GetDataFromDatabase();
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

            DaysInMonth = DateTime.DaysInMonth(Year, Month);
            m_selectedDateTime = new DateTime(Year, Month, 1);
            DayOfWeek dayOfWeek = m_selectedDateTime.DayOfWeek;
            FirstDayOfMonth = GetDayOfWeekAsNumber(dayOfWeek.ToString());
            MonthStr = GetMonthName(Month);
			GetDataFromDatabase();
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

            DaysInMonth = DateTime.DaysInMonth(Year, Month);

            m_selectedDateTime = new DateTime(Year, Month, 1);
            DayOfWeek dayOfWeek = m_selectedDateTime.DayOfWeek;
            FirstDayOfMonth = GetDayOfWeekAsNumber(dayOfWeek.ToString());

            MonthStr = GetMonthName(Month);
			GetDataFromDatabase();
        }

		public void GetDataFromDatabase()
		{
			LastDateInMonth = new DateTime(Year, Month, DaysInMonth);
			IEnumerable<Exam> buttons = m_context.Exams.Include(e => e.Cohorts)
                .Include(e => e.Teachers).Where(e => e.FirstExamDate <= LastDateInMonth && e.LastExamDate >= m_selectedDateTime)
                .OrderBy(e => e.FirstExamDate);
			ExamButtons = new LinkedList<Exam>(buttons);
			ExamToBeDeleted = new List<Exam>(3);
		}

        public void OnPostSelectedExam(int examID, int month, int year)
        {
            Year = year;
            Month = month;
            DaysInMonth = DateTime.DaysInMonth(Year, Month);

            m_selectedDateTime = new DateTime(Year, Month, 1);
            DayOfWeek dayOfWeek = m_selectedDateTime.DayOfWeek;
            FirstDayOfMonth = GetDayOfWeekAsNumber(dayOfWeek.ToString());

            MonthStr = GetMonthName(Month);

            SelectedExam = m_service.GetById(examID);
            ExamType = (Exam.ExamTypeEnum)SelectedExam.ExamType;
            CohortsAll = m_cohortService.GetAll();
            TeachersAll = m_teacherService.GetAll();
            ShowUpdate = true;
            GetDataFromDatabase();
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
