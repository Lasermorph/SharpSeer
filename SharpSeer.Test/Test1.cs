using SharpSeer.Interfaces;
using SharpSeer.Models;
using SharpSeer.Services;
namespace SharpSeer.Test;

[TestClass]
public sealed class Test1
{
    SharpSeerDbContext m_dbContext;
    ExamService m_examService;
    public HashSet<int> OverlappingExams { get; set; } = new HashSet<int>();
    public List<Tuple<Exam, Teacher>>? OverlappingTeacher { get; set; } = null;
    public List<Tuple<Exam, Cohort>>? OverlappingCohort { get; set; } = null;

    public Test1()
    {
        m_dbContext = new SharpSeerDbContext();
        m_examService = new ExamService(m_dbContext);
    }

    [TestMethod]
    public void TestMethod1()
    {
        List<Exam> exams = new List<Exam>();
        Cohort cohort1 = new Cohort();
        cohort1.Name = "Cohort1";
        cohort1.Id = 2;
        Teacher teacher1 = new Teacher();
        teacher1.Name = "Lars";
        Exam exam1 = new Exam();
        exam1.Id = 1;
        exam1.Cohorts.Add(cohort1);
        exam1.Teachers.Add(teacher1);
        exam1.FirstExamDate = new DateTime(2025, 4, 20);
        exam1.LastExamDate = new DateTime(2025, 4, 25);
        exam1.ExamType = 1;

        Exam exam2 = new Exam();
        exam2.Id = 2;
        exam2.Cohorts.Add(cohort1);
        exam2.Teachers.Add(teacher1);
        exam2.FirstExamDate = new DateTime(2025, 3, 2);
        exam2.LastExamDate = new DateTime(2025, 6, 6);
        exam2.ExamType = 2;

        exams.Add(exam1);
        exams.Add(exam2);

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


        Assert.IsTrue(OverlappingExams.Any(), "there is no exams");
        Assert.IsTrue(OverlappingCohort.Any(), "Cohorts collection is empty");
        Assert.IsTrue(OverlappingTeacher.Any(), "Teacher collection is empty");
    }
}
