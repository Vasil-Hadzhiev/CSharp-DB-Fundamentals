namespace DatabaseInitializer.Generators
{
    using P01_StudentSystem.Data;
    using P01_StudentSystem.Data.Enums;
    using P01_StudentSystem.Data.Models;
    using System;
    using System.Linq;

    public class HomeworkGenerator
    {
        private static Random rnd = new Random();

        private static string[] contents =
        {
            "C# OOP homework",
            "DB Basic homework",
            "DB Advanced homework",
            "Java OOP homework",
            "C# OOP Advanced homework",
            "Java OOP Advanced homework"
        };

        internal static void InitialHomeworkSeed(StudentSystemContext db, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var homework = NewHomework();

                db.HomeworkSubmissions.Add(homework);
                db.SaveChanges();
            }
        }

        private static Homework NewHomework()
        {
            var homework = new Homework()
            {
                Content = NewContent(),
                SubmissionTime = DateGenerator.GenerateDate(),
                ContentType = GetRandomContentType(),
                StudentId = GetRandomStudentFromDb(),
                CourseId = GetRandomCourseFromDb()
            };

            return homework;
        }

        private static int GetRandomCourseFromDb()
        {
            var db = new StudentSystemContext();

            using (db)
            {
                var coursesIds = db
                    .Courses
                    .Select(c => c.CourseId)
                    .ToArray();

                return coursesIds[rnd.Next(0, coursesIds.Length)];
            }
        }

        private static int GetRandomStudentFromDb()
        {
            var db = new StudentSystemContext();

            using (db)
            {
                var studentsIds = db
                    .Students
                    .Select(s => s.StudentId)
                    .ToArray();

                return studentsIds[rnd.Next(0, studentsIds.Length)];
            }
        }

        private static ContentType GetRandomContentType()
        {
            ContentType[] types = new ContentType[]
            {
                ContentType.Application,
                ContentType.Pdf,
                ContentType.Zip
            };

            return types[rnd.Next(0, types.Length)];
        }

        private static string NewContent()
        {
            return contents[rnd.Next(0, contents.Length)];
        }
    }
}