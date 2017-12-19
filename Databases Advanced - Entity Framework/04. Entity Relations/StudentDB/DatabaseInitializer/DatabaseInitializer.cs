namespace DatabaseInitializer
{
    using global::DatabaseInitializer.Generators;
    using P01_StudentSystem.Data;
    using System;

    public class DatabaseInitializer
    {
        private static Random rnd = new Random();

        public static void ResetDatabase()
        {
            using (var db = new StudentSystemContext())
            {
                db.Database.EnsureDeleted();

                db.Database.EnsureCreated();

                InitialSeed(db);
            }
        }

        public static void InitialSeed(StudentSystemContext db)
        {
            SeedStudents(db, 100);

            SeedCourses(db, 30);

            SeedStudentsCourses(db, 120);

            SeedHomeworks(db, 150);

            SeedResources(db);
        }

        private static void SeedStudentsCourses(StudentSystemContext db, int count)
        {
            StudentCoursesGenerator.InitialStudentCoursesSeed(db, count);
        }

        private static void SeedResources(StudentSystemContext db)
        {
            ResourceGenerator.InitialResourseSeed(db);
        }

        private static void SeedHomeworks(StudentSystemContext db, int count)
        {
            HomeworkGenerator.InitialHomeworkSeed(db, count);
        }

        private static void SeedCourses(StudentSystemContext db, int count)
        {
            CourseGenerator.InitialCourseSeed(db, count);
        }

        private static void SeedStudents(StudentSystemContext db, int count)
        {
            StudentGenerator.InitialStudentSeed(db, count);
        }
    }
}