namespace DatabaseInitializer.Generators
{
    using P01_StudentSystem.Data;
    using P01_StudentSystem.Data.Enums;
    using P01_StudentSystem.Data.Models;
    using System;
    using System.Linq;

    public class ResourceGenerator
    {
        private static Random rnd = new Random();

        private static string[] resourceNames =
        {
            "C# OOP Basic - Class Definition",
            "C# OOP Basic - Encapsulation",
            "C# OOP Basic - Inheritance",
            "C# OOP Basic - Polymorphism",
            "C# OOP Basic - Abstraction",
            "C# OOP Advanced - Interfaces",
            "C# OOP Advanced - Generics",
            "C# OOP Advanced - Iterators and Comparators",
            "C# OOP Advanced - Reflection",
            "DB Advanced - Introduction to EF Core",
            "DB Advanced - Code First",
            "DB Advanced - Entity Relations"
        };

        private static ResourceType[] types =
        {
            ResourceType.Presentation,
            ResourceType.Video,
            ResourceType.Document,
            ResourceType.Other
        };

        internal static void InitialResourseSeed(StudentSystemContext db)
        {
            var coursesIds = db
                .Courses
                .Select(c => c.CourseId)
                .ToArray();

            for (int i = 0; i < resourceNames.Length; i++)
            {
                var resource = new Resource()
                {
                    Name = resourceNames[i],
                    Url = "E:\\Resources\\" + resourceNames[i],
                    ResourceType = types[rnd.Next(0, types.Length)],
                    CourseId = coursesIds[rnd.Next(0, coursesIds.Length)]
                };

                db.Resources.Add(resource);

                db.SaveChanges();
            }
        }
    }
}