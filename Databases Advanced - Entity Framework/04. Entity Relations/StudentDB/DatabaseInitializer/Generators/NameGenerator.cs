namespace DatabaseInitializer.Generators
{
    using System;

    public class NameGenerator
    {
        private static string[] firstNames =
            { "Vasil", "Petur", "Georgi", "Stoian", "Martin", "Plamen", "Dimitar" };

        private static string[] lastNames =
            { "Hadzhiev", "Genov", "Angelov", "Maznev", "Ivanov" };

        public static string FirstName() => GenerateName(firstNames);
        public static string LastName() => GenerateName(lastNames);

        private static string GenerateName(string[] names)
        {
            var rnd = new Random();

            var index = rnd.Next(0, names.Length);

            var name = names[index];

            return name;
        }
    }
}