namespace P03_FootballBetting
{
    using P03_FootballBetting.Data;

    public class StartUp
    {
        public static void Main()
        {
            var db = new FootballBettingContext();

            using (db)
            {
                db.Database.EnsureDeleted();

                db.Database.EnsureCreated();
            }
        }
    }
}