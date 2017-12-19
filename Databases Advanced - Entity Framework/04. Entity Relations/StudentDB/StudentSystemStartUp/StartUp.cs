namespace StudentSystemStartUp
{
    using DatabaseInitializer;

    public class StartUp
    {
        public static void Main()
        {
            DatabaseInitializer.ResetDatabase();
        }
    }
}