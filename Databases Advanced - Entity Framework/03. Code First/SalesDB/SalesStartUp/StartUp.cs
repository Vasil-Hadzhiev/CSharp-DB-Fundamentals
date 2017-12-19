namespace SalesStartUp
{
    using P03_SalesDatabase.Data;

    public class StartUp
    {
        public static void Main()
        {
            var db = new SalesContext();

            using (db)
            {
                db.Database.EnsureCreated();
            }
        }
    }
}