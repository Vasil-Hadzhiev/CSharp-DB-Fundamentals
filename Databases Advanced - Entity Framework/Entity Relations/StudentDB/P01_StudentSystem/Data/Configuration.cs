namespace P01_StudentSystem.Data
{
    public class Configuration
    {
        public static string ConnectionString { get; set; } =
            "Server=VASIL\\SQLEXPRESS;Database=StudentSystem;Integrated Security = true";
    }
}