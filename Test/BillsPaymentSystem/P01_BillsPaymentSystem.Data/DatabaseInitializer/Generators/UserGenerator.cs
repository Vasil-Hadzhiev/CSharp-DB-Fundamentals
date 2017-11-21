namespace P01_BillsPaymentSystem.Data.DatabaseInitializer.Generators
{
    using P01_BillsPaymentSystem.Data.Models;
    using System;

    public class UserGenerator
    {
        private static Random rnd = new Random();

        private static string[] firstNames =
        {
            "Vasil",
            "Petur",
            "Stoyan",
            "Georgi",
            "Velizar",
            "Stamat",
            "Dimitar",
            "Martin",
            "Plamen",
            "Ivan"
        };

        private static string[] lastNames =
        {
            "Hadzhiev",
            "Genov",
            "Angelov",
            "Petrov",
            "Ivanov",
            "Maznev",
            "Stamatov"
        };

        private static string[] emails =
        {
            "redsnake@gmail.com",
            "wowzers@gmail.com",
            "wutface@gmail.com",
            "whyalwaysme@gmail.com",
            "batman@gmail.com",
            "theflash@gmail.com",
            "arrow@gmail.com",
            "superman@gmail.com",
            "hulksmash@gmail.com"
        };

        private static string[] passwords =
        {
            "veryhardpassword",
            "ultraeasypassword!",
            "iloveazis!",
            "isecretelyloveazis!",
            "ihateazis",
            "123asd456",
            "qwerty987",
            "jijibiji",
            "shestshestici",
            "sedemsedmici"
        };

        internal static void InitialUserSeed(BillsPaymentSystemContext db, int count)
        {
            for (int i = 0; i < count; i++)
            {
                db.Users.Add(NewUser());
                db.SaveChanges();
            }
        }

        private static User NewUser()
        {
            User user = new User()
            {
                FirstName = GenerateUserFirstName(),
                LastName = GenerateUserLastName(),
                Email = GenerateEmail(),
                Password = GeneratePassword()
            };

            return user;
        }

        private static string GeneratePassword()
        {
            return passwords[rnd.Next(0, passwords.Length)];
        }

        private static string GenerateEmail()
        {
            return emails[rnd.Next(0, emails.Length)];
        }

        private static string GenerateUserLastName()
        {
            return lastNames[rnd.Next(0, lastNames.Length)];
        }

        private static string GenerateUserFirstName()
        {
            return firstNames[rnd.Next(0, firstNames.Length)];
        }
    }
}