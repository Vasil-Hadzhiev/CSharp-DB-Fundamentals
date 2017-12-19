namespace P03__FootballBetting.Data
{
    public class Configuration
    {
        public static string ConnectionString { get; set; } =
            "Server=VASIL\\SQLEXPRESS;Database=FootballBettingDB;Integrated Security = true";
    }
}
