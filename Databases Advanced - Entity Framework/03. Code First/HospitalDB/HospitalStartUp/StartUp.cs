namespace HospitalStartUp
{
    using P01_HospitalDatabase.Initializer;

    public class StartUp
    {
        public static void Main()
        {
            DatabaseInitializer.ResetDatabase();

            //var db = new HospitalContext();

            //using (db)
            //{
            //    DatabaseInitializer.InitialSeed(db);
            //}
        }
    }
}

