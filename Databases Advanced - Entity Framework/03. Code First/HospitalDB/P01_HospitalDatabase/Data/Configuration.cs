namespace P01_HospitalDatabase.Data
{
    public class Configuration
    {
        public static string ConnectionString { get; set; } = 
            "Server=VASIL\\SQLEXPRESS;Database=Hospital;Integrated Security = true";
    }
}
