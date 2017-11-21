namespace P01_BillsPaymentSystem.Data.DatabaseInitializer.Generators
{
    using System;

    public class DateGenerator
    {
        private static Random rnd = new Random();

        public static DateTime GenerateDate()
        {
            var startDate = new DateTime(1980, 01, 01);
            var endDate = new DateTime(2010, 01, 01);

            var daysDifference = (endDate - startDate).Days;

            var date = startDate.AddDays(rnd.Next(0, daysDifference));

            return date;
        }

        public static DateTime GenerateEndDate(DateTime startDate)
        {

            var date = startDate.AddDays(rnd.Next(30, 60));

            return date;
        }
    }
}