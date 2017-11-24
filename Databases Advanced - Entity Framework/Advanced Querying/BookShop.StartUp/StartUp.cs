namespace BookShop
{
    using BookShop.Data;
    using BookShop.Initializer;
    using BookShop.Models;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            //For problem 1
            //var command = Console.ReadLine();

            //For problem 4
            //var year = int.Parse(Console.ReadLine());

            //For problem 5
            //var categories = Console.ReadLine();

            //For problem 6
            //var date = Console.ReadLine();

            //For problems 7, 8, 9
            //var input = Console.ReadLine();

            //For problem 10
            //var length = int.Parse(Console.ReadLine());

            var result = string.Empty;

            using (var db = new BookShopContext())
            {
                //DbInitializer.ResetDatabase(db);

                //Problem 1
                //result = GetBooksByAgeRestriction(db, command);

                //Problem 2
                //result = GetGoldenBooks(db);

                //Problem 3
                //result = GetBooksByPrice(db);

                //Problem 4
                //result = GetBooksNotReleasedIn(db, year);

                //Problem 5
                //result = GetBooksByCategory(db, categories);

                //Problem 6
                //result = GetBooksReleasedBefore(db, date);

                //Problem 7
                //result = GetAuthorNamesEndingIn(db, input);

                //Problem 8
                //result = GetBookTitlesContaining(db, input);

                //Problem 9
                //result = GetBooksByAuthor(db, input);

                //Problem 10
                //var count = 0;
                //count = CountBooks(db, length);

                //Problem 11
                //result = CountCopiesByAuthor(db);

                Console.WriteLine(result);
            }
        }

        private static string CountCopiesByAuthor(BookShopContext db)
        {
            var authors = db.Authors
                .Select(a => new
                {
                    Name = $"{a.FirstName} {a.LastName}",
                    Copies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.Copies)
                .ToList();

            var sb = new StringBuilder();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.Name} - {author.Copies}");
            }

            return sb.ToString().Trim();
        }

        private static int CountBooks(BookShopContext db, int length)
        {
            var booksCount = db.Books
                .Where(b => b.Title.Length > length)
                .Count();

            return booksCount;
        }

        private static string GetBooksByAuthor(BookShopContext db, string input)
        {
            var books = db.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    b.Title,
                    Author = $"{b.Author.FirstName} {b.Author.LastName}"
                })
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.Author})");
            }

            return sb.ToString().Trim();
        }

        private static string GetBookTitlesContaining(BookShopContext db, string input)
        {
            var books = db.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().Trim();
        }

        private static string GetAuthorNamesEndingIn(BookShopContext db, string input)
        {
            var authors = db.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    Name = $"{a.FirstName} {a.LastName}"
                })
                .OrderBy(a => a.Name)
                .ToList();

            var sb = new StringBuilder();

            foreach (var author in authors)
            {
                sb.AppendLine(author.Name);
            }

            return sb.ToString().Trim();
        }

        private static string GetBooksReleasedBefore(BookShopContext db, string date)
        {
            var formatString = "dd-MM-yyyy";

            var parsedDate = DateTime.ParseExact(date, formatString, CultureInfo.InvariantCulture);

            var books = db.Books
                .Where(b => b.ReleaseDate < parsedDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price
                })
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:f2}");
            }

            return sb.ToString().Trim();
        }

        private static string GetBooksByCategory(BookShopContext db, string categories)
        {
            var categoryTokens = categories
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLower())
                .ToList();

            var books = db.Books
                .Where(b => b.BookCategories.Any(c => categoryTokens.Contains(c.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().Trim();
        }

        private static string GetBooksNotRealesedIn(BookShopContext db, int year)
        {
            var books = db.Books
                .Where(b => DateTime.Parse(b.ReleaseDate.ToString()).Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString();
        }

        private static string GetBooksByPrice(BookShopContext db)
        {
            var books = db.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return sb.ToString().Trim();
        }

        private static string GetGoldenBooks(BookShopContext db)
        {
            var type = (EditionType)Enum.Parse(typeof(EditionType), "Gold", true);

            var books = db.Books
                .Where(b => b.EditionType == type && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().Trim();
        }

        private static string GetBooksByAgeRestriction(BookShopContext db, string command)
        {
            var restriction = (AgeRestriction)Enum.Parse(typeof(AgeRestriction), command, true);

            var books = db.Books
                .Where(b => b.AgeRestriction == restriction)
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().Trim();
        }
    }
}